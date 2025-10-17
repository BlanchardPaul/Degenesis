using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;
namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankList
{
    private List<RankDto>? Ranks;
    private List<CultDto> Cults = [];
    private List<RankPrerequisiteDto> RankPrerequisites = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadRanks();
    }

    private async Task LoadRanks()
    {
        Ranks = await _client.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
        Cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        RankPrerequisites = await _client.GetFromJsonAsync<List<RankPrerequisiteDto>>("/rank-prerequisites") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "Rank", new RankDto() },
            { "Cults", Cults },
            { "RankPrerequisites", RankPrerequisites },
            { "Ranks", Ranks }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<RankModal>("Create Rank", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadRanks();
            StateHasChanged();
        }
    }

    private async Task ShowEditDialog(Guid rankId)
    {
        var rank = Ranks?.FirstOrDefault(r => r.Id == rankId);
        if (rank != null)
        {
            var parameters = new DialogParameters
            {
                { "Rank", rank },
                { "Cults", Cults },
                { "RankPrerequisites", RankPrerequisites },
                { "Ranks", Ranks }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<RankModal>("Edit Rank", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadRanks();
                StateHasChanged();
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
        StateHasChanged();
    }

    private static string GetPrerequisiteLabel(RankPrerequisiteDto prerequisite)
    {
        if (prerequisite == null)
            return "Unknown";

        if (prerequisite.IsBackgroundPrerequisite && prerequisite.BackgroundRequired != null)
        {
            return $"{prerequisite.BackgroundRequired.Name} >= {prerequisite.BackgroundLevelRequired}";
        }

        string attributePart = prerequisite.AttributeRequired?.Name ?? "";
        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        string sumPart = prerequisite.SumRequired.HasValue ? $" >= {prerequisite.SumRequired}" : "";

        return $"{attributePart}{skillPart}{sumPart}";
    }
}
