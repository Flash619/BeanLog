using System.ComponentModel.DataAnnotations;

namespace BeanLog.Shared.Web.Areas.Identity.Models;

public class RegistrationModel
{
    [MaxLength(25, ErrorMessage = "Username must be less than 25 characters.")]
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email address is invalid.")]
    [Required(ErrorMessage = "Email address is required.")]
    public string EmailAddress { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirmation is required.")]
    public string PasswordConfirmation { get; set; }
    
    public string? ReturnUrl { get; set; }
}