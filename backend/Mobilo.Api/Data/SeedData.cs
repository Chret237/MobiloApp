using Microsoft.EntityFrameworkCore;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Data;

public static class SeedData
{
    private const string Query = "?crop=entropy&cs=tinysrgb&fit=crop&fm=jpg&q=85&w=1200";

    public static async Task InitializeAsync(MobiloDbContext db, CancellationToken cancellationToken)
    {
        await db.Database.EnsureCreatedAsync(cancellationToken);
        await db.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "Users" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
                "FullName" TEXT NOT NULL,
                "Email" TEXT NOT NULL,
                "Phone" TEXT NULL,
                "PasswordHash" TEXT NOT NULL,
                "PasswordSalt" TEXT NOT NULL,
                "CreatedAt" TEXT NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users" ("Email");
            """, cancellationToken);

        if (await db.Vehicles.AnyAsync(cancellationToken))
        {
            return;
        }

        db.Vehicles.AddRange(
            new Vehicle
            {
                Name = "Range Rover Velar",
                Type = "Luxury SUV",
                Description = "Verified luxury SUV with a comfortable interior and smooth automatic driving.",
                ImageUrl = "https://images.unsplash.com/photo-1622268697621-54243bd1e9f4" + Query,
                PricePerDay = 128,
                Rating = 4.9m,
                ReviewCount = 128,
                Seats = 5,
                Bags = 2,
                Gearbox = "Automatic",
                Location = "Downtown Austin",
                IsActive = true,
            },
            new Vehicle
            {
                Name = "Volvo XC60",
                Type = "Premium SUV",
                Description = "Quiet, practical and refined SUV for longer trips and everyday comfort.",
                ImageUrl = "https://images.unsplash.com/photo-1519641471654-76ce0107ad1b" + Query,
                PricePerDay = 98,
                Rating = 4.8m,
                ReviewCount = 96,
                Seats = 5,
                Bags = 2,
                Gearbox = "Automatic",
                Location = "Downtown Austin",
                IsActive = true,
            },
            new Vehicle
            {
                Name = "Ford Explorer",
                Type = "Family SUV",
                Description = "Spacious family SUV with flexible storage and dependable road handling.",
                ImageUrl = "https://images.unsplash.com/photo-1606103749141-1efed6903912" + Query,
                PricePerDay = 82,
                Rating = 4.7m,
                ReviewCount = 74,
                Seats = 7,
                Bags = 3,
                Gearbox = "Automatic",
                Location = "Downtown Austin",
                IsActive = true,
            });

        await db.SaveChangesAsync(cancellationToken);
    }
}
