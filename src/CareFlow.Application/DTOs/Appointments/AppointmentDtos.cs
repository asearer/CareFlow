namespace CareFlow.Application.DTOs.Appointments;

public record CreateAppointmentRequest(Guid PatientId, Guid TherapistId, DateTime StartTime, DateTime EndTime);
public record AppointmentDto(Guid Id, Guid PatientId, string PatientName, Guid TherapistId, string TherapistName, DateTime StartTime, DateTime EndTime, string Status);
