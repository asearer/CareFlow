using CareFlow.Domain.Entities;

namespace CareFlow.Domain.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
}
