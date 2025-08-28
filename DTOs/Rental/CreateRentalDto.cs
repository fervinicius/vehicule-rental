using System.ComponentModel.DataAnnotations;

public class CreateRentalDto
{
  [Required]
  public Guid DriverId { get; set; }
  [Required]
  public Guid MotorcycleId { get; set; }
  [Required]
  public Guid RentalPlanId { get; set; }
}