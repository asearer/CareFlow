using CareFlow.Domain.Enums;
using CareFlow.Domain.Exceptions;
using CareFlow.Domain.ValueObjects;

namespace CareFlow.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid TherapistId { get; private set; }
    public DateRange TimeRange { get; private set; }
    public AppointmentStatus Status { get; private set; }

    // Navigation properties (optional, good for EF)
    public virtual Patient Patient { get; private set; }
    public virtual User Therapist { get; private set; }

    private Appointment() { }

    public Appointment(Guid patientId, Guid therapistId, DateRange timeRange)
    {
        Id = Guid.NewGuid();
        PatientId = patientId;
        TherapistId = therapistId;
        TimeRange = timeRange;
        Status = AppointmentStatus.Scheduled;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
        {
            throw new DomainException("Cannot cancel a completed appointment.");
        }
        Status = AppointmentStatus.Canceled;
    }

    public void Complete()
    {
        if (Status == AppointmentStatus.Canceled)
        {
            throw new DomainException("Cannot complete a canceled appointment.");
        }
        Status = AppointmentStatus.Completed;
    }
}
