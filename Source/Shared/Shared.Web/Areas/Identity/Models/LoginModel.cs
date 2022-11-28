using System.ComponentModel.DataAnnotations;

namespace BeanLog.Shared.Web.Areas.Identity.Models;

public class LoginModel
{
    [EmailAddress(ErrorMessage = "Email address is invalid.")]
    [Required(ErrorMessage = "Email address is required.")]
    public string EmailAddress { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;

    public bool Persistent { get; set; }
    
    public string? ReturnUrl { get; set; }
}