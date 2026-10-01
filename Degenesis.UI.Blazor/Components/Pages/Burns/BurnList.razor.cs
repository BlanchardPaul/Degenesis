using Degenesis.Shared.DTOs.Burns;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnList : AuthenticatedComponentBase
{
    private List<BurnDto>? Burns;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadBurns();
    }

    private async Task LoadBurns()
    {
        Burns = await Client!.GetFromJsonAsync<List<BurnDto>>("/burns") ?? [];
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
        var result = await Client!.DeleteAsync($"/burns/{burnId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadBurns();
    }

    private bool FilterFunc(BurnDto burn)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (burn.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}