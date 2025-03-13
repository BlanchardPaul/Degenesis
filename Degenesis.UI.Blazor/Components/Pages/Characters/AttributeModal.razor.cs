using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class AttributeModal
{
    [Inject] private AttributeService AttributeService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public AttributeDto Attribute { get; set; } = new();

    private async Task SaveAttribute()
    {
        if (Attribute.Id == Guid.Empty)
            await AttributeService.CreateAttributeAsync(Attribute);
        else
            await AttributeService.UpdateAttributeAsync(Attribute);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

}