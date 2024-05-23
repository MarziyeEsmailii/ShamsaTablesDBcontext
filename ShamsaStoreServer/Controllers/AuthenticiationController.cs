using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ShamsaStoreServer.Entities;
using System.Threading.Tasks;


namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AuthenctiationUser> _userManager;

        private readonly SignInManager<AuthenctiationUser> _signInManager;

        public AuthenticationController(UserManager<AuthenctiationUser> userManager, SignInManager<AuthenctiationUser> signInManager)
        {
            _userManager = userManager;

            _signInManager = signInManager;
        }

        #region ثبت نام کاربر جدید در سیستم 
        /// <summary>
        /// ثبت نام کاربر جدید در سیستم.
        /// </summary>
        /// <param name="model">مدل اطلاعات کاربر جدید.</param>
        /// <returns>یک پاسخ HTTP که شامل وضعیت کاربری است.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // جستجوی کاربر با ایمیل موجود
            var user =
               await _userManager.FindByEmailAsync(model.Email);

            // بررسی اینکه آیا کاربر با ایمیل مورد نظر وجود دارد یا خیر
            if (user is not null)
            {
                return BadRequest("با این ایمیل قبلا ثبت نام شده است");
            }

            var hasher =
            new PasswordHasher<IdentityUser>();

            // ایجاد یک کاربر جدید با اطلاعات دریافتی
            user = new AuthenctiationUser
            {
                Email = model.Email,
                PhoneNumber = model.Phone,
                FullName = model.FullName,
                UserName = model.Email,
                PasswordHash = hasher.HashPassword(null, model.Password),
            };

            // تلاش برای ایجاد کاربر در سیستم
            var result =
                await _userManager.CreateAsync(user);

            // بررسی نتیجه ایجاد کاربر
            if (result.Succeeded)
            {
                // تلاش برای افزودن نقش "User" به کاربر
                var roleResult =
                   await _userManager.AddToRoleAsync(user, "User");

                // بررسی نتیجه افزودن نقش
                if (roleResult.Succeeded)
                    return Ok(); // در صورت موفقیت، پاسخ موفقیت آمیز برمی‌گرداند
            }

            // در صورتی که هر مرحله‌ای شکست خورد، خطای سروری را برمی‌گرداند
            return BadRequest("سرور به مشکل خورده است بعدا امتحان کنید");
        }
        #endregion



        #region ورود کاربر 
        /// <summary>
        /// ورود کاربر به سیستم با استفاده از اطلاعات ورود.
        /// </summary>
        /// <param name="model">مدل اطلاعات ورود کاربر.</param>
        /// <param name="useCookies">بررسی استفاده از کوکی ها.</param>
        /// <param name="useSessionCookies">بررسی استفاده از کوکی های جلسه.</param>
        /// <returns>یک پاسخ HTTP که شامل نتایج مختلفی می‌باشد.</returns>
        [HttpPost("Login")]
        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login([FromBody] LoginDto model,
           [FromQuery] bool? useCookies,
            [FromQuery] bool? useSessionCookies)
        {
            // تعیین استفاده از کوکی یا بئره
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);


            // تعیین اینکه آیا کوکی باید پایدار باشد یا خیر
            var isPersistent = (useCookies == true) && (useSessionCookies != true);


            // تنظیم اسکیم احراز هویت
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;


            // جستجوی کاربر با ایمیل وارد شده
            var user =
            await _userManager.FindByEmailAsync(model.Email);


            // بررسی اینکه آیا کاربر وجود دارد یا خیر
            if (user == null)
            {
                // در صورت عدم وجود کاربر، خطای مربوطه را برمی‌گرداند
                return TypedResults.Problem("ایمیل یا رمز عبور اشتباه است", statusCode: StatusCodes.Status401Unauthorized);
            }


            // تلاش برای ورود کاربر با استفاده از رمز عبور
            var result =
          await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent, lockoutOnFailure: true);

            // بررسی نتیجه ورود کاربر
            if (!result.Succeeded)
            {
                // در صورت عدم موفقیت در ورود، خطای مربوطه را برمی‌گرداند
                return TypedResults.Problem("ایمیل یا رمز عبور اشتباه است", statusCode: StatusCodes.Status401Unauthorized);
            }

            // در صورت موفقیت در ورود، هیچ پاسخی بر نمی‌گرداند
            return TypedResults.Empty;
        }
        #endregion
    }
}
