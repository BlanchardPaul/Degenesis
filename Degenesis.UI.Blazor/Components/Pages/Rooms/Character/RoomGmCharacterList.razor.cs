using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.Character;
public partial class RoomGmCharacterList
{
    [Parameter]
    public List<CharacterDisplayForGMListDto> GmCharacters { get; set; } = [];

    private static string GetGmCharacterUrl(Guid characterId, string view)
        => $"/gm-character/{characterId}/{view}";
}