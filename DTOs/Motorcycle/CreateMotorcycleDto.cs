using System.ComponentModel.DataAnnotations;

namespace rental_challenge.DTOs.Motorcycle
{
  public class CreateMotorcycleDto
  {
    [Required]
    public int Year { get; set; }

    [Required]
    public required string Model { get; set; }

    [Required]
    public required string LicensePlate { get; set; }
  }
}