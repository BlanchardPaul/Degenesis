using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialList
{
    private List<PotentialDto>? Potentials;
    private List<CultDto> Cults = [];
    private List<PotentialPrerequisiteDto> PotentialPrerequisites = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadPotentials();
    }

    private async Task LoadPotentials()
    {
        var potentialResult = await Client!.GetFromJsonAsync<Result<List<PotentialDto>>>("/potentials") ?? new Result<List<PotentialDto>> { IsError = true, Error = "Unknown error" };
        if(potentialResult.IsError)
        {
            Snackbar.Add($"Error loading potentials: {potentialResult.Error}", Severity.Error);
            Potentials = [];
        }else
            Potentials = potentialResult.Value ?? [];

        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultResult.Value ?? [];

        var potentialPrerequisiteResult = await Client!.GetFromJsonAsync<Result<List<PotentialPrerequisiteDto>>>("/potential-prerequisites") ?? new Result<List<PotentialPrerequisiteDto>> { IsError = true, Error = "Unknown error" };
        if(potentialPrerequisiteResult.IsError)
        {
            Snackbar.Add($"Error loading potential prerequisites: {potentialPrerequisiteResult.Error}", Severity.Error);
            PotentialPrerequisites = [];
        }
        else
            PotentialPrerequisites = potentialPrerequisiteResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/potentials/{potentialId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadPotentials();
        StateHasChanged();
    }

    private static string GetPrerequisiteLabel(PotentialPrerequisiteDto prerequisite)
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
        string rankPart = prerequisite.RankRequired != null ? $" RANK: {prerequisite.RankRequired.Name}" : "";

        return $"{attributePart}{skillPart}{sumPart}{rankPart}".Trim();
    }

    private bool FilterFunc(PotentialDto potential)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (potential.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}