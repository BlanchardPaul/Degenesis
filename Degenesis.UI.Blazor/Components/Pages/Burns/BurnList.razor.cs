using Degenesis.Shared.DTOs.Burns;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<BurnDto>? burns;

    protected override async Task OnInitializedAsync()
    {
        await LoadBurns();
    }

    private async Task LoadBurns()
    {
        burns = await BurnService.GetBurnsAsync();
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
        var burn = burns?.FirstOrDefault(a => a.Id == burnId);
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
        await BurnService.DeleteBurnAsync(burnId);
        burns = await BurnService.GetBurnsAsync();
    }
}