using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;

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
        cults = await Client!.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        skills = await Client!.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
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
        var result = await Client!.DeleteAsync($"/cults/{cultId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
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