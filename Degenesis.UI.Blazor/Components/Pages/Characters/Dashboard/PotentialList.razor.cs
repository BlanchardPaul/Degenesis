using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialList
{
    private List<PotentialDto>? Potentials;
    private List<CultDto> Cults = [];
    private List<PotentialPrerequisiteDto> PotentialPrerequisites = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadPotentials();
    }

    private async Task LoadPotentials()
    {
        Potentials = await _client.GetFromJsonAsync<List<PotentialDto>>("/potentials") ?? [];
        Cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        PotentialPrerequisites = await _client.GetFromJsonAsync<List<PotentialPrerequisiteDto>>("/potential-prerequisites") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "Potential", new PotentialDto() },
            { "Cults", Cults },
            { "PotentialPrerequisites", PotentialPrerequisites }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<PotentialModal>("Create Potential", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadPotentials();
            StateHasChanged();
        }
    }

    private async Task ShowEditDialog(Guid potentialId)
    {
        var potential = Potentials?.FirstOrDefault(p => p.Id == potentialId);
        if (potential != null)
        {
            var parameters = new DialogParameters
            {
                { "Potential", potential },
                { "Cults", Cults },
                { "PotentialPrerequisites", PotentialPrerequisites }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<PotentialModal>("Edit Potential", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadPotentials();
                StateHasChanged();
            }
        }
    }

    private async Task DeletePotential(Guid potentialId)
    {
        var result = await _client.DeleteAsync($"/potentials/{potentialId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadPotentials();
        StateHasChanged();
    }

    private static string GetPrerequisiteLabel(PotentialPrerequisiteDto prerequisite)
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
        string rankPart = prerequisite.RankRequired != null ? $" RANK: {prerequisite.RankRequired.Name}" : "";

        return $"{attributePart}{skillPart}{sumPart}{rankPart}".Trim();
    }
}