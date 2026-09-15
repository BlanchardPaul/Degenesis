using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;

public partial class ProtectionModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public ProtectionDto Protection { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    private List<Guid> SelectedCultIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        SelectedCultIds = [.. Protection.Cults.Select(c => c.Id)];
    }
    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = [.. selectedValues];
        Protection.Cults = [.. Cults.Where(c => SelectedCultIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveProtection()
    {
        if (Protection.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/protections", Protection);
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
            var result = await _client.PutAsJsonAsync("/protections", Protection);
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