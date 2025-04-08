using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankDto Rank { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = new();
    [Parameter] public List<RankPrerequisiteDto> RankPrerequisites { get; set; } = new();

    private HashSet<Guid> SelectedPrerequisiteIds { get; set; } = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        Rank.Prerequisites ??= new List<RankPrerequisiteDto>();
        SelectedPrerequisiteIds = Rank.Prerequisites.Select(rp => rp.Id).ToHashSet();

        if (Rank.CultId == Guid.Empty && Cults.Any())
        {
            Rank.CultId = Cults.First().Id;
        }
    }

    private Task OnPrerequisitesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedPrerequisiteIds = selectedValues.ToHashSet();
        Rank.Prerequisites = RankPrerequisites.Where(rp => SelectedPrerequisiteIds.Contains(rp.Id)).ToList();
        return Task.CompletedTask;
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
        if (prerequisite is null) return "Unknown Prerequisite";

        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        return $"{prerequisite.AttributeRequired.Name}{skillPart} = {prerequisite.SumRequired}";
    }
}
