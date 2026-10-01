using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;
using System;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultureList
{
    private List<CultureDto>? cultures;
    private List<CultDto> cults = [];
    private List<AttributeDto> attributes = [];
    private List<SkillDto> skills = [];
    private string SearchString = "";
    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadCultures();
    }

    private async Task LoadCultures()
    {
        cultures = await Client!.GetFromJsonAsync<List<CultureDto>>("/cultures") ?? [];
        cults = await Client!.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        attributes = await Client!.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        skills = await Client!.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
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
        var result = await Client!.DeleteAsync($"/cultures/{cultureId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadCultures();
    }

    private bool FilterFunc(CultureDto culture)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (culture.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
