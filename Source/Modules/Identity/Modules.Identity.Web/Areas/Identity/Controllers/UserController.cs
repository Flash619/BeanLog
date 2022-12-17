using BeanLog.Modules.Identity.Domain.Users;
using BeanLog.Modules.Identity.Web.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanLog.Modules.Identity.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Route("Api/[area]/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.EmailAddress);

        if (user == null)
        {
            return await DelayedUnauthorized();
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, true, false);

        if (!result.Succeeded)
        {
            return await DelayedUnauthorized();
        }

        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, userPrincipal);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        if (!_signInManager.IsSignedIn(HttpContext.User))
        {
            return Ok();
        }

        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync();

        return Ok();
    }

    private async Task<IActionResult> DelayedUnauthorized()
    {
        await Task.Delay(new Random().Next(1000, 1500));

        return Unauthorized();
    }
}