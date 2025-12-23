namespace CareFlow.Application.Common.Interfaces;

public interface IBackgroundJobService
{
    Task SendAppointmentReminders();
    Task ProcessDailySummaries();
}
