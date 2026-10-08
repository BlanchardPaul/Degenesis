using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterSheetModals;

public partial class CharacterAddPotentialModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<PotentialDto>? Potentials { get; set; }

    private Guid SelectedPotentialId;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        List<Guid> ownedPotentialIds = [.. Character.Potentials.Select(p => p.PotentialId)];
        Potentials = await Client!.GetFromJsonAsync<List<PotentialDto>>("/potentials") ?? [];
        Potentials = Potentials = [.. Potentials
        .Where(p => (p.CultId == Character.Cult.Id || p.CultId is null)
                    && !ownedPotentialIds.Contains(p.Id))];
    }

    private bool IsPotentialDisabled(PotentialDto potential)
    {

        foreach (var prereq in potential.Prerequisites ?? Enumerable.Empty<PotentialPrerequisiteDto>())
        {
            if (prereq.IsBackgroundPrerequisite)
            {
                var bg = Character.Backgrounds.FirstOrDefault(b => b.BackgroundId == prereq.BackgroundRequiredId!.Value);
                if (bg is null || bg.Level < (prereq.BackgroundLevelRequired ?? 0))
                    return true;
            }

            else if (prereq.IsRankPrerequisite)
            {
                if (Character.Rank.Id != prereq.RankRequiredId)
                    return true;
            }

            else
            {
                int attrLevel = 0;
                int skillLevel = 0;

                if (prereq.AttributeRequiredId.HasValue)
                    attrLevel = Character.Attributes.FirstOrDefault(a => a.AttributeId == prereq.AttributeRequiredId)?.Level ?? 0;

                if (prereq.SkillRequiredId.HasValue)
                    skillLevel = Character.Skills.FirstOrDefault(s => s.SkillId == prereq.SkillRequiredId)?.Level ?? 0;

                int sum = attrLevel + skillLevel;

                if (!prereq.SumRequired.HasValue || sum < prereq.SumRequired.Value)
                    return true;
            }
        }

        return false;
    }

    private static string GetPotentialPrerequisiteLabel(PotentialPrerequisiteDto prerequisite)
    {
        if (prerequisite is null)
            return "Unknown";

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

    private async Task ConfirmSelection()
    {
        if (SelectedPotentialId == Guid.Empty)
        {
            Snackbar.Add("Please select a potential first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-potentials/", new CharacterGuidValueEditDto { Id = Character.Id, Value = SelectedPotentialId });
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Character potential added successfully.", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
