using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultList
{
    private List<CultDto>? cults;
    private List<SkillDto> skills = [];
    private string SearchString = "";
    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadCults();
    }

    private async Task LoadCults()
    {
        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add("Error loading cults: " + cultResult.Error);
            cults = [];
        }
        else
            cults = cultResult.Value ?? [];
        
        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillResult.IsError)
        {
            Snackbar.Add("Error loading skills: " + skillResult.Error);
            skills = [];
        }
        else
            skills = skillResult.Value ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Cult", new CultDto() }, { "Skills", skills } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CultModal>("Create Cult", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadCults();
        }
    }

    private async Task ShowEditDialog(Guid cultId)
    {
        var cult = cults?.FirstOrDefault(c => c.Id == cultId);
        if (cult != null)
        {
            var parameters = new DialogParameters { { "Cult", cult }, { "Skills", skills } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<CultModal>("Edit Cult", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadCults();
            }
        }
    }

    private async Task DeleteCult(Guid cultId)
    {
        var response = await Client!.DeleteAsync($"/cults/{cultId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
        await LoadCults();
    }

    private bool FilterFunc(CultDto cult)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (cult.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}