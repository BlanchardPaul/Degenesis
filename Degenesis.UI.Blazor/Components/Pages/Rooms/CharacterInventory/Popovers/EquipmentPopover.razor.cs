using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Popovers;

public partial class EquipmentPopover
{
    [Parameter]
    public EquipmentDto Equipment { get; set; } = null!;

    [Parameter]
    public EventCallback OnClose { get; set; }

    private async Task Close()
    {
        await OnClose.InvokeAsync();
    }
}