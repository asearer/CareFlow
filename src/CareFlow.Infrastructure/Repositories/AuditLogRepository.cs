using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using CareFlow.Infrastructure.Persistence;

namespace CareFlow.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log, CancellationToken cancellationToken)
    {
        await _context.AuditLogs.AddAsync(log, cancellationToken);
    }
}
