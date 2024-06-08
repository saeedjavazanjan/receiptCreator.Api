using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ReceiptCreator.Api.Entities;
using ReceiptCreator.Api.Repository;

namespace ReceiptCreator.Api.Endpoints;

public static class BackUpEndPoints
{


    public static RouteGroupBuilder MapBackupEndPoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/backup").WithParameterValidation();
  group.MapPost("/uploadDb",async (
            IFileService iFileService,
            IRepository iRepository,
            [FromForm] GetDatabaseDto dbDto,
            ClaimsPrincipal? user
            )=>{
          var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
          if(currentUser==null){
              return Results.NotFound(new{error="کاربر یافت نشد."}); 
          }
          var userName = currentUser.PhoneNumber;
          
              if (dbDto.DatabaseFile != null)
              {
                  var fileResult =
                      iFileService.SaveDatabase(dbDto.DatabaseFile,currentUser.PhoneNumber);
                  if (fileResult.Item1 == 1)
                  {
                      return Results.Ok("با موفقیت آپلود شد");
                  }
                  else
                  {
                      return Results.Conflict(new { error = fileResult.Item2});
                  }

              }
              else
              {
                  return Results.Conflict(new { error = "فایل خالی است"});

              }


        }).RequireAuthorization().DisableAntiforgery();
  
        group.MapGet("/downloadDb", async (
            IFileService iFileService,
            IRepository iRepository,
            ClaimsPrincipal? user
        ) => {
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Results.Unauthorized();
            }
    
            User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
            if (currentUser == null)
            {
                return Results.NotFound(new { error = "کاربر یافت نشد." });
            }
    
            var userName = currentUser.PhoneNumber;
            var dbFilePath = iFileService.GetDatabasePath(userName);
            if (string.IsNullOrEmpty(dbFilePath.Item2) || !System.IO.File.Exists(dbFilePath.Item2))
            {
                return Results.NotFound(new { error = "فایل دیتابیس یافت نشد." });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(dbFilePath.Item2);
            var fileContent = new MemoryStream(fileBytes);
            return Results.File(fileContent, "application/octet-stream", "receipt.sqlite");
        }).RequireAuthorization().DisableAntiforgery();
  
  
        return group;

    }
    
}