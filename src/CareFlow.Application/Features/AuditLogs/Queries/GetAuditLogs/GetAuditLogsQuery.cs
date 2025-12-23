using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using MediatR;

namespace CareFlow.Application.Features.AuditLogs.Queries.GetAuditLogs;

public record GetAuditLogsQuery : IRequest<List<AuditLog>>;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, List<AuditLog>>
{
    private readonly IAuditLogRepository _repository;

    public GetAuditLogsQueryHandler(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AuditLog>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
