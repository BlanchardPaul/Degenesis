using Degenesis.Shared.DTOs.Characters;
using MudBlazor;
namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankList
{
    private List<RankDto>? ranks;
    private List<CultDto> cults = [];
    private List<RankPrerequisiteDto> rankPrerequisites = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadRanks();
    }

    private async Task LoadRanks()
    {
        ranks = await _client.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
        cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        rankPrerequisites = await _client.GetFromJsonAsync<List<RankPrerequisiteDto>>("/rank-prerequisites") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "Rank", new RankDto() },
            { "Cults", cults },
            { "RankPrerequisites", rankPrerequisites }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<RankModal>("Create Rank", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadRanks();
        }
    }

    private async Task ShowEditDialog(Guid rankId)
    {
        var rank = ranks?.FirstOrDefault(r => r.Id == rankId);
        if (rank != null)
        {
            var parameters = new DialogParameters
            {
                { "Rank", rank },
                { "Cults", cults },
                { "RankPrerequisites", rankPrerequisites }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<RankModal>("Edit Rank", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadRanks();
            }
        }
    }

    private async Task DeleteRank(Guid rankId)
    {
        var result = await _client.DeleteAsync($"/ranks/{rankId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadRanks();
    }

    private static string GetPrerequisiteLabel(RankPrerequisiteDto prerequisite)
    {
        if (prerequisite is null) return "Unknown Prerequisite";

        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        return $"{prerequisite.AttributeRequired.Name}{skillPart} = {prerequisite.SumRequired}";
    }
}
