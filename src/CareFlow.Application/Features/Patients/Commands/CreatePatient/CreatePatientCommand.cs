using CareFlow.Domain.Entities;
using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CareFlow.Application.Features.Patients.Commands.CreatePatient;

public record CreatePatientCommand(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address) : IRequest<Guid>;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(v => v.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(v => v.LastName).NotEmpty().MaximumLength(100);
        RuleFor(v => v.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(v => v.DateOfBirth).NotEmpty();
        RuleFor(v => v.PhoneNumber).NotEmpty();
        RuleFor(v => v.Address).NotEmpty();
    }
}

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public CreatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient(
            request.FirstName, 
            request.LastName, 
            request.DateOfBirth.ToUniversalTime(), // Ensure UTC
            request.Email, 
            request.PhoneNumber, 
            request.Address);

        await _patientRepository.AddAsync(patient, cancellationToken);
        
        await _auditService.LogAsync(
            action: "CreatePatient",
            entityName: "Patient",
            entityId: patient.Id,
            userId: null, // In a real app we'd get this from CurrentUserService
            details: $"Created patient {patient.FirstName} {patient.LastName}");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patient.Id;
    }
}
