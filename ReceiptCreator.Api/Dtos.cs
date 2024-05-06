﻿using System.ComponentModel.DataAnnotations;

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
   [StringLength(500)] string Address,
    [Required][StringLength(12)] string PhoneNumber,
    [StringLength(20)] string PageId,
   [Required][StringLength(20)] string JobTitle
);

public record AddUserDto(
    [StringLength(20)] string UserName,
    [Required][StringLength(4)] string Password,
    [Required] [StringLength(500)] string Address,
    [Required] [StringLength(12)] string PhoneNumber,
    [Required][StringLength(20)] string PageId,
    [StringLength(20)] string JobTitle
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