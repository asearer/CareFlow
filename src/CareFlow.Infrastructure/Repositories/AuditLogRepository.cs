using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using CareFlow.Domain.Interfaces;
using CareFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<AuditLog>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }
}
