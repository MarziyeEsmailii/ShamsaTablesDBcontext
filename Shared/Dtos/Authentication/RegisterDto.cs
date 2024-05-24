using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Authentication;

public class RegisterDto
{
    /// <summary>
    /// این برای درخواست ثبت نام است
    /// </summary>
    [Required(ErrorMessage = "ایمیل را وارد کنید")]
    public string Email { get; set; }

    [Required(ErrorMessage = "نام و نام خانوادگی خود را وارد کنید")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    public string Password { get; set; }
}
