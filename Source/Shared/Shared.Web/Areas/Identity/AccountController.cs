using BeanLog.Shared.Web.Areas.Identity.Models;
using BeanLog.Shared.Web.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BeanLog.Shared.Web.Areas.Identity;

[Area("identity")]
[Route("[area]/[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    [HttpGet("/Login")]
    public IActionResult Login([FromQuery] string? returnUrl = default)
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            if (returnUrl != default)
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

        var user = await _userManager.FindByEmailAsync(model.EmailAddress);
        
        if (user == null)
        {
            model.Password = string.Empty;
            
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return View(model);
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Persistent, false);

        if (!result.Succeeded)
        {
            model.Password = string.Empty;
            
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return View(model);
        }
        
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, userPrincipal);

        return LocalRedirect(model.ReturnUrl ?? "/");
    }
    
    [HttpGet("/Logout")]
    public async Task<IActionResult> Logout([FromQuery] string? returnUrl = default)
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
        }

        return LocalRedirect(returnUrl ?? "/");
    }

    [HttpGet("/Register")]
    public async Task<IActionResult> Register([FromQuery] string? returnUrl = default)
    {
        return View(new RegistrationModel()
        {
            ReturnUrl = returnUrl
        });
    }
    
    [HttpPost("/Register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Password = string.Empty;
            model.PasswordConfirmation = string.Empty;
            
            return View(model);
        }

        if (model.Password != model.PasswordConfirmation)
        {
            ModelState.AddModelError(nameof(model.PasswordConfirmation), "Passwords do not match.");
            
            model.Password = string.Empty;
            model.PasswordConfirmation = string.Empty;

            return View(model);
        }

        var result = await _userManager.CreateAsync(new User()
        {
            UserName = model.Username,
            Email = model.EmailAddress
        }, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                if (error.Code.StartsWith("Password"))
                {
                    ModelState.AddModelError(nameof(model.Password), error.Description);
                }
                else if (error.Code.Contains("UserName") || error.Code.Contains("Email"))
                {
                    ModelState.AddModelError(nameof(model.EmailAddress), error.Description);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (ModelState.ErrorCount == 0)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            }

            _logger.LogWarning($"Failed to register a new account. Errors: \n{string.Join("", result.Errors.Select(x => $"({x.Code}) {x.Description},\n"))}");

            model.Password = string.Empty;
            model.PasswordConfirmation = string.Empty;

            return View(model);
        }

        return RedirectToAction("Login", new { JustRegistered = true, ReturnUrl = model.ReturnUrl });
    }
}
