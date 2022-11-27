using System.Security.Claims;
using BeanLog.Shared.Web.Models.Identity.Session;
using Microsoft.AspNetCore.Components.Authorization;

namespace BeanLog.Spa.Infrastructure.Authorization;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(await GetClaimsPrincipal());
    }

    private async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        var sessionInfo = await GetSessionInfo();

        if (sessionInfo is not {State: SessionState.Active})
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var claimsIdentity = new ClaimsIdentity(sessionInfo.Claims.Select(x => new Claim(x.Key, x.Value)), "API");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }

    private async Task<SessionInfo?> GetSessionInfo()
    {
        /*var client = _httpClientFactory.CreateClient("API");
        var response = await client.GetAsync("/identity/sessions/current");
        
        return await JsonSerializer.DeserializeAsync<SessionInfo>(await response.Content.ReadAsStreamAsync());*/

        return new SessionInfo();
    }
}