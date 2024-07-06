using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ReceiptCreator.Api.Authentication;
using ReceiptCreator.Api.Entities;
using ReceiptCreator.Api.KavehNegar;
using ReceiptCreator.Api.Repository;

namespace ReceiptCreator.Api.Endpoints;

public static class UserEndPoints
{
    private const string GetUser = "getUser";
    private const string SmsSender = "sendSMS";

    
    public static RouteGroupBuilder MapUsersEndPoints(this IEndpointRouteBuilder routes){
        var group=routes.MapGroup("/users").WithParameterValidation();
        string generatedPassword  = null;
        
        group.MapGet("/currentUser",async (
                IRepository repository,
                ClaimsPrincipal user
                )=> 
            {
                var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    User? currentUser = await repository.GetUserAsync(Int32.Parse(userId));
                    return currentUser is not null ? 
                        Results.Ok(currentUser.AsDto()) : Results.NotFound(new{error="کاربر یافت نشد."});
                }
                return Results.Conflict(new{error="کاربر یافت نشد."});
 
            }
        ).WithName(GetUser).RequireAuthorization();
        

        group.MapGet("/sendSms",async (
                String phoneNumber,
                String password
                )=> 
            {
                String result= await SendSms.SendSMS.SendSMSToUser(password,phoneNumber);

                return Results.Ok(result);
 
            }
        ).WithName(SmsSender).RequireRateLimiting("fixed");




        group.MapPost("/signUp", async (
            IRepository iRepository,
            RegisterUserDto registerUserDto) =>
        {
            generatedPassword = GenerateRandomNo();



            User? existedUser = await iRepository.GetRegesteredPhoneNumberAsync(registerUserDto.PhoneNumber);
            if (existedUser is not null)
            {
                return Results.Conflict(new { error = "با این شماره قبلا ثبت نام صورت گرفته است." });
            }

            else
            {
                UserOtp userOtp = new()
                {
                    UserName = registerUserDto.CompanyName,
                    OtpPassword = generatedPassword,
                    PhoneNumber = registerUserDto.PhoneNumber,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
                UserOtp? existedUserOtp = await iRepository.GetUserOtpAsync(registerUserDto.PhoneNumber);
                if (existedUserOtp is not null)
                {
                    existedUserOtp.OtpPassword = generatedPassword;
                    existedUserOtp.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    await iRepository.UpdateUserOtpAsync(existedUserOtp);
                }
                else if (existedUserOtp is null)
                {
                    await iRepository.AddUserOtp(userOtp);

                }
                
                /*var routeValues = new
                {
                    phoneNumber = registerUserDto.PhoneNumber,
                    password = generatedPassword
                };
                var result=Results.CreatedAtRoute(SmsSender,routeValues);*/
                
                String result= await SendSms.SendSMS.
                    SendSMSToUser(generatedPassword,
                        registerUserDto.PhoneNumber);
                return Results.Ok(result);


            }
        }).RequireRateLimiting("fixed");

        group.MapPost("/signIn", async (
            IRepository iRepository,
            SignInUserDto signInUserDto) =>
              {
                  User? regesterdUser = await iRepository.
                      GetRegesteredPhoneNumberAsync(signInUserDto.PhoneNumber);

                  UserOtp? existedUserOtp = await iRepository.GetUserOtpAsync(signInUserDto.PhoneNumber);

                  if (regesterdUser is not null )
                  {

                      generatedPassword = GenerateRandomNo();
                      
                       if (existedUserOtp is not null)
                       {
                           existedUserOtp.OtpPassword = generatedPassword;
                           existedUserOtp.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                           await iRepository.UpdateUserOtpAsync(existedUserOtp);
                       }else if (existedUserOtp is null)
                       {
                           UserOtp userOtp = new()
                           {
                               UserName = regesterdUser.Name,
                               OtpPassword = generatedPassword,
                               PhoneNumber = regesterdUser.PhoneNumber,
                               Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                           };
                           await iRepository.AddUserOtp(userOtp);

                       }

                       String result= await SendSms.SendSMS.
                          SendSMSToUser(generatedPassword,
                              signInUserDto.PhoneNumber);
                       /*var routeValues = new
                       {
                           phoneNumber = signInUserDto.PhoneNumber,
                           password = generatedPassword
                       };
                       var result=Results.CreatedAtRoute(SmsSender,routeValues);*/
                      return Results.Ok(result);
                      //Console.WriteLine(result);
                     
                  }
                  return Results.NotFound(new { error="شما ثبت نام نکرده اید."});
                  
              }).RequireRateLimiting("fixed");
      
        group.MapPost("/signInPasswordCheck", async (
            IJwtProvider iJwtProvider,
            IRepository iRepository,
            SignInUserDto signInUserDto
            ) =>
        {
           UserOtp? userOtp= await iRepository.GetUserOtpAsync(signInUserDto.PhoneNumber);
           long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

           long timeDistance = (currentTime/1000) - (userOtp!.Time/1000);

           if (timeDistance > 120)
           {
               return Results.Conflict(new { error="پسورد منقضی شده"});

           }

            if (signInUserDto.Password == userOtp!.OtpPassword)
            {
                
                
                User? regesterdeUser = await iRepository.GetRegesteredPhoneNumberAsync(signInUserDto.PhoneNumber);

                if (regesterdeUser is not null)
                {
                    var token=  await iJwtProvider.Generate(regesterdeUser);
                  //   var userData=Results.CreatedAtRoute(GetUser,new {user.UserId},user);
                  return Results.Ok(new{tok=token,userData=regesterdeUser});
                    
                }

                return Results.NotFound(new { error="شما ثبت نام نکرده اید."});

            }
            else
            {
                return Results.Conflict(new { error="رمز اشتباه است"});
            }
        });
        
        
        group.MapPost("/signUpPasswordCheck",async (
            IJwtProvider iJwtProvider,
            IRepository iRepository,
            RegisterUserDto registerUserDto
            )=>{
            UserOtp? userOtp= await iRepository.GetUserOtpAsync(registerUserDto.PhoneNumber);
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (userOtp ==null)
            {
                return Results.NotFound(new { error="یوزر یافت نشد"});

            }
            
            long timeDistance = currentTime/1000 - ((userOtp!.Time)/1000);

            if (timeDistance > 120)
            {
                return Results.Conflict(new { error="پسورد منقضی شده"});

            }

            if (registerUserDto.Password == userOtp!.OtpPassword )
            {

                User user = new()
                {
                    Name = registerUserDto.CompanyName,
                    Address = registerUserDto.Address,
                    PhoneNumber = registerUserDto.PhoneNumber,
                    PageId = registerUserDto.PageId,
                    JobTitle = registerUserDto.JobTitle
                   
                };

                User? regesterdeUser = await iRepository.GetRegesteredPhoneNumberAsync(registerUserDto.PhoneNumber);

                if (regesterdeUser is not null)
                {
                   
                    return Results.Conflict(new{error="با این شماره قبلا ثبت نام صورت گرفته است."});

                }
                else
                {
                    await iRepository.AddUser(user);
                 var token=  await iJwtProvider.Generate(user);
                    // var userData=Results.CreatedAtRoute(GetUser,new {user.UserId},user);
                    return Results.Ok(new{tok=token,userData=user});
                }
            }
            else
            {
                return Results.Conflict(new { error="رمز اشتباه است"});

            }
        });
        
        group.MapDelete("/{id}",async (IRepository repository,int id)=> {
            User? user =await repository.GetUserAsync(id);

            if(user is not null){
                await repository.DeleteUser(id); 
            }
            return Results.NoContent();   
        }).RequireAuthorization();
        group.MapDelete("userOtp/{id}",async (IRepository repository,int id)=> {
                await repository.DeleteUserOtpAsync(id); 
            
            return Results.NoContent();   
        }).RequireAuthorization();

        group.MapPost("/updateProfile", async (
            IRepository iRepository,
            ClaimsPrincipal? user,
            ProfileDataDto profileDataDto
        ) =>
        {
            var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId!= null)
            {
                User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
                if(currentUser==null){
                    return Results.NotFound(new{error="کاربر یافت نشد."}); 
                }
                

                currentUser.Name = profileDataDto.CompanyName;
                currentUser.Address = profileDataDto.CompanyAddress;
                currentUser.PageId =profileDataDto.CompanyLink;
                currentUser.JobTitle = profileDataDto.JobTitle;
                await iRepository.UpdateProfileAsync(currentUser);
                return Results.Ok("به روز رسانی موفق");


            }

            return Results.NotFound(new{error="کاربر یافت نشد."});


        }).RequireAuthorization();
        
        group.MapPost("/requestPanel", async (
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

            PanelRequests? panelRequests = new()
            {
                Name = currentUser.Name,
                Address = currentUser.Address,
                PhoneNumber = currentUser.PhoneNumber,
                PageId = currentUser.PageId,
                JobTitle = currentUser.JobTitle
            };

            await iRepository.AddPanelRequest(panelRequests);
            
            return Results.Ok("درخواست شما ثبت شد");
        }).RequireAuthorization().DisableAntiforgery();

        
        
        
        
        
        return group;
    }
    
   
    
    private static string GenerateRandomNo()
    {
        int _min = 1000;
        int _max = 9999;
        Random _rdm = new Random();
        return _rdm.Next(_min, _max).ToString();
    }
    
    
    
}