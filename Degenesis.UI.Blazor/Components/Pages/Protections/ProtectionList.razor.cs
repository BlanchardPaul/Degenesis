using Degenesis.Shared.DTOs.Protections;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;


public partial class ProtectionList
{
    private List<ProtectionDto>? protections;
    private List<ProtectionQualityDto> protectionQualities = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadProtections();
    }

    private async Task LoadProtections()
    {
        protections = [.. (await ProtectionService.GetProtectionsAsync())];
        protectionQualities = [.. (await ProtectionQualityService.GetProtectionQualitysAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Protection", new ProtectionDto() },
                { "ProtectionQualities", protectionQualities }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<ProtectionModal>("Create Protection", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadProtections();
        }
    }

    private async Task ShowEditDialog(Guid protectionId)
    {
        var protection = protections?.FirstOrDefault(p => p.Id == protectionId);
        if (protection != null)
        {
            var parameters = new DialogParameters
                {
                    { "Protection", protection },
                    { "ProtectionQualities", protectionQualities }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<ProtectionModal>("Edit Protection", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadProtections();
            }
        }
    }

    private async Task DeleteProtection(Guid protectionId)
    {
        await ProtectionService.DeleteProtectionAsync(protectionId);
        await LoadProtections();
    }
}