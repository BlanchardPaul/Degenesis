using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankPrerequisiteList
{
    private List<RankPrerequisiteDto>? rankPrerequisites;
    private List<AttributeDto> attributes = new();
    private List<SkillDto> skills = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadRankPrerequisites();
    }

    private async Task LoadRankPrerequisites()
    {
        rankPrerequisites = (await RankPrerequisiteService.GetRankPrerequisitesAsync()).ToList();
        attributes = (await AttributeService.GetAttributesAsync()).ToList();
        skills = (await SkillService.GetSkillsAsync()).ToList();
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
        await RankPrerequisiteService.DeleteRankPrerequisiteAsync(rankPrerequisiteId);
        await LoadRankPrerequisites();
    }
}
