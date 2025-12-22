using CareFlow.Domain.Entities;
using CareFlow.Domain.Interfaces;
using CareFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareFlow.Infrastructure.Repositories;

public class ClinicalNoteRepository : IClinicalNoteRepository
{
    private readonly ApplicationDbContext _context;

    public ClinicalNoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClinicalNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ClinicalNotes
            .Include(n => n.Patient)
            .Include(n => n.Therapist)
            .Include(n => n.Appointment)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClinicalNote>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken)
    {
        return await _context.ClinicalNotes
            .Where(n => n.PatientId == patientId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ClinicalNote note, CancellationToken cancellationToken)
    {
        await _context.ClinicalNotes.AddAsync(note, cancellationToken);
    }

    public Task UpdateAsync(ClinicalNote note, CancellationToken cancellationToken)
    {
        _context.ClinicalNotes.Update(note);
        return Task.CompletedTask;
    }
}
