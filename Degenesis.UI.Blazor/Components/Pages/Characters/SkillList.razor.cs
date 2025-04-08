using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class SkillList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<SkillDto>? skills;
    private List<AttributeDto> attributes = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadSkills();
        attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
    }

    private async Task LoadSkills()
    {
        skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
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
        var result = await _client.DeleteAsync($"/skills/{skillId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadSkills();
    }
}