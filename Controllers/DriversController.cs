using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rental_challenge.Data;
using rental_challenge.Models;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
  private readonly RentalDbContext _context;

  public DriversController(RentalDbContext context)
  {
    _context = context;
  }

  [HttpPost]
  public async Task<IActionResult> CreateDriver([FromBody] CreateDriverDto driverDto)
  {
    if (await _context.Drivers.AnyAsync(d => d.Cnpj == driverDto.Cnpj))
    {
      return Conflict("A driver with this CNPJ already exists.");
    }

    if (await _context.Drivers.AnyAsync(d => d.CnhNumber == driverDto.CnhNumber))
    {
      return Conflict("A driver with this CNH number already exists.");
    }

    var driver = new Driver
    {
      Name = driverDto.Name,
      Cnpj = driverDto.Cnpj,
      DateOfBirth = driverDto.DateOfBirth,
      CnhNumber = driverDto.CnhNumber,
      CnhType = driverDto.CnhType
    };

    _context.Drivers.Add(driver);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(CreateDriver), new { id = driver.Id }, driver);
  }

  [HttpGet]
  public async Task<IActionResult> GetDrivers()
  {
    var drivers = await _context.Drivers.ToListAsync();
    return Ok(drivers);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetDriverById(Guid id)
  {
    var driver = await _context.Drivers.FindAsync(id);

    if (driver is null)
    {
      return NotFound("Driver not found");
    }

    return Ok(driver);
  }

  [HttpPost("{id}/cnh-image")]
  public async Task<IActionResult> UploadCnhImage(Guid id, IFormFile cnhImage)
  {
    var driver = await _context.Drivers.FindAsync(id);
    if (driver is null)
    {
      return NotFound("Driver not found.");
    }

    if (cnhImage == null || cnhImage.Length == 0)
    {
      return BadRequest("Image file is required.");
    }

    var allowedExtensions = new[] { ".png", ".bmp" };
    var extension = Path.GetExtension(cnhImage.FileName).ToLowerInvariant();
    if (!allowedExtensions.Contains(extension))
    {
      return BadRequest("Invalid image format. Only PNG and BMP are allowed.");
    }

    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    if (!Directory.Exists(uploadsPath))
    {
      Directory.CreateDirectory(uploadsPath);
    }

    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(uploadsPath, uniqueFileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await cnhImage.CopyToAsync(stream);
    }

    driver.CnhImageUrl = filePath;
    await _context.SaveChangesAsync();

    return Ok(new { message = "CNH image uploaded successfully.", filePath = driver.CnhImageUrl });
  }
}