using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

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
        var cultureResult = await Client!.GetFromJsonAsync<Result<List<CultureDto>>>("/cultures") ?? new Result<List<CultureDto>> { IsError = true, Error = "Unknown error" };
        if (cultureResult.IsError)
        {
            Snackbar.Add($"Error loading cultures: {cultureResult.Error}", Severity.Error);
            cultures = [];
        }
        else
            cultures = cultureResult.Value ?? [];

        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            cults = [];
        }
        else
            cults = cultResult.Value ?? [];

        var attributeResult = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if (attributeResult.IsError)
        {
            Snackbar.Add($"Error loading attributes: {attributeResult.Error}", Severity.Error);
            attributes = [];
        }
        else
            attributes = attributeResult.Value ?? [];

        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillResult.IsError)
        {
            Snackbar.Add($"Error loading skills: {skillResult.Error}", Severity.Error);
            skills = [];
        }
        else
            skills = skillResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/cultures/{cultureId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
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
