using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomPage
{
    [Parameter] public Guid IdRoom { get; set; }
}
