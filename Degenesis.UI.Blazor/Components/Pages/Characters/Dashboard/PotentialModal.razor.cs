using Degenesis.Shared.DTOs.Characters;
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
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        Potential.Prerequisites ??= [];
        SelectedPrerequisiteIds = Potential.Prerequisites.Select(pp => pp.Id).ToHashSet();

        if (!Potential.CultId.HasValue && Cults.Any())
        {
            Potential.CultId = Cults.First().Id;
        }
    }

    private Task OnPrerequisitesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedPrerequisiteIds = selectedValues.ToHashSet();
        Potential.Prerequisites = PotentialPrerequisites
            .Where(pp => SelectedPrerequisiteIds.Contains(pp.Id))
            .ToList();
        return Task.CompletedTask;
    }

    private async Task SavePotential()
    {
        if (Potential.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/potentials", Potential);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync($"/potentials", Potential);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
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