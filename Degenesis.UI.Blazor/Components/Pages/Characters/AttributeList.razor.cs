using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class AttributeList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<AttributeDto>? attributes;

    protected override async Task OnInitializedAsync()
    {
        await LoadAttributes();
    }

    private async Task LoadAttributes()
    {
        attributes = await AttributeService.GetAttributesAsync();
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
        await AttributeService.DeleteAttributeAsync(attributeId);
        attributes = await AttributeService.GetAttributesAsync();
    }

}