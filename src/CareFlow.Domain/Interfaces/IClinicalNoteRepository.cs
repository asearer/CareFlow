using CareFlow.Domain.Entities;

namespace CareFlow.Domain.Interfaces;

public interface IClinicalNoteRepository
{
    Task<ClinicalNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ClinicalNote>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken);
    Task AddAsync(ClinicalNote note, CancellationToken cancellationToken);
    Task UpdateAsync(ClinicalNote note, CancellationToken cancellationToken);
}
