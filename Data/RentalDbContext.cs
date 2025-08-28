using Microsoft.EntityFrameworkCore;
using rental_challenge.Models;

namespace rental_challenge.Data
{
  public class RentalDbContext : DbContext
  {
    public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
    {
    }

    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<RentalPlan> RentalPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Motorcycle>().HasIndex(m => m.LicensePlate).IsUnique();

      modelBuilder.Entity<Driver>().HasIndex(d => d.Cnpj).IsUnique();

      modelBuilder.Entity<Driver>().HasIndex(d => d.CnhNumber).IsUnique();

      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<RentalPlan>().HasData(
        new RentalPlan { Id = new Guid("f81b1a72-91f3-4e67-a72a-c2a4f0b2f3a6"), DurationInDays = 7, DailyCost = 30.00m },
        new RentalPlan { Id = new Guid("b1a3c7c2-8e4d-4e9f-8d9e-3e2b1a0f8c7d"), DurationInDays = 15, DailyCost = 28.00m },
        new RentalPlan { Id = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), DurationInDays = 30, DailyCost = 22.00m },
        new RentalPlan { Id = new Guid("1c3e4567-e89b-12d3-a456-426614174000"), DurationInDays = 45, DailyCost = 20.00m },
        new RentalPlan { Id = new Guid("2d4f5678-e89b-12d3-a456-426614174001"), DurationInDays = 50, DailyCost = 18.00m }
      );
    }
  }
}