using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;
namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankList
{
    private List<RankDto>? Ranks;
    private List<CultDto> Cults = [];
    private List<RankPrerequisiteDto> RankPrerequisites = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadRanks();
    }

    private async Task LoadRanks()
    {
        var rankResult = await Client!.GetFromJsonAsync<Result<List<RankDto>>>("/ranks") ?? new Result<List<RankDto>> { IsError = true, Error = "Unknown error" };
        if(rankResult.IsError)
        {
            Snackbar.Add($"Error loading ranks: {rankResult.Error}", Severity.Error);
            Ranks = [];
        }
        else
            Ranks = rankResult.Value ?? [];
        
        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if(cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultResult.Value ?? [];

        var rankPrerequisiteResult = await Client!.GetFromJsonAsync<Result<List<RankPrerequisiteDto>>>("/rank-prerequisites") ?? new Result<List<RankPrerequisiteDto>> { IsError = true, Error = "Unknown error" };
        if(rankPrerequisiteResult.IsError)
        {
            Snackbar.Add($"Error loading rank prerequisites: {rankPrerequisiteResult.Error}", Severity.Error);
            RankPrerequisites = [];
        }
        else
            RankPrerequisites = rankPrerequisiteResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/ranks/{rankId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadRanks();
        StateHasChanged();
    }

    private static string GetPrerequisiteLabel(RankPrerequisiteDto prerequisite)
    {
        if (prerequisite is null)
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

    private bool FilterFunc(RankDto rank)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (rank.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
