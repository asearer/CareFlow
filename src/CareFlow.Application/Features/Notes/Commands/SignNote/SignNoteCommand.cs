using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Interfaces;
using MediatR;

namespace CareFlow.Application.Features.Notes.Commands.SignNote;

public record SignNoteCommand(Guid NoteId, Guid TherapistId) : IRequest<bool>;

public class SignNoteCommandHandler : IRequestHandler<SignNoteCommand, bool>
{
    private readonly IClinicalNoteRepository _noteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public SignNoteCommandHandler(IClinicalNoteRepository noteRepository, IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _noteRepository = noteRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task<bool> Handle(SignNoteCommand command, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(command.NoteId, cancellationToken);
        if (note == null)
        {
            throw new Exception("Note not found.");
        }

        if (note.TherapistId != command.TherapistId)
        {
            throw new Exception("Unauthorized to sign this note.");
        }

        note.Sign();
        
        await _auditService.LogAsync(
            action: "SignNote",
            entityName: "ClinicalNote",
            entityId: note.Id,
            userId: command.TherapistId,
            details: "Note signed and finalized.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
