using System.Security.Claims;
using BeanLog.Modules.Identity.Domain.Users;
using BeanLog.Modules.Identity.Web.Models.Session;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanLog.Modules.Identity.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Route("Api/[area]/[controller]/[action]")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;

    public SessionController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
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