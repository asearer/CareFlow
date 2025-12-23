using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;

namespace CareFlow.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _repository;

    public AuditService(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task LogAsync(string action, string entityName, Guid entityId, Guid? userId, string details)
    {
        var log = new AuditLog(action, entityName, entityId, userId, details);
        await _repository.AddAsync(log, CancellationToken.None);
        // We might want to save changes here immediately or let UnitOfWork handle it. 
        // For simplicity and decoupling, let's assume this is part of the same transaction scope as the UoW in the handler.
        // However, Repository AddAsync just adds to ChangeTracker.
        // If we want separate save, we need access to context.
        // Given Clean Archi structure, usually Handler calls SaveChanges on UnitOfWork. 
        // So this service just adds to the context.
    }
}
