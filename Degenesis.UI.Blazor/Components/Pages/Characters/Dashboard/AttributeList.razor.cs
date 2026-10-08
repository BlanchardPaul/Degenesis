using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class AttributeList
{
    private List<AttributeDto>? attributes;
    string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadAttributes();
    }

    private async Task LoadAttributes()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading attributes: " + result.Error);
            attributes = [];
        }
        else
            attributes = result.Value;
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
        var response = await Client!.DeleteAsync($"/attributes/{attributeId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadAttributes();
    }

    private bool FilterFunc(AttributeDto attribute)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (attribute.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}