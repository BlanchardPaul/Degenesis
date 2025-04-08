using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Login
{
    private UserLoginDto loginModel = new();

    private async Task HandleLogin()
    {
        var token = await UserService.LoginAsync(loginModel);
        if (token is not null)
        {
            var authStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
            await authStateProvider.SetToken(token);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add("Login failed", Severity.Error);
        }
    }

    private void NavigateToRegister()
    {
        NavigationManager.NavigateTo("/register");
    }
}