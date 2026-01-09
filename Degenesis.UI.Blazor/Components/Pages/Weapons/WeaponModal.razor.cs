using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponDto Weapon { get; set; } = new();
    [Parameter] public List<WeaponTypeDto> WeaponTypes { get; set; } = [];
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public List<WeaponQualityDto> WeaponQualities { get; set; } = [];
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    private List<Guid> SelectedCultIds { get; set; } = [];
    private List<Guid> SelectedQualityIds { get; set; } = [];

    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        SelectedCultIds = [.. Weapon.Cults.Select(c => c.Id)];
        SelectedQualityIds = [.. Weapon.Qualities.Select(c => c.Id)];
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        if (Weapon.WeaponTypeId == Guid.Empty && WeaponTypes.Count > 0)
        {
            Weapon.WeaponTypeId = WeaponTypes[0].Id;
        }

        SelectedQualityIds = [.. Weapon.Qualities.Select(q => q.Id)];
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = [.. selectedValues];
        Weapon.Qualities = [.. WeaponQualities.Where(c => SelectedQualityIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = [.. selectedValues];
        Weapon.Cults = [.. Cults.Where(c => SelectedCultIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveWeapon()
    {
        if (Weapon.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/weapons", Weapon);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync("/weapons", Weapon);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}