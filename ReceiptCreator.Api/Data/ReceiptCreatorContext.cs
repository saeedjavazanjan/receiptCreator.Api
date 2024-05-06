using Microsoft.EntityFrameworkCore;
using ReceiptCreator.Api.Entities;

namespace ReceiptCreator.Api.Data;

public class ReceiptCreatorContext:DbContext
{
    public ReceiptCreatorContext(DbContextOptions<ReceiptCreatorContext> options):base(options){

    }
    
    
    public DbSet<User> Users  => Set<User>();
    public DbSet<UserOtp> UsersOtp => Set<UserOtp>();


}