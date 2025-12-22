using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using CareFlow.Domain.ValueObjects;
using CareFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareFlow.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Therapist)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Therapist)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetOverlappingAppointmentsAsync(Guid therapistId, DateRange range, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Where(a => a.TherapistId == therapistId &&
                        a.TimeRange.Start < range.End &&
                        a.TimeRange.End > range.Start &&
                        a.Status != Domain.Enums.AppointmentStatus.Canceled)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        await _context.Appointments.AddAsync(appointment, cancellationToken);
    }

    public Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        _context.Appointments.Update(appointment);
        return Task.CompletedTask;
    }
}
