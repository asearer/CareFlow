namespace CareFlow.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(string action, string entityName, Guid entityId, Guid? userId, string details);
}
