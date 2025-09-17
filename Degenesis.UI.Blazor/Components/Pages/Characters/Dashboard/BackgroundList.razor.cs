using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class BackgroundList
{
    private List<BackgroundDto>? backgrounds;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadBackgrounds();
    }

    private async Task LoadBackgrounds()
    {
        backgrounds = await _client.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? [];
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
        var result = await _client.DeleteAsync($"/backgrounds/{backgroundId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadBackgrounds();
    }

}