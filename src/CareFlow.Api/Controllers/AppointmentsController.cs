using CareFlow.Application.DTOs.Appointments;
using CareFlow.Application.Features.Appointments.Commands.CreateAppointment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareFlow.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateAppointmentRequest request)
    {
        var command = new CreateAppointmentCommand(request);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id }, id);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CareFlow.Domain.Entities.Appointment>>> GetAll()
    {
        var result = await _mediator.Send(new CareFlow.Application.Features.Appointments.Queries.GetAppointments.GetAppointmentsQuery());
        return Ok(result);
    }
}
