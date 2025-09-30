using Degenesis.Shared.DTOs.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;

public partial class ProtectionModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public ProtectionDto Protection { get; set; } = new();
    [Parameter] public List<ProtectionQualityDto> ProtectionQualities { get; set; } = new();

    private List<Guid> SelectedQualityIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        Protection.Qualities ??= [];
        SelectedQualityIds = [.. Protection.Qualities.Select(bs => bs.Id)];
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = [.. selectedValues];
        Protection.Qualities = [.. ProtectionQualities.Where(s => SelectedQualityIds.Contains(s.Id))]; ;
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