using CareFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareFlow.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Patient> Patients { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<ClinicalNote> ClinicalNotes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
