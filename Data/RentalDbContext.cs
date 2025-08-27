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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Motorcycle>().HasIndex(m => m.LicensePlate).IsUnique();

      modelBuilder.Entity<Driver>().HasIndex(d => d.Cnpj).IsUnique();

      modelBuilder.Entity<Driver>().HasIndex(d => d.CnhNumber).IsUnique();

      base.OnModelCreating(modelBuilder);
    }
  }
}