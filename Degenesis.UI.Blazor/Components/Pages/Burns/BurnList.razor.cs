using Degenesis.Shared.DTOs;
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
        var result = await Client!.GetFromJsonAsync<Result<List<BurnDto>>>("/burns") ?? new Result<List<BurnDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading burns: " + result.Error);
            Burns = [];
        }
        else
            Burns = result.Value;
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
        var response = await Client!.DeleteAsync($"/burns/{burnId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

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