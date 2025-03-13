using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class BackgroundList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<BackgroundDto>? backgrounds;

    protected override async Task OnInitializedAsync()
    {
        await LoadBackgrounds();
    }

    private async Task LoadBackgrounds()
    {
        backgrounds = await BackgroundService.GetBackgroundsAsync();
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
        await BackgroundService.DeleteBackgroundAsync(backgroundId);
        backgrounds = await BackgroundService.GetBackgroundsAsync();
    }

}