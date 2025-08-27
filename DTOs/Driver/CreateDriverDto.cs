using rental_challenge.Models;
using System.ComponentModel.DataAnnotations;

public class CreateDriverDto
{
  [Required]
  public required string Name { get; set; }

  [Required]
  public required string Cnpj { get; set; }

  [Required]
  public DateTime DateOfBirth { get; set; }

  [Required]
  public required string CnhNumber { get; set; }

  [Required]
  public CnhType CnhType { get; set; }
}