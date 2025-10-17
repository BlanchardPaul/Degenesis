using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomCharacterSheet
{
    [Parameter] public CharacterDisplayDto? Character { get; set; }

}
