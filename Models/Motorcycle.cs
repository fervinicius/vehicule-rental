using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rental_challenge.Models
{
  [Table("Motorcycles")]
  public class Motorcycle
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public int Year { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Model { get; set; }

    [Required]
    [MaxLength(20)]
    public required string LicensePlate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}