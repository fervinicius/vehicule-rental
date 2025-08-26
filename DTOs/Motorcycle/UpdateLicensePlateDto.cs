using System.ComponentModel.DataAnnotations;

namespace rental_challenge.DTOs.Motorcycle
{
  public class UpdateLicensePlateDto
  {
    [Required]
    public required string NewLicensePlate { get; set; }
  }
}