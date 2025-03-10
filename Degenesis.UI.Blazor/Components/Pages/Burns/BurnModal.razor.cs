using Degenesis.Shared.DTOs.Burns;
using Degenesis.UI.Service.Features;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnModal
{
    [Inject] private BurnService BurnService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public BurnDto Burn { get; set; } = new();

    private async Task SaveBurn()
    {
        if (Burn.Id == Guid.Empty)
            await BurnService.CreateBurnAsync(Burn);
        else
            await BurnService.UpdateBurnAsync(Burn);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}