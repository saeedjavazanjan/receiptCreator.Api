using System.ComponentModel.DataAnnotations;

namespace ReceiptCreator.Api;





public record UserDto(
    int Id,
    string Name,
    string Address,
    string PhoneNumber,
    string TypeOfPage,
    string JobTitle
);
public record RegisterUserDto(
   [StringLength(20)] string CompanyName,
   [StringLength(4)] string Password,
   [StringLength(500)] string Address,
    [Required][StringLength(12)] string PhoneNumber,
    [StringLength(20)] string PageId,
   [Required][StringLength(30)] string JobTitle
);
public record ProfileDataDto(
   [StringLength(20)] string CompanyName,
   [StringLength(12)] string CompanyPhone ,
   [StringLength(500)] string CompanyAddress,
    [StringLength(500)] string CompanyLink,
    [StringLength(5000)] string CompanyRules,
   [Required][StringLength(30)] string JobTitle
);

public record SignInUserDto(
    [Required] [StringLength(12)] string PhoneNumber,
     [StringLength(4)] string Password
);

public record UserOtpDto(
    int Id,
    string UserName,
    string OtpPassword,
    string UserPhoneNumber,
    long Time
);

public record GetDatabaseDto(
    IFormFile? DatabaseFile
);

