using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.NPCs;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public partial class RoomMembersDrawer
{
    [Parameter] public List<CharacterDisplayForGMListDto> GmCharacters { get; set; } = [];
    [Parameter] public List<NPCDto> GmNpcs { get; set; } = [];
    [Parameter] public EventCallback<RoomMapToken> OnTokenDragStart { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    private string _avatarVersion = Guid.NewGuid().ToString("N");
    private List<CharacterDisplayForGMListDto>? _avatarCharacters;
    private List<NPCDto>? _avatarNpcs;
    private string ApiRoot => HttpClientFactory.CreateClient("API").BaseAddress is not { } baseAddress
        ? string.Empty
        : new Uri(baseAddress, "..").GetLeftPart(UriPartial.Authority);

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

    private Task StartDraggingCharacter(CharacterDisplayForGMListDto character)
        => OnTokenDragStart.InvokeAsync(new RoomMapToken
        {
            MemberId = character.Id,
            Name = character.Name,
            AvatarName = character.AvatarName
        });

    private Task StartDraggingNpc(NPCDto npc)
        => OnTokenDragStart.InvokeAsync(new RoomMapToken
        {
            MemberId = npc.Id,
            IsNpc = true,
            Name = npc.Name,
            AvatarName = npc.AvatarName
        });

    private static string GetInitials(string name)
        => string.IsNullOrWhiteSpace(name) ? "?" : name[..1].ToUpperInvariant();
}
