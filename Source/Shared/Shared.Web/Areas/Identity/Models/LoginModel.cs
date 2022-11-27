using System.ComponentModel.DataAnnotations;

namespace BeanLog.Shared.Web.Areas.Identity.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;

    public bool Persistent { get; set; }
    public string? ReturnUrl { get; init; }
}