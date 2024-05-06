using System.ComponentModel.DataAnnotations;

namespace ReceiptCreator.Api.Entities;

public class UserOtp
{
   

    public int Id { get; set; }
    [MaxLength(20)]
    [Required]
    public required string UserName { get; set; }
    
    public required string PhoneNumber{ get; set; }
    
    public required string OtpPassword { get; set; }
    
    public long Time { get; set; }
}