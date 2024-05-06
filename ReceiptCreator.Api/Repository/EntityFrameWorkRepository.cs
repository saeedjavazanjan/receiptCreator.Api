using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ReceiptCreator.Api.Data;
using ReceiptCreator.Api.Entities;
namespace ReceiptCreator.Api.Repository;

public class EntityFrameWorkRepository(ReceiptCreatorContext dbContext) : IRepository
{
    
    //Users
    public async Task<User?> GetUserAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);

    }
    public async Task<User?> GetRegesteredPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task AddUser(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddUserOtp(UserOtp userOtp)
    {
        dbContext.UsersOtp.Add(userOtp);
        await dbContext.SaveChangesAsync();
        
        
    }

    public async Task<UserOtp?> GetUserOtpAsync(string userPhoneNumber)
    {
        try
        {
            return await dbContext.UsersOtp.FirstOrDefaultAsync(userOtp => userOtp.PhoneNumber == userPhoneNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteUser(int id)
    {
        await dbContext.Users.Where(user => user.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateUserOtpAsync(UserOtp userOtp)
    {
        dbContext.Update(userOtp);
        await dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteUserOtpAsync(int id)
    {
        await dbContext.UsersOtp.Where(user => user.Id == id)
            .ExecuteDeleteAsync();
        
    }


}
