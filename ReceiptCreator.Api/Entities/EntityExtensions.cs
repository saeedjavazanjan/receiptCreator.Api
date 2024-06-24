namespace ReceiptCreator.Api.Entities;

public static class EntityExtensions
{
    
    public static UserDto AsDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.Address,
            user.PhoneNumber,
            user.PageId,
            user.JobTitle
        );


    }
    public static PanelRequestDto AsDto(this PanelRequests panelRequests)
    {
        return new PanelRequestDto(
            panelRequests.Id,
            panelRequests.Name,
            panelRequests.Address,
            panelRequests.PhoneNumber,
            panelRequests.PageId,
            panelRequests.JobTitle
        );


    }
  
    public static UserOtpDto AsDto(this UserOtp userOtp)
    {
        return new UserOtpDto(
            userOtp.Id,
            userOtp.UserName,
            userOtp.OtpPassword,
            userOtp.PhoneNumber,
            userOtp.Time
          
        );


    }

  
}