namespace CareFlow.Application.DTOs.Notes;

public record CreateNoteRequest(Guid PatientId, Guid AppointmentId, string Subjective, string Objective, string Assessment, string Plan);
public record UpdateNoteRequest(Guid Id, string Subjective, string Objective, string Assessment, string Plan);
public record ClinicalNoteDto(Guid Id, Guid PatientId, string PatientName, DateTime CreatedAt, string Subjective, string Objective, string Assessment, string Plan, string Status);
