using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialPrerequisiteList
{
    private List<PotentialPrerequisiteDto>? PotentialPrerequisites;
    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];
    private List<BackgroundDto> Backgrounds = [];
    private List<RankDto> Ranks = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadPotentialPrerequisites();
    }

    private async Task LoadPotentialPrerequisites()
    {
        PotentialPrerequisites = await Client!.GetFromJsonAsync<List<PotentialPrerequisiteDto>>("/potential-prerequisites") ?? [];
        Attributes = await Client!.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        Skills = await Client!.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
        Backgrounds = await Client!.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? [];
        Ranks = await Client!.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "PotentialPrerequisite", new PotentialPrerequisiteDto() },
            { "Attributes", Attributes },
            { "Skills", Skills },
            { "Backgrounds", Backgrounds },
            { "Ranks", Ranks }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<PotentialPrerequisiteModal>("Create Potential Prerequisite", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadPotentialPrerequisites();
        }
    }

    private async Task ShowEditDialog(Guid potentialPrerequisiteId)
    {
        var prerequisite = PotentialPrerequisites?.FirstOrDefault(pp => pp.Id == potentialPrerequisiteId);
        if (prerequisite != null)
        {
            var parameters = new DialogParameters
            {
                { "PotentialPrerequisite", prerequisite },
                { "Attributes", Attributes },
                { "Skills", Skills },
                { "Backgrounds", Backgrounds },
                { "Ranks", Ranks }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<PotentialPrerequisiteModal>("Edit Potential Prerequisite", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadPotentialPrerequisites();
            }
        }
    }

    private async Task DeletePotentialPrerequisite(Guid potentialPrerequisiteId)
    {
        var result = await Client!.DeleteAsync($"/potential-prerequisites/{potentialPrerequisiteId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadPotentialPrerequisites();
    }

    private bool FilterFunc(PotentialPrerequisiteDto potentialPrerequisite)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (potentialPrerequisite.AttributeRequired is not null)
            if (potentialPrerequisite.AttributeRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        if (potentialPrerequisite.SkillRequired is not null)
            if (potentialPrerequisite.SkillRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        if (potentialPrerequisite.BackgroundRequired is not null)
            if (potentialPrerequisite.BackgroundRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        if (potentialPrerequisite.RankRequired is not null)
            if (potentialPrerequisite.RankRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }
}
