using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialPrerequisiteList
{
    private List<PotentialPrerequisiteDto>? PotentialPrerequisites;
    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];
    private List<BackgroundDto> Backgrounds = [];
    private List<RankDto> Ranks = [];
    private List<CultDto> Cults = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadPotentialPrerequisites();
    }

    private async Task LoadPotentialPrerequisites()
    {
        PotentialPrerequisites = await _client.GetFromJsonAsync<List<PotentialPrerequisiteDto>>("/potential-prerequisites") ?? [];
        Attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        Skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
        Backgrounds = await _client.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? [];
        Ranks = await _client.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "PotentialPrerequisite", new PotentialPrerequisiteDto() },
            { "Attributes", Attributes },
            { "Skills", Skills },
            { "Backgrounds", Backgrounds },
            { "Ranks", Ranks }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<PotentialPrerequisiteModal>("Create Potential Prerequisite", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadPotentialPrerequisites();
        }
    }

    private async Task ShowEditDialog(Guid potentialPrerequisiteId)
    {
        var prerequisite = PotentialPrerequisites?.FirstOrDefault(pp => pp.Id == potentialPrerequisiteId);
        if (prerequisite != null)
        {
            var parameters = new DialogParameters
            {
                { "PotentialPrerequisite", prerequisite },
                { "Attributes", Attributes },
                { "Skills", Skills },
                { "Backgrounds", Backgrounds },
                { "Ranks", Ranks }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<PotentialPrerequisiteModal>("Edit Potential Prerequisite", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadPotentialPrerequisites();
            }
        }
    }

    private async Task DeletePotentialPrerequisite(Guid potentialPrerequisiteId)
    {
        var result = await _client.DeleteAsync($"/potential-prerequisites/{potentialPrerequisiteId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadPotentialPrerequisites();
    }
}
