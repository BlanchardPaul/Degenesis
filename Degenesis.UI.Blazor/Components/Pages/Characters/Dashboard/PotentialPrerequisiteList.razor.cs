using Degenesis.Shared.DTOs;
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
        var potentialPrerequisiteResult = await Client!.GetFromJsonAsync<Result<List<PotentialPrerequisiteDto>>>("/potential-prerequisites") ?? new Result<List<PotentialPrerequisiteDto>> { IsError = true, Error = "Unknown error" };
        if (potentialPrerequisiteResult.IsError)
        {
            Snackbar.Add($"Error loading potential prerequisites: {potentialPrerequisiteResult.Error}", Severity.Error);
            PotentialPrerequisites = [];
        }else
            PotentialPrerequisites = potentialPrerequisiteResult.Value ?? [];

        var attributeResult = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if (attributeResult.IsError)
        {
            Snackbar.Add($"Error loading attributes: {attributeResult.Error}", Severity.Error);
            Attributes = [];
        }
        else
            Attributes = attributeResult.Value ?? [];

        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillResult.IsError)
        {
            Snackbar.Add($"Error loading skills: {skillResult.Error}", Severity.Error);
            Skills = [];
        }
        else
            Skills = skillResult.Value ?? [];

        var backgroundResult = await Client!.GetFromJsonAsync<Result<List<BackgroundDto>>>("/backgrounds") ?? new Result<List<BackgroundDto>> { IsError = true, Error = "Unknown error" };
        if (backgroundResult.IsError)
        {
            Snackbar.Add($"Error loading backgrounds: {backgroundResult.Error}", Severity.Error);
            Backgrounds = [];
        }
        else
            Backgrounds = backgroundResult.Value ?? [];
        
        var rankResult = await Client!.GetFromJsonAsync<Result<List<RankDto>>>("/ranks") ?? new Result<List<RankDto>> { IsError = true, Error = "Unknown error" };
        if (rankResult.IsError)
        {
            Snackbar.Add($"Error loading ranks: {rankResult.Error}", Severity.Error);
            Ranks = [];
        }
        else
            Ranks = rankResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/potential-prerequisites/{potentialPrerequisiteId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
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
