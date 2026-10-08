using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public PotentialDto Potential { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    [Parameter] public List<PotentialPrerequisiteDto> PotentialPrerequisites { get; set; } = [];

    private HashSet<Guid> SelectedPrerequisiteIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        Potential.Prerequisites ??= [];
        SelectedPrerequisiteIds = [.. Potential.Prerequisites.Select(pp => pp.Id)];

        if (!Potential.CultId.HasValue && Cults.Count != 0)
        {
            Potential.CultId = Cults.First().Id;
        }
    }

    private Task OnPrerequisitesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedPrerequisiteIds = [.. selectedValues];
        Potential.Prerequisites = [.. PotentialPrerequisites.Where(pp => SelectedPrerequisiteIds.Contains(pp.Id))];
        return Task.CompletedTask;
    }

    private async Task SavePotential()
    {
        HttpResponseMessage response;
        if (Potential.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/potentials", Potential);
        else
            response = await Client!.PutAsJsonAsync($"/potentials", Potential);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Potential.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private string GetPrerequisiteLabel(Guid id)
    {
        var prerequisite = PotentialPrerequisites.FirstOrDefault(pp => pp.Id == id);
        if (prerequisite is null)
            return "Unknown Prerequisite";

        if (prerequisite.IsBackgroundPrerequisite && prerequisite.BackgroundRequired != null)
        {
            return $"{prerequisite.BackgroundRequired.Name} >= {prerequisite.BackgroundLevelRequired}";
        }

        if (prerequisite.IsRankPrerequisite && prerequisite.RankRequired != null)
        {
            return $"Rank: {prerequisite.RankRequired.Name}";
        }

        string attributePart = prerequisite.AttributeRequired?.Name ?? "";
        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        string sumPart = prerequisite.SumRequired.HasValue ? $" >= {prerequisite.SumRequired}" : "";

        return $"{attributePart}{skillPart}{sumPart}".Trim();
    }
}