using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankPrerequisiteModal
{
    [Inject] private RankPrerequisiteService RankPrerequisiteService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankPrerequisiteDto RankPrerequisite { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    protected override void OnParametersSet()
    {
        if (RankPrerequisite.AttributeRequiredId == Guid.Empty && Attributes.Count!=0)
        {
            RankPrerequisite.AttributeRequiredId = Attributes.First().Id;
        }
    }

    private async Task SaveRankPrerequisite()
    {
        if (RankPrerequisite.Id == Guid.Empty)
            await RankPrerequisiteService.CreateRankPrerequisiteAsync(RankPrerequisite);
        else
            await RankPrerequisiteService.UpdateRankPrerequisiteAsync(RankPrerequisite);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
