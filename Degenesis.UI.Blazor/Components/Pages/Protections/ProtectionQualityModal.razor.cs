using Degenesis.Shared.DTOs.Protections;
using Degenesis.UI.Service.Features.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;

public partial class ProtectionQualityModal
{
    [Inject] private ProtectionQualityService ProtectionQualityService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public ProtectionQualityDto ProtectionQuality { get; set; } = new();

    private async Task SaveProtectionQuality()
    {
        if (ProtectionQuality.Id == Guid.Empty)
            await ProtectionQualityService.CreateProtectionQualityAsync(ProtectionQuality);
        else
            await ProtectionQualityService.UpdateProtectionQualityAsync(ProtectionQuality);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}