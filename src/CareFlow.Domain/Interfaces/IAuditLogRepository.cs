using CareFlow.Domain.Entities;

namespace CareFlow.Domain.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken cancellationToken);
}
