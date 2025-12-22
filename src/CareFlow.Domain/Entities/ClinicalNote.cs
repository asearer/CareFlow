using CareFlow.Domain.Enums;
using CareFlow.Domain.Exceptions;

namespace CareFlow.Domain.Entities;

public class ClinicalNote
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid TherapistId { get; private set; }
    public Guid AppointmentId { get; private set; }
    
    public string Subjective { get; private set; }
    public string Objective { get; private set; }
    public string Assessment { get; private set; }
    public string Plan { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public NoteStatus Status { get; private set; }

    // Navigation
    public virtual Patient Patient { get; private set; }
    public virtual User Therapist { get; private set; }
    public virtual Appointment Appointment { get; private set; }

    private ClinicalNote() { }

    public ClinicalNote(Guid patientId, Guid therapistId, Guid appointmentId, string subjective, string objective, string assessment, string plan)
    {
        Id = Guid.NewGuid();
        PatientId = patientId;
        TherapistId = therapistId;
        AppointmentId = appointmentId;
        Subjective = subjective;
        Objective = objective;
        Assessment = assessment;
        Plan = plan;
        CreatedAt = DateTime.UtcNow;
        Status = NoteStatus.Draft;
    }

    public void UpdateContent(string subjective, string objective, string assessment, string plan)
    {
        if (Status == NoteStatus.Signed)
        {
            throw new DomainException("Cannot update a signed note.");
        }
        Subjective = subjective;
        Objective = objective;
        Assessment = assessment;
        Plan = plan;
    }

    public void Sign()
    {
        if (Status == NoteStatus.Signed)
        {
            throw new DomainException("Note is already signed.");
        }
        Status = NoteStatus.Signed;
    }
}
