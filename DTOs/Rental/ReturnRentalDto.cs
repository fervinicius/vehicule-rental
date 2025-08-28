using System.ComponentModel.DataAnnotations;

public class ReturnRentalDto
{
  [Required]
  public DateTime ReturnDate { get; set; }
}