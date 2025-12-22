using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Interfaces;
using CareFlow.Application.DTOs.Notes;
using CareFlow.Domain.Entities;
using MediatR;

namespace CareFlow.Application.Features.Notes.Commands.CreateNote;

public record CreateNoteCommand(CreateNoteRequest Request, Guid TherapistId) : IRequest<Guid>;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly IClinicalNoteRepository _noteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNoteCommandHandler(IClinicalNoteRepository noteRepository, IUnitOfWork unitOfWork)
    {
        _noteRepository = noteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var note = new ClinicalNote(
            command.Request.PatientId,
            command.TherapistId,
            command.Request.AppointmentId,
            command.Request.Subjective,
            command.Request.Objective,
            command.Request.Assessment,
            command.Request.Plan
        );

        await _noteRepository.AddAsync(note, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}
