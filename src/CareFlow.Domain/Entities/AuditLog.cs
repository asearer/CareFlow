namespace CareFlow.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public string Action { get; private set; } // e.g., "CreatePatient", "SignNote"
    public string EntityName { get; private set; } // e.g., "Patient", "ClinicalNote"
    public Guid EntityId { get; private set; }
    public Guid? UserId { get; private set; } // Who performed the action
    public DateTime Timestamp { get; private set; }
    public string Details { get; private set; } // JSON or details

    private AuditLog() { }

    public AuditLog(string action, string entityName, Guid entityId, Guid? userId, string details)
    {
        Id = Guid.NewGuid();
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        UserId = userId;
        Timestamp = DateTime.UtcNow;
        Details = details;
    }
}
