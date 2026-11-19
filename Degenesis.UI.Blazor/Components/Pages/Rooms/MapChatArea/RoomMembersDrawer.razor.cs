using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.NPCs;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public partial class RoomMembersDrawer
{
    [Parameter] public List<CharacterDisplayForGMListDto> GmCharacters { get; set; } = [];
    [Parameter] public List<NPCDto> GmNpcs { get; set; } = [];
    [Parameter] public EventCallback OnClose { get; set; }

    private string _avatarVersion = Guid.NewGuid().ToString("N");
    private List<CharacterDisplayForGMListDto>? _avatarCharacters;
    private List<NPCDto>? _avatarNpcs;
    private string ApiRoot => Client?.BaseAddress is null
        ? string.Empty
        : new Uri(Client.BaseAddress, "..").GetLeftPart(UriPartial.Authority);

    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(GmCharacters, _avatarCharacters) || !ReferenceEquals(GmNpcs, _avatarNpcs))
        {
            _avatarCharacters = GmCharacters;
            _avatarNpcs = GmNpcs;
            _avatarVersion = Guid.NewGuid().ToString("N");
        }
    }

    private string GetCharacterAvatarUrl(CharacterDisplayForGMListDto character)
        => $"{ApiRoot}/character-avatars/{Uri.EscapeDataString(character.AvatarName!)}?v={_avatarVersion}";

    private string GetNpcAvatarUrl(NPCDto npc)
        => $"{ApiRoot}/npc-avatars/{Uri.EscapeDataString(npc.AvatarName!)}?v={_avatarVersion}";

    private static string GetInitials(string name)
        => string.IsNullOrWhiteSpace(name) ? "?" : name[..1].ToUpperInvariant();
}
