using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Degenesis.UI.Blazor.Extensions;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Récupérer le token depuis ProtectedLocalStorage
            var tokenResult = await _protectedLocalStorage.GetAsync<string>("authToken");
            var token = tokenResult.Success ? tokenResult.Value : null;

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
        }
        // En cas d'erreur (par exemple, pendant le prerendering), retourner un état non authentifié
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task SetToken(string token)
    {
        // Stocker le token dans ProtectedLocalStorage
        await _protectedLocalStorage.SetAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string> GetToken()
    {
        // Récupérer le token depuis ProtectedLocalStorage
        var tokenResult = await _protectedLocalStorage.GetAsync<string>("authToken");
        return tokenResult.Success ? tokenResult.Value : null;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}