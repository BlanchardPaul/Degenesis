using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class AttributeList
{
    private List<AttributeDto>? attributes;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadAttributes();
    }

    private async Task LoadAttributes()
    {
        attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Attribute", new AttributeDto() } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<AttributeModal>("Create Attribute", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadAttributes();
        }
    }

    private async Task ShowEditDialog(Guid attributeId)
    {
        var attribute = attributes?.FirstOrDefault(a => a.Id == attributeId);
        if (attribute != null)
        {
            var parameters = new DialogParameters { { "Attribute", attribute } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<AttributeModal>("Edit Attribute", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadAttributes();
            }
        }
    }

    private async Task DeleteAttribute(Guid attributeId)
    {
        var result = await _client.DeleteAsync($"/attributes/{attributeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadAttributes();
    }

}