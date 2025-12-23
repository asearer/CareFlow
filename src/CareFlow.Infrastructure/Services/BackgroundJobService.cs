using CareFlow.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CareFlow.Infrastructure.Services;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;
    private readonly IAuditService _auditService;

    public BackgroundJobService(ILogger<BackgroundJobService> logger, IAuditService auditService)
    {
        _logger = logger;
        _auditService = auditService;
    }

    public async Task SendAppointmentReminders()
    {
        _logger.LogInformation("Sending appointment reminders for tomorrow...");
        await _auditService.LogAsync("JobExecution", "HangfireJob", Guid.NewGuid(), null, "Sent appointment reminders");
    }

    public async Task ProcessDailySummaries()
    {
        _logger.LogInformation("Processing daily summaries...");
        await _auditService.LogAsync("JobExecution", "HangfireJob", Guid.NewGuid(), null, "Processed daily summaries");
    }
}
