using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class UsersManagement : AuthenticatedComponentBase
{
    private List<UserDtoForAdmin>? Users;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadUsersAsync();
    }

    private async Task EditRole(UserDtoForAdmin user)
    {
        var parameters = new DialogParameters<EditUserRoleDialog>
        {
            { dialog => dialog.CurrentRole, user.Role }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<EditUserRoleDialog>("Edit role", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Canceled || dialogResult.Data is not string role)
            return;

        var response = await Client!.PutAsJsonAsync($"/users/management/{Uri.EscapeDataString(user.UserName)}/role", role);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? $"Error updating role: {response.StatusCode}", Severity.Error);
            return;
        }

        Snackbar.Add("Role updated", Severity.Success);
        await LoadUsersAsync();
    }

    private async Task ConfirmDeleteUser(UserDtoForAdmin user)
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { dialog => dialog.ContentText, $"Delete user '{user.UserName}'?" }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult is not null && !dialogResult.Canceled)
            await DeleteUserAsync(user);
    }

    private async Task DeleteUserAsync(UserDtoForAdmin user)
    {
        var response = await Client!.DeleteAsync($"/users/management/{Uri.EscapeDataString(user.UserName)}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? $"Error deleting user: {response.StatusCode}", Severity.Error);
            return;
        }

        Snackbar.Add("User deleted", Severity.Success);
        await LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        var response = await Client!.GetAsync("/users/management");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<List<UserDtoForAdmin>>>();
            Snackbar.Add(result?.Error ?? $"Error loading users: {response.StatusCode}", Severity.Error);
            return;
        }

        var userResult = await response.Content.ReadFromJsonAsync<Result<List<UserDtoForAdmin>>>();
        if (userResult is null || userResult.IsError)
        {
            Snackbar.Add(userResult?.Error ?? "Unable to read the users response", Severity.Error);
            return;
        }

        Users = userResult.Value ?? [];
    }
}
