using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;
using System.Net.Http;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Login
{
    private UserLoginDto loginModel = new();

    private async Task HandleLogin()
    {
        var response = await Client!.PostAsJsonAsync("/users/login", loginModel);
        var result = await response.Content.ReadFromJsonAsync<Result<string>>();
        if (!response.IsSuccessStatusCode)
        {
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        var token = result?.Value;
        if (string.IsNullOrEmpty(token))
        {
            Snackbar.Add("Token is null or empty.", Severity.Error);
            return;
        }
        var authStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
        await authStateProvider.SetToken(token);
        Snackbar.Add("Successfuly logged in.", Severity.Success);
        NavigationManager.NavigateTo("/");

    }

    private void NavigateToRegister()
    {
        NavigationManager.NavigateTo("/register");
    }
}