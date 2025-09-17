using Degenesis.Shared.DTOs.Burns;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnList
{
    private List<BurnDto>? Burns;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadBurns();
    }

    private async Task LoadBurns()
    {
        Burns = await _client.GetFromJsonAsync<List<BurnDto>>("/burns") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Burn", new BurnDto() } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<BurnModal>("Create Burn", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadBurns();
        }
    }

    private async Task ShowEditDialog(Guid burnId)
    {
        var burn = Burns?.FirstOrDefault(a => a.Id == burnId);
        if (burn != null)
        {
            var parameters = new DialogParameters { { "Burn", burn } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<BurnModal>("Edit Burn", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadBurns();
            }
        }
    }

    private async Task DeleteBurn(Guid burnId)
    {
        var result = await _client.DeleteAsync($"/burns/{burnId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadBurns();
    }
}