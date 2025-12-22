using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using MediatR;

namespace CareFlow.Application.Features.Appointments.Queries.GetAppointments;

public record GetAppointmentsQuery : IRequest<IEnumerable<Appointment>>;

public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, IEnumerable<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Appointment>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _appointmentRepository.GetAllAsync(cancellationToken);
    }
}
