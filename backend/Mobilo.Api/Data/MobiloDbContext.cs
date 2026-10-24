using Microsoft.EntityFrameworkCore;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Data;

public sealed class MobiloDbContext(DbContextOptions<MobiloDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(vehicle => vehicle.Id);
            entity.Property(vehicle => vehicle.Name).HasMaxLength(120).IsRequired();
            entity.Property(vehicle => vehicle.Type).HasMaxLength(80).IsRequired();
            entity.Property(vehicle => vehicle.Description).HasMaxLength(1000).IsRequired();
            entity.Property(vehicle => vehicle.ImageUrl).HasMaxLength(500).IsRequired();
            entity.Property(vehicle => vehicle.PricePerDay).HasPrecision(10, 2);
            entity.Property(vehicle => vehicle.Rating).HasPrecision(3, 2);
            entity.Property(vehicle => vehicle.Gearbox).HasMaxLength(40).IsRequired();
            entity.Property(vehicle => vehicle.Location).HasMaxLength(120).IsRequired();
            entity.HasIndex(vehicle => new { vehicle.IsActive, vehicle.Type, vehicle.Location });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.FullName).HasMaxLength(120).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(200).IsRequired();
            entity.Property(user => user.Phone).HasMaxLength(30);
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.PasswordSalt).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
        });
    }
}
