using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class BackgroundList
{
    private List<BackgroundDto>? backgrounds;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadBackgrounds();
    }

    private async Task LoadBackgrounds()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<BackgroundDto>>>("/backgrounds") ?? new Result<List<BackgroundDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading backgrounds: " + result.Error);
            backgrounds = [];
        }
        else
            backgrounds = result.Value ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Background", new BackgroundDto() } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<BackgroundModal>("Create Background", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadBackgrounds();
        }
    }

    private async Task ShowEditDialog(Guid attributeId)
    {
        var attribute = backgrounds?.FirstOrDefault(a => a.Id == attributeId);
        if (attribute != null)
        {
            var parameters = new DialogParameters { { "Background", attribute } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<BackgroundModal>("Edit Background", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadBackgrounds();
            }
        }
    }

    private async Task DeleteBackground(Guid backgroundId)
    {
        var response = await Client!.DeleteAsync($"/backgrounds/{backgroundId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadBackgrounds();
    }

    private bool FilterFunc(BackgroundDto background)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (background.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}