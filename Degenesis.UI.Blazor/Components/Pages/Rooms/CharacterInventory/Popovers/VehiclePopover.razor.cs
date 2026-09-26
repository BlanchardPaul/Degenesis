using Degenesis.Shared.DTOs.Vehicles;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Popovers;

public partial class VehiclePopover
{
    [Parameter]
    public VehicleDto Vehicle { get; set; } = null!;

    [Parameter]
    public EventCallback OnClose { get; set; }

    private async Task Close()
    {
        await OnClose.InvokeAsync();
    }
}
