using System.Security.Claims;
using BeanLog.Shared.Web.Models.Identity.Session;
using BeanLog.Shared.Web.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanLog.Shared.Web.Areas.Identity;

[Area("identity")]
[Route("[area]/[controller]")]
public class SessionController : Controller
{
    private readonly SignInManager<User> _signInManager;

    public SessionController(SignInManager<User> signInManager)
    {
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
}
