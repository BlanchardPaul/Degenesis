using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class Register
{

    private UserCreateDto RegisterModel = new();

    private async Task HandleRegister()
    {
        var response = await Client!.PostAsJsonAsync("/users/register", RegisterModel);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Registration successful! Please log in.", Severity.Success);
        NavigationManager.NavigateTo("/login");

    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/");
    }
}