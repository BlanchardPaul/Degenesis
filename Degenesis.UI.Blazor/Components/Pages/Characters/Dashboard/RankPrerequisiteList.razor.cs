using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankPrerequisiteList
{
    private List<RankPrerequisiteDto>? RankPrerequisites;
    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];
    private List<BackgroundDto> Backgrounds  = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadRankPrerequisites();
    }

    private async Task LoadRankPrerequisites()
    {
        var rankPrerequisiteResult = await Client!.GetFromJsonAsync<Result<List<RankPrerequisiteDto>>>("/rank-prerequisites") ?? new Result<List<RankPrerequisiteDto>> { IsError = true, Error = "Unknown error" };
        if(rankPrerequisiteResult.IsError)
        {
            Snackbar.Add($"Error loading rank prerequisites: {rankPrerequisiteResult.Error}", Severity.Error);
            RankPrerequisites = [];
        }
        else
            RankPrerequisites = rankPrerequisiteResult.Value ?? [];

        var attributeResult = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if(attributeResult.IsError)
        {
            Snackbar.Add($"Error loading attributes: {attributeResult.Error}", Severity.Error);
            Attributes = [];
        }
        else
            Attributes = attributeResult.Value ?? [];

        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if(skillResult.IsError)
        {
            Snackbar.Add($"Error loading skills: {skillResult.Error}", Severity.Error);
            Skills = [];
        }
        else
            Skills = skillResult.Value ?? [];

        var backgroundResult = await Client!.GetFromJsonAsync<Result<List<BackgroundDto>>>("/backgrounds") ?? new Result<List<BackgroundDto>> { IsError = true, Error = "Unknown error" };
        if(backgroundResult.IsError)
        {
            Snackbar.Add($"Error loading backgrounds: {backgroundResult.Error}", Severity.Error);
            Backgrounds = [];
        }
        else
            Backgrounds = backgroundResult.Value ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "RankPrerequisite", new RankPrerequisiteDto() },
            { "Attributes", Attributes },
            { "Skills", Skills },
            { "Backgrounds", Backgrounds }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<RankPrerequisiteModal>("Create Rank Prerequisite", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadRankPrerequisites();
        }
    }

    private async Task ShowEditDialog(Guid rankPrerequisiteId)
    {
        var rankPrerequisite = RankPrerequisites?.FirstOrDefault(rp => rp.Id == rankPrerequisiteId);
        if (rankPrerequisite != null)
        {
            var parameters = new DialogParameters
            {
                { "RankPrerequisite", rankPrerequisite },
                { "Attributes", Attributes },
                { "Skills", Skills },
                { "Backgrounds", Backgrounds }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<RankPrerequisiteModal>("Edit Rank Prerequisite", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadRankPrerequisites();
            }
        }
    }

    private async Task DeleteRankPrerequisite(Guid rankPrerequisiteId)
    {
        var response = await Client!.DeleteAsync($"/rank-prerequisites/{rankPrerequisiteId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadRankPrerequisites();
    }

    private bool FilterFunc(RankPrerequisiteDto rankPrerequisite)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (rankPrerequisite.AttributeRequired is not null)
            if (rankPrerequisite.AttributeRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        if (rankPrerequisite.SkillRequired is not null)
            if (rankPrerequisite.SkillRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        if (rankPrerequisite.BackgroundRequired is not null)
            if (rankPrerequisite.BackgroundRequired.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }
}
