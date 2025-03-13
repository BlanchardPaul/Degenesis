using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankList
{
    private List<RankDto>? ranks;
    private List<CultDto> cults = new();
    private List<RankPrerequisiteDto> rankPrerequisites = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadRanks();
    }

    private async Task LoadRanks()
    {
        ranks = [.. (await RankService.GetRanksAsync())];
        cults = [.. (await CultService.GetCultsAsync())];
        rankPrerequisites = [.. (await RankPrerequisiteService.GetRankPrerequisitesAsync())];
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
        await RankService.DeleteRankAsync(rankId);
        await LoadRanks();
    }

    private static string GetPrerequisiteLabel(RankPrerequisiteDto prerequisite)
    {
        if (prerequisite == null) return "Unknown Prerequisite";

        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        return $"{prerequisite.AttributeRequired.Name}{skillPart} = {prerequisite.SumRequired}";
    }
}
