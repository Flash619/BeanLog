using System.ComponentModel.DataAnnotations;

namespace BeanLog.Modules.Identity.Web.Models.User;

public class UserLoginRequest
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; init; }
    
    [Required]
    public string Password { get; init; }
}