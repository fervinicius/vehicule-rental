using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rental_challenge.Data;
using rental_challenge.Models;

[ApiController]
[Route("[controller]")]
public class RentalsController : ControllerBase
{
  private readonly RentalDbContext _context;

  public RentalsController(RentalDbContext context)
  {
    _context = context;
  }

  [HttpPost]
  public async Task<IActionResult> CreateRental([FromBody] CreateRentalDto rentalDto)
  {
    var driver = await _context.Drivers.FindAsync(rentalDto.DriverId);

    if (driver is null) return NotFound("Driver not found.");

    if (driver.CnhType != CnhType.A && driver.CnhType != CnhType.AB)
    {
      return BadRequest("Driver must have CNH type 'A' or 'A+B' to rent a motorcycle.");
    }

    var isMotorcycleRented = await _context.Rentals
        .AnyAsync(r => r.MotorcycleId == rentalDto.MotorcycleId && r.EndDate == null);

    if (isMotorcycleRented)
    {
      return Conflict("This motorcycle is already rented.");
    }

    var plan = await _context.RentalPlans.FindAsync(rentalDto.RentalPlanId);

    if (plan is null) return NotFound("Rental plan not found.");

    var startDate = DateTime.UtcNow.AddDays(1);

    var predictedEndDate = startDate.AddDays(plan.DurationInDays);

    var rental = new Rental
    {
      DriverId = rentalDto.DriverId,
      MotorcycleId = rentalDto.MotorcycleId,
      RentalPlanId = rentalDto.RentalPlanId,
      StartDate = startDate,
      PredictedEndDate = predictedEndDate,
      EndDate = null // Fica nulo até a devolução
    };

    _context.Rentals.Add(rental);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(CreateRental), new { id = rental.Id }, rental);
  }

  // Endpoint para listar os planos e facilitar os testes
  [HttpGet("plans")]
  public async Task<IActionResult> GetPlans()
  {
    return Ok(await _context.RentalPlans.ToListAsync());
  }

  [HttpPost("{id}/return")]
  public async Task<IActionResult> ReturnMotorcycle(Guid id, [FromBody] ReturnRentalDto returnDto)
  {
    var rental = await _context.Rentals
        .Include(r => r.RentalPlanId)
        .FirstOrDefaultAsync(r => r.Id == id && r.EndDate == null);

    if (rental is null)
    {
      return NotFound("Active rental not found.");
    }

    rental.EndDate = returnDto.ReturnDate;

    var plan = await _context.RentalPlans.FindAsync(rental.RentalPlanId);

    if (plan is null)
    {
      return StatusCode(500, "Rental plan associated with this rental not found.");
    }

    int actualDurationInDays = (returnDto.ReturnDate.Date - rental.StartDate.Date).Days;

    decimal totalCost = 0;

    // Caso 1: Devolução tardia
    if (actualDurationInDays > plan.DurationInDays)
    {
      int extraDays = actualDurationInDays - plan.DurationInDays;
      decimal originalPlanCost = plan.DurationInDays * plan.DailyCost;
      decimal lateFee = extraDays * 50.00m; // R$ 50 por dia adicional
      totalCost = originalPlanCost + lateFee;
    }
    // Caso 2: Devolução antecipada
    else if (actualDurationInDays < plan.DurationInDays)
    {
      int remainingDays = plan.DurationInDays - actualDurationInDays;
      decimal costOfUsedDays = actualDurationInDays * plan.DailyCost;
      decimal costOfUnusedDays = remainingDays * plan.DailyCost;
      decimal penalty = 0;

      switch (plan.DurationInDays)
      {
        case 7:
          penalty = costOfUnusedDays * 0.20m; // 20% de multa
          break;
        case 15:
          penalty = costOfUnusedDays * 0.40m; // 40% de multa
          break;
          // Outros planos não têm multa por devolução antecipada
      }
      totalCost = costOfUsedDays + penalty;
    }
    // Caso 3: Devolução no prazo
    else
    {
      totalCost = plan.DurationInDays * plan.DailyCost;
    }

    await _context.SaveChangesAsync();

    return Ok(new { message = "Motorcycle returned successfully.", totalCost = totalCost });
  }
}