using CareFlow.Domain.Entities;
using CareFlow.Domain.ValueObjects;

namespace CareFlow.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Appointment>> GetOverlappingAppointmentsAsync(Guid therapistId, DateRange range, CancellationToken cancellationToken);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken);
}
