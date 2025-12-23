using CareFlow.Application.Features.AuditLogs.Queries.GetAuditLogs;
using CareFlow.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareFlow.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuditLog>>> GetAll()
    {
        var logs = await _mediator.Send(new GetAuditLogsQuery());
        return Ok(logs);
    }
}
