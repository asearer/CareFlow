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

        // Look for any users.
        if (context.Users.Any())
        {
            return;   // DB has been seeded
        }

        var adminUser = new User(
            "admin@demo.com",
            BCrypt.Net.BCrypt.HashPassword("DemoPassword123!"),
            "Admin",
            "User",
            UserRole.Admin
        );

        context.Users.Add(adminUser);
        context.SaveChanges();
    }
}
