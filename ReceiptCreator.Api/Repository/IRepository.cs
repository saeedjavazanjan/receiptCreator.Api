using ReceiptCreator.Api.Entities;

namespace ReceiptCreator.Api.Repository;

public interface IRepository
{
  
    
    //users
    Task<User?> GetUserAsync(int id);
    Task<User?> GetRegesteredPhoneNumberAsync(string phoneNumber);
    Task AddUser (User user);
    Task DeleteUser(int id);
    Task UpdateProfileAsync(User user);

    
    Task AddUserOtp ( UserOtp userOtp);
    Task<UserOtp?> GetUserOtpAsync(string userPhoneNumber);
    Task UpdateUserOtpAsync(UserOtp userOtp);
    Task DeleteUserOtpAsync(int id);


   

}