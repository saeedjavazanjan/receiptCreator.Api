using System.ComponentModel.DataAnnotations;

namespace ReceiptCreator.Api.Entities;
public class User
{
    public int Id { get; set; }
  
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
    
    [MaxLength(500)]
    [Required]
    public required string Address { get; set; }
    
    [MaxLength(15)]
    [Required]
    public required string PhoneNumber { get; set; }
   
    [MaxLength(20)]
    [Required]
    public required string PageId { get; set; }
  
    [MaxLength(20)]
    public required string JobTitle { get; set; }
    
   
}