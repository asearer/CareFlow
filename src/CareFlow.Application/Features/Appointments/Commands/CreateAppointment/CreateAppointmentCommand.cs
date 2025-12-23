using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Interfaces;
using CareFlow.Application.DTOs.Appointments;
using CareFlow.Domain.Entities;
using CareFlow.Domain.ValueObjects;
using MediatR;

namespace CareFlow.Application.Features.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(CreateAppointmentRequest Request) : IRequest<Guid>;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        var timeRange = new DateRange(command.Request.StartTime, command.Request.EndTime);

        // Check for conflicts
        var conflicts = await _appointmentRepository.GetOverlappingAppointmentsAsync(
            command.Request.TherapistId, 
            timeRange, 
            cancellationToken);

        if (conflicts.Any())
        {
            throw new Exception("Therapist is already booked for this time range.");
        }

        var appointment = new Appointment(
            command.Request.PatientId,
            command.Request.TherapistId,
            timeRange
        );

        await _appointmentRepository.AddAsync(appointment, cancellationToken);
        
        await _auditService.LogAsync(
            action: "CreateAppointment",
            entityName: "Appointment",
            entityId: appointment.Id,
            userId: command.Request.TherapistId,
            details: $"Scheduled appointment for {timeRange.Start}");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}
