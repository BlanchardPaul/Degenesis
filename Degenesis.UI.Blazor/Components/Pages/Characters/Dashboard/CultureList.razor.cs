using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultureList
{
    private List<CultureDto>? cultures;
    private List<CultDto> cults = [];
    private List<AttributeDto> attributes = [];
    private List<SkillDto> skills = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadCultures();
    }

    private async Task LoadCultures()
    {
        cultures = await _client.GetFromJsonAsync<List<CultureDto>>("/cultures") ?? [];
        cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
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
        var result = await _client.DeleteAsync($"/cultures/{cultureId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadCultures();
    }
}
