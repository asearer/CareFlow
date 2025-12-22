using CareFlow.Application.Features.Patients.Commands.CreatePatient;
using CareFlow.Application.Features.Patients.Queries.GetPatients;
using CareFlow.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareFlow.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
    {
        var result = await _mediator.Send(new GetPatientsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreatePatientCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id }, id);
    }
}
