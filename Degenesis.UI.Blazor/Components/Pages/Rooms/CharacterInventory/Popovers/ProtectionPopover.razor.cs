using Degenesis.Shared.DTOs.Protections;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Popovers;

public partial class ProtectionPopover
{
    [Parameter]
    public ProtectionDto Protection { get; set; } = null!;

    [Parameter]
    public EventCallback OnClose { get; set; }

    private async Task Close()
    {
        await OnClose.InvokeAsync();
    }
}
