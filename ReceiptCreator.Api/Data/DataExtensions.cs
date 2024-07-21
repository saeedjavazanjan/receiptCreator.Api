using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.EntityFrameworkCore;
using ReceiptCreator.Api.Authentication;
using ReceiptCreator.Api.Endpoints;
using ReceiptCreator.Api.Repository;

namespace ReceiptCreator.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ReceiptCreatorContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connString=configuration.GetConnectionString("Production");
      //  var connString=configuration.GetConnectionString("ReceiptCreatorContext");
        services.AddSqlServer<ReceiptCreatorContext>(connString,builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                builder.UseRowNumberForPaging();
            })
            .AddScoped<IRepository,EntityFrameWorkRepository>();
        return services;

    }


    
    public static IServiceCollection AddJwtProvider(
        this IServiceCollection services
    )
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
    
    public static IServiceCollection AddFileService(
        this IServiceCollection services
    )
    {
        services.AddScoped<IFileService, FileService>();
        return services;
    }

    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        /*services.AddHttpClient("MyClient", client =>
        {
            client.BaseAddress = new Uri("http://192.168.1.167:5047/users");
        });*/

        return services;
    }
}