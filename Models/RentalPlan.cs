using System.ComponentModel.DataAnnotations.Schema;

namespace rental_challenge.Models
{
  [Table("rentalPlans")]
  public class RentalPlan
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public int DurationInDays { get; set; }
    public decimal DailyCost { get; set; }
  }
}