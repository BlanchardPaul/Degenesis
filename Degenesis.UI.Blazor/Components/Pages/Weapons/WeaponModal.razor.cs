using Degenesis.Shared.DTOs.Characters;
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
    [Parameter] public List<WeaponQualityDto> WeaponQualities { get; set; } = [];

    private HashSet<Guid> SelectedQualityIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        if (Weapon.WeaponTypeId == Guid.Empty && WeaponTypes.Count > 0)
        {
            Weapon.WeaponTypeId = WeaponTypes[0].Id;
        }

        SelectedQualityIds = new HashSet<Guid>(Weapon.Qualities.Select(q => q.Id));
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = new HashSet<Guid>(selectedValues);
        Weapon.Qualities = WeaponQualities.Where(q => SelectedQualityIds.Contains(q.Id)).ToList();
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