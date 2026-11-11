using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
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
        var token = await GetToken();
        return await BuildAuthenticationStateAsync(token);
    }

    public async Task SetToken(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                await _protectedLocalStorage.DeleteAsync("authToken");
            }
            else
            {
                await _protectedLocalStorage.SetAsync("authToken", token);
            }
        }
        catch (Exception ex) when (IsPrerenderInteropException(ex))
        {
            // During prerender, browser storage is not yet available. We continue with the in-memory state.
        }

        var authState = await BuildAuthenticationStateAsync(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    private async Task<AuthenticationState> BuildAuthenticationStateAsync(string? token)
    {
        try
        {
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
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task<string?> GetToken()
    {
        try
        {
            var tokenResult = await _protectedLocalStorage.GetAsync<string>("authToken");
            return tokenResult.Success ? tokenResult.Value : null;
        }
        catch (Exception ex) when (IsPrerenderInteropException(ex))
        {
            return null;
        }
    }

    private static bool IsPrerenderInteropException(Exception exception)
    {
        return exception is JSDisconnectedException
            || (exception is InvalidOperationException
                && exception.Message.Contains("JavaScript interop calls cannot be issued at this time", StringComparison.OrdinalIgnoreCase));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}