using System.Security.Claims;
using BeanLog.Shared.Web.Areas.Identity.Models;
using BeanLog.Shared.Web.Models.Identity.Session;
using BeanLog.Shared.Web.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanLog.Shared.Web.Areas.Identity;

[Area("identity")]
[Route("[area]/[controller]")]
public class SessionController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public SessionController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("Current")]
    public SessionInfo Current()
    {
        if (!_signInManager.IsSignedIn(HttpContext.User))
        {
            return new SessionInfo();
        }

        return new SessionInfo()
        {
            State = SessionState.Active,
            Claims = HttpContext.User.Claims
                .Where(x => x.Type is ClaimTypes.Name or ClaimTypes.Email or ClaimTypes.NameIdentifier)
                .Select(x => new KeyValuePair<string, string>(x.Type, x.Value)).ToHashSet()
        };
    }

    [HttpGet("/Login")]
    public IActionResult Login(string? returnUrl)
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return LocalRedirect("/");
        }

        return View(new LoginModel()
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost("/Login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Password = string.Empty;

            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.Persistent, false);

        if (!result.Succeeded)
        {
            model.Password = string.Empty;
            
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return View(model);
        }

        var user = await _signInManager.UserManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            model.Password = string.Empty;
            
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return View(model);
        }
        
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, userPrincipal);

        return LocalRedirect(model.ReturnUrl ?? "/");
    }
}
