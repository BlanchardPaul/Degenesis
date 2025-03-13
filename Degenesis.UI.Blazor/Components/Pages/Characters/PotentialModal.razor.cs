using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class PotentialModal
{
    [Inject] private PotentialService PotentialService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public PotentialDto Potential { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = new();

    protected override void OnParametersSet()
    {
        if (!Potential.CultId.HasValue && Cults.Any())
        {
            Potential.CultId = Cults.First().Id;
        }
    }

    private async Task SavePotential()
    {
        if (Potential.Id == Guid.Empty)
            await PotentialService.CreatePotentialAsync(Potential);
        else
            await PotentialService.UpdatePotentialAsync(Potential);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
