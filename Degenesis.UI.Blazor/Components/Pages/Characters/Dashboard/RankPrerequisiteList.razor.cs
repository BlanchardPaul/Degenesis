using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankPrerequisiteList
{
    private List<RankPrerequisiteDto>? RankPrerequisites;
    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];
    private List<BackgroundDto> Backgrounds  = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadRankPrerequisites();
    }

    private async Task LoadRankPrerequisites()
    {
        RankPrerequisites = await _client.GetFromJsonAsync<List<RankPrerequisiteDto>>("/rank-prerequisites") ?? [];
        Attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        Skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
        Backgrounds = await _client.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? [];
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
        var result = await _client.DeleteAsync($"/rank-prerequisites/{rankPrerequisiteId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadRankPrerequisites();
    }
}
