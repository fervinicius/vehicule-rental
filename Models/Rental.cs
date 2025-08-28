using System.ComponentModel.DataAnnotations.Schema;

namespace rental_challenge.Models
{
  [Table("rentals")]
  public class Rental
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DriverId { get; set; }
    public Guid MotorcycleId { get; set; }
    public Guid RentalPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PredictedEndDate { get; set; }
    public DateTime? EndDate { get; set; }
  }
}