using CareFlow.Domain.Entities;
using CareFlow.Domain.Enums;
using CareFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareFlow.Api.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Create database and schema if not exists (Demo mode)
        context.Database.EnsureCreated();

        // Seed Admin User
        var adminUser = context.Users.FirstOrDefault(u => u.Email == "admin@demo.com");
        if (adminUser == null)
        {
            adminUser = new User(
                "admin@demo.com",
                BCrypt.Net.BCrypt.HashPassword("DemoPassword123!"),
                "Admin",
                "User",
                UserRole.Admin
            );
            context.Users.Add(adminUser);
            context.SaveChanges();
        }

        // Seed Patients
        if (!context.Patients.Any())
        {
            var patients = new List<Patient>
            {
                new Patient("John", "Doe", new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc), "john.doe@example.com", "555-0101", "123 Main St"),
                new Patient("Jane", "Smith", new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc), "jane.smith@example.com", "555-0102", "456 Oak Ave"),
                new Patient("Robert", "Johnson", new DateTime(1975, 10, 20, 0, 0, 0, DateTimeKind.Utc), "robert.j@example.com", "555-0103", "789 Pine Rd"),
                new Patient("Emily", "Davis", new DateTime(1985, 3, 30, 0, 0, 0, DateTimeKind.Utc), "emily.d@example.com", "555-0104", "321 Elm St"),
                new Patient("Michael", "Wilson", new DateTime(1995, 7, 7, 0, 0, 0, DateTimeKind.Utc), "michael.w@example.com", "555-0105", "654 Birch Ln")
            };
            context.Patients.AddRange(patients);
            context.SaveChanges();
        }

        // Seed Appointments
        if (!context.Appointments.Any())
        {
            var patients = context.Patients.ToList();
            if (patients.Any() && adminUser != null)
            {
                var today = DateTime.UtcNow.Date;
                var appointments = new List<Appointment>
                {
                    new Appointment(patients[0].Id, adminUser.Id, new CareFlow.Domain.ValueObjects.DateRange(today.AddHours(9), today.AddHours(10))),
                    new Appointment(patients[1].Id, adminUser.Id, new CareFlow.Domain.ValueObjects.DateRange(today.AddHours(11), today.AddHours(12))),
                    new Appointment(patients[2].Id, adminUser.Id, new CareFlow.Domain.ValueObjects.DateRange(today.AddDays(1).AddHours(14), today.AddDays(1).AddHours(15))),
                    new Appointment(patients[3].Id, adminUser.Id, new CareFlow.Domain.ValueObjects.DateRange(today.AddDays(2).AddHours(10), today.AddDays(2).AddHours(11))),
                    new Appointment(patients[0].Id, adminUser.Id, new CareFlow.Domain.ValueObjects.DateRange(today.AddDays(3).AddHours(16), today.AddDays(3).AddHours(17)))
                };
                context.Appointments.AddRange(appointments);
                context.SaveChanges();
            }
        }
    }
}
