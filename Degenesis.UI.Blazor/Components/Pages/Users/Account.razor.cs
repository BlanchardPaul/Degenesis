using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Account
{
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private AccountDisplayDto? account;
    private string? _userName;
    private string? _email;
    private string? _currentPassword;
    private string? _newPassword;
    private string? _confirmPassword;
    private bool _isRenaming;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        var userName = auth.User.FindFirst("unique_name")?.Value;
        if (string.IsNullOrEmpty(userName))
        {
            Snackbar.Add("Not connected", Severity.Error);
            return;
        }

        var result = await Client!.GetFromJsonAsync<Result<AccountDisplayDto>>($"/users/{userName}")
            ?? new Result<AccountDisplayDto> { IsError = true, Error = "Unknown error" };

        if (result.IsError)
        {
            Snackbar.Add("Error loading account: " + result.Error, Severity.Error);
            return;
        }

        account = result.Value;
        _userName = account!.UserName;
        _email = account.Email;
    }

    private async Task UpdateUserName()
    {
        _isRenaming = true;
        var response = await Client!.PutAsJsonAsync("/users/username",
            new UpdateUserNameDto { CurrentUserName = account!.UserName, NewUserName = _userName! });
        _isRenaming = false;

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        // The identity key changed: the current token is obsolete, the user must re-login
        Snackbar.Add("Name updated. Please log in again.", Severity.Info);
        await LogoutAsync();
    }

    private async Task UpdateEmail()
    {
        var response = await Client!.PutAsJsonAsync("/users/email",
            new UpdateEmailDto { UserName = account!.UserName, NewEmail = _email! });

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Email updated", Severity.Success);
        account.Email = _email!;
    }

    private async Task UpdatePassword()
    {
        var response = await Client!.PutAsJsonAsync("/users/password",
            new UpdatePasswordDto
            {
                UserName = account!.UserName,
                CurrentPassword = _currentPassword!,
                NewPassword = _newPassword!
            });

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Password updated", Severity.Success);
        _currentPassword = _newPassword = _confirmPassword = null;
    }

    private async Task LogoutAsync()
    {
        // Clear the protected local storage and notify the auth state, then redirect to the login page.
        await ProtectedLocalStorage.DeleteAsync("authToken");
        var authStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
        await authStateProvider.SetToken(string.Empty);
        NavigationManager.NavigateTo("/login");
        Navigation.NavigateTo("/login", forceLoad: true);
    }
    
}
