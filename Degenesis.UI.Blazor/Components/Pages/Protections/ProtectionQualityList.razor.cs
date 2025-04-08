using Degenesis.Shared.DTOs.Protections;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;

public partial class ProtectionQualityList
{
    private List<ProtectionQualityDto>? protectionQualities;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadProtectionQualities();
    }

    private async Task LoadProtectionQualities()
    {
        protectionQualities = await _client.GetFromJsonAsync<List<ProtectionQualityDto>>("/protection-qualities") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "ProtectionQuality", new ProtectionQualityDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<ProtectionQualityModal>("Create Protection Quality", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadProtectionQualities();
        }
    }

    private async Task ShowEditDialog(Guid protectionQualityId)
    {
        var quality = protectionQualities?.FirstOrDefault(q => q.Id == protectionQualityId);
        if (quality != null)
        {
            var parameters = new DialogParameters
                {
                    { "ProtectionQuality", quality }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<ProtectionQualityModal>("Edit Protection Quality", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadProtectionQualities();
            }
        }
    }

    private async Task DeleteProtectionQuality(Guid protectionQualityId)
    {
        var result = await _client.DeleteAsync($"/protection-qualities/{protectionQualityId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadProtectionQualities();
    }
}
