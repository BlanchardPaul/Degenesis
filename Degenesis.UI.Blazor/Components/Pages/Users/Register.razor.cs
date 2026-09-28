using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Service.Features.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Register
{

    private UserCreateDto RegisterModel = new();

    private async Task HandleRegister()
    {
        var success = await UserService.RegisterAsync(RegisterModel);
        if (success)
        {
            Snackbar.Add("Registration successful! Please log in.", Severity.Success);
            NavigationManager.NavigateTo("/login");
        }
        else
        {
            Snackbar.Add("Registration failed. Please try again.", Severity.Error);
        }
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/");
    }
}