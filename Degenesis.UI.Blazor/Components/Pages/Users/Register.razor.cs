using Degenesis.Shared.DTOs.Users;
using Degenesis.UI.Service.Features.Users;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Register
{

    private UserCreateDto registerModel = new();

    private async Task HandleRegister()
    {
        var success = await UserService.RegisterAsync(registerModel);
        if (success)
        {
            NavigationManager.NavigateTo("/login");
        }
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/");
    }
}