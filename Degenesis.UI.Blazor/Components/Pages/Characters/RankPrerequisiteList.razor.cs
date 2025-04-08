using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankPrerequisiteList
{
    private List<RankPrerequisiteDto>? rankPrerequisites;
    private List<AttributeDto> attributes = [];
    private List<SkillDto> skills = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadRankPrerequisites();
    }

    private async Task LoadRankPrerequisites()
    {
        rankPrerequisites = await _client.GetFromJsonAsync<List<RankPrerequisiteDto>>("/rank-prerequisites") ?? [];
        attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
    }

    private async Task ShowCreateDialog()   
    {
        var parameters = new DialogParameters
        {
            { "RankPrerequisite", new RankPrerequisiteDto() },
            { "Attributes", attributes },
            { "Skills", skills }
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
        var rankPrerequisite = rankPrerequisites?.FirstOrDefault(rp => rp.Id == rankPrerequisiteId);
        if (rankPrerequisite != null)
        {
            var parameters = new DialogParameters
            {
                { "RankPrerequisite", rankPrerequisite },
                { "Attributes", attributes },
                { "Skills", skills }
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
        var result = await _client.DeleteAsync($"/rank-prerequisites/{rankPrerequisiteId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadRankPrerequisites();
    }
}
