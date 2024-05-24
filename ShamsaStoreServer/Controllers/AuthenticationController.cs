using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShamsaStoreServer.Entities;
using Shared.Dtos.Authentication;
using System;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    // سرویس های پیش فرض ایدنتیتی
    private readonly UserManager<AuthenctiationUser> _userManager;

    private readonly SignInManager<AuthenctiationUser> _signInManager;

    // lazy loading 
    public AuthenticationController(UserManager<AuthenctiationUser> userManager,SignInManager<AuthenctiationUser> signInManager)
    {
        _userManager = userManager;

        _signInManager = signInManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user =
           await _userManager.FindByEmailAsync(model.Email);

        if (user is not null)
        {
            return BadRequest("با این ایمیل قبلا ثبت نام شده است");
        }

        // هش کردن پسورد به دلیل امنیت بیشتر اطلاعات کاربران

        var hasher =
            new PasswordHasher<IdentityUser>();

        user = new AuthenctiationUser
        {
            Email = model.Email,
            PhoneNumber = model.Phone,
            FullName = model.FullName,
            UserName = model.Email,
            PasswordHash = hasher.HashPassword(null, model.Password),
        };

        // برای ما یک نتیجه بر میگرداند
        var result =
            await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            var roleResult =
               await _userManager.AddToRoleAsync(user, "User");

            if (roleResult.Succeeded)
                return Ok();
        }

        return BadRequest("سرور به مشکل خورده است بعدا امتحان کنید");
    }

    [HttpPost("Login")]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginDto model,
        [FromQuery] bool? useCookies,
        [FromQuery] bool? useSessionCookies)
    {
        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);

        var isPersistent = (useCookies == true) && (useSessionCookies != true);

        _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
        
        var user = 
            await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return TypedResults.Problem("ایمیل یا رمز عبور اشتباه است", statusCode: StatusCodes.Status401Unauthorized);
        }

        var result = 
            await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent, lockoutOnFailure: true);
        
        if (!result.Succeeded)
        {
            return TypedResults.Problem("ایمیل یا رمز عبور اشتباه است", statusCode: StatusCodes.Status401Unauthorized);
        }
        
        return TypedResults.Empty;
    }
}
