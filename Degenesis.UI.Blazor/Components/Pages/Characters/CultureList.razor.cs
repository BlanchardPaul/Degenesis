using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CultureList
{
    private List<CultureDto>? cultures;
    private List<CultDto> cults = [];
    private List<AttributeDto> attributes = [];
    private List<SkillDto> skills = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadCultures();
    }

    private async Task LoadCultures()
    {
        cultures = (await CultureService.GetCulturesAsync());
        cults = (await CultService.GetCultsAsync());
        attributes = (await AttributeService.GetAttributesAsync());
        skills = (await SkillService.GetSkillsAsync());
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "Culture", new CultureDto() },
            { "Cults", cults },
            { "Attributes", attributes },
            { "Skills", skills }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CultureModal>("Create Culture", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadCultures();
        }
    }

    private async Task ShowEditDialog(Guid cultureId)
    {
        var culture = cultures?.FirstOrDefault(c => c.Id == cultureId);
        if (culture != null)
        {
            var parameters = new DialogParameters
            {
                { "Culture", culture },
                { "Cults", cults },
                { "Attributes", attributes },
                { "Skills", skills }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<CultureModal>("Edit Culture", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadCultures();
            }
        }
    }

    private async Task DeleteCulture(Guid cultureId)
    {
        await CultureService.DeleteCultureAsync(cultureId);
        await LoadCultures();
    }
}
