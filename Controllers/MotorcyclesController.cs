using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rental_challenge.Data;
using rental_challenge.DTOs.Motorcycle;
using rental_challenge.Models;

namespace rental_challenge.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MotorcyclesController : ControllerBase
  {
    private readonly RentalDbContext _context;

    public MotorcyclesController(RentalDbContext context)
    {
      _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleDto motorcycleDto)
    {
      var licensePlateExists = await _context.Motorcycles.AnyAsync(m => m.LicensePlate == motorcycleDto.LicensePlate);
      if (licensePlateExists)
      {
        return Conflict("A motorcycle with this license plate already exists.");
      }

      var motorcycle = new Motorcycle
      {
        Year = motorcycleDto.Year,
        Model = motorcycleDto.Model,
        LicensePlate = motorcycleDto.LicensePlate
      };

      _context.Motorcycles.Add(motorcycle);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(CreateMotorcycle), new { id = motorcycle.Id }, motorcycle);
    }

    [HttpGet]
    public async Task<IActionResult> GetMotorcycles([FromQuery] string? LicensePlate)
    {
      var query = _context.Motorcycles.AsQueryable();

      if (!string.IsNullOrEmpty(LicensePlate))
      {
        query = query.Where(m => m.LicensePlate == LicensePlate);
      }

      var motorcycles = await query.ToListAsync();

      return Ok(motorcycles);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLicensePlate(Guid id, [FromBody] UpdateLicensePlateDto dto)
    {
      var motorcycle = await _context.Motorcycles.FindAsync(id);

      if (motorcycle is null)
      {
        return NotFound("Motorcycle not found.");
      }

      var licensePlateExists = await _context.Motorcycles
          .AnyAsync(m => m.LicensePlate == dto.NewLicensePlate && m.Id != id);

      if (licensePlateExists)
      {
        return Conflict("License plate already in use by another motorcycle.");
      }

      motorcycle.LicensePlate = dto.NewLicensePlate;

      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle(Guid id)
    {
      var motorcycle = await _context.Motorcycles.FindAsync(id);

      if (motorcycle is null)
      {
        return NotFound("Motorcycle not found.");
      }

      var hasRentals = await _context.Rentals.AnyAsync(r => r.MotorcycleId == id);

      if (hasRentals)
      {
        return Conflict("Cannot delete a motorcycle that has associated rentals.");
      }

      _context.Motorcycles.Remove(motorcycle);

      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}