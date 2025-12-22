using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using MediatR;

namespace CareFlow.Application.Features.Patients.Queries.GetPatients;

public record GetPatientsQuery : IRequest<IEnumerable<Patient>>;

public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, IEnumerable<Patient>>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<Patient>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetAllAsync(cancellationToken);
    }
}
