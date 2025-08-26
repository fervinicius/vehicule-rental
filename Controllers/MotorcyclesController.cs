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
      // Verifica se já existe uma moto com a mesma placa
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

      // Retorna o objeto criado com o status 201 Created
      return CreatedAtAction(nameof(CreateMotorcycle), new { id = motorcycle.Id }, motorcycle);
    }
  }
}