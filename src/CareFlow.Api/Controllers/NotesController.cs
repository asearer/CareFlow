using CareFlow.Application.DTOs.Notes;
using CareFlow.Application.Features.Notes.Commands.CreateNote;
using CareFlow.Application.Features.Notes.Commands.SignNote;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareFlow.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateNoteRequest request)
    {
        var therapistId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new CreateNoteCommand(request, therapistId);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id }, id);
    }

    [HttpPost("{id}/sign")]
    public async Task<ActionResult> Sign(Guid id)
    {
        var therapistId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new SignNoteCommand(id, therapistId);
        await _mediator.Send(command);
        return NoContent();
    }
}
