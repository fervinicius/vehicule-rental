using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace rental_challenge.Models
{
  // Enum para garantir que apenas os tipos de CNH válidos sejam usados
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum CnhType
  {
    A,
    B,
    AB
  }

  [Table("drivers")]
  public class Driver
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(14)] // Formato: XX.XXX.XXX/XXXX-XX
    public required string Cnpj { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [MaxLength(11)]
    public required string CnhNumber { get; set; }

    [Required]
    public CnhType CnhType { get; set; }

    public string? CnhImageUrl { get; set; }
  }
}