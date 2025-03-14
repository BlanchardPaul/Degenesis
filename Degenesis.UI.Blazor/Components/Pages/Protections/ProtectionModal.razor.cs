using Degenesis.Shared.DTOs.Protections;
using Degenesis.UI.Service.Features.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;

public partial class ProtectionModal
{
    [Inject] private ProtectionService ProtectionService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public ProtectionDto Protection { get; set; } = new();
    [Parameter] public List<ProtectionQualityDto> ProtectionQualities { get; set; } = new();

    private HashSet<Guid> SelectedQualityIds { get; set; } = new();

    protected override void OnParametersSet()
    {
        // Assurer la préselection des qualités existantes
        SelectedQualityIds = new HashSet<Guid>(Protection.Qualities.Select(q => q.Id));
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = new HashSet<Guid>(selectedValues);
        Protection.QualityIds = [.. SelectedQualityIds];
        return Task.CompletedTask;
    }

    private async Task SaveProtection()
    {
        if (Protection.Id == Guid.Empty)
            await ProtectionService.CreateProtectionAsync(Protection);
        else
            await ProtectionService.UpdateProtectionAsync(Protection);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}