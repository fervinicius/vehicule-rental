using System.ComponentModel.DataAnnotations.Schema;

namespace rental_challenge.Models
{
  [Table("rentals")]
  public class Rental
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MotorcycleId { get; set; }
  }
}