using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class SkillList
{
    private List<SkillDto>? skills;
    private List<AttributeDto> attributes = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadSkills();
    }

    private async Task LoadSkills()
    {
        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillResult.IsError)
        {
            Snackbar.Add($"Error loading skills: {skillResult.Error}", Severity.Error);
            skills = [];
        }
        else
            skills = skillResult.Value ?? [];
        var attributeResult = await Client!.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        if (attributeResult is null)
        {
            Snackbar.Add($"Error loading attributes", Severity.Error);
            attributes = [];
        }
        else
            attributes = attributeResult;
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Skill", new SkillDto() }, { "Attributes", attributes } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<SkillModal>("Create Skill", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadSkills();
        }
    }

    private async Task ShowEditDialog(Guid skillId)
    {
        var skill = skills?.FirstOrDefault(s => s.Id == skillId);
        if (skill != null)
        {
            var parameters = new DialogParameters { { "Skill", skill }, { "Attributes", attributes } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<SkillModal>("Edit Skill", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadSkills();
            }
        }
    }

    private async Task DeleteSkill(Guid skillId)
    {
        var response = await Client!.DeleteAsync($"/skills/{skillId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadSkills();
    }

    private bool FilterFunc(SkillDto skill)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (skill.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}