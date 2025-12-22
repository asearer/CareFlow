using CareFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareFlow.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.TimeRange, tr =>
        {
            tr.Property(p => p.Start).HasColumnName("StartTime");
            tr.Property(p => p.End).HasColumnName("EndTime");
        });

        builder.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Therapist)
            .WithMany()
            .HasForeignKey(a => a.TherapistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
