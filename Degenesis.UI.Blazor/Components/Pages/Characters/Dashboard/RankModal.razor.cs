using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankDto Rank { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    [Parameter] public List<RankPrerequisiteDto> RankPrerequisites { get; set; } = [];
    [Parameter] public List<RankDto> Ranks { get; set; } = [];
    public List<RankDto> SortedRanks { get; set; } = [];
    private HashSet<Guid> SelectedPrerequisiteIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        Rank.Prerequisites ??= [];
        SelectedPrerequisiteIds = [.. Rank.Prerequisites.Select(rp => rp.Id)];

        if (Rank.CultId == Guid.Empty && Cults.Any())
        {
            Rank.CultId = Cults.First().Id;
        }

        SortedRanks = [.. Ranks.Where(r => r.CultId == Rank.CultId)];

    }

    private Task OnPrerequisitesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedPrerequisiteIds = selectedValues.ToHashSet();
        Rank.Prerequisites = [.. RankPrerequisites.Where(rp => SelectedPrerequisiteIds.Contains(rp.Id))];
        return Task.CompletedTask;
    }

    private void HandleCultChange(Guid selectedCultureId)
    {
        Rank.CultId = selectedCultureId;
        SortedRanks = [.. Ranks.Where(r => r.CultId == selectedCultureId)];
        Rank.ParentRankId = null;
    }

    private async Task SaveRank()
    {
        if (Rank.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/ranks", Rank);
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
            var result = await _client.PutAsJsonAsync($"/ranks", Rank);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private string GetPrerequisiteLabel(Guid id)
    {
        var prerequisite = RankPrerequisites.FirstOrDefault(rp => rp.Id == id);
        if (prerequisite is null)
            return "Unknown Prerequisite";

        if (prerequisite.IsBackgroundPrerequisite && prerequisite.BackgroundRequired != null)
        {
            return $"{prerequisite.BackgroundRequired.Name} >= {prerequisite.BackgroundLevelRequired}";
        }

        string attributePart = prerequisite.AttributeRequired?.Name ?? "";
        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        string sumPart = prerequisite.SumRequired.HasValue ? $" >= {prerequisite.SumRequired}" : "";

        return $"{attributePart}{skillPart}{sumPart}";
    }
}
