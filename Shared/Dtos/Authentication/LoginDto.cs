using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Authentication;

public class LoginDto
{
    /// <summary>
    /// این برای درخواست ورود است
    /// </summary>
    [Required(ErrorMessage = "ایمیل را وارد کنید")]
    public string Email { get; set; }

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    public string Password { get; set; }
}
