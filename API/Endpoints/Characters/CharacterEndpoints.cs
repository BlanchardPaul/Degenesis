using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;
using System.Security.Claims;

namespace API.Endpoints.Characters;

public static class CharacterEndpoints
{
    public static void MapCharacterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/characters").WithTags("Characters");

        group.MapGet("/{roomId:guid}", async (Guid roomId, ICharacterService service, ClaimsPrincipal user) =>
        {
            var character = await service.GetCharacterByUserAndRoomAsync(roomId, user?.Identity?.Name ?? string.Empty);
            return character is not null ? Results.Ok(character) : Results.NotFound();
        });

        group.MapGet("/", async (ICharacterService service) =>
        {
            var characters = await service.GetAllCharactersAsync();
            return characters is not null ? Results.Ok(characters) : Results.NotFound();
        });

        group.MapPost("/", async (CharacterCreateDto character, ICharacterService service, ClaimsPrincipal user) =>
        {
            var created = await service.CreateCharacterAsync(character, user?.Identity?.Name ?? string.Empty);
            return character is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/basic-infos", async (CharacterBasicInfosEditDto characterBasicInfos, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterBasicInfosAsync(characterBasicInfos);
            return success ? Results.Ok() : Results.BadRequest();
        });
        
        group.MapPut("/chroniclermoney", async (CharacterIntValueEditDto characterChroniclerMoney, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterChroniclerMoneyAsync(characterChroniclerMoney);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/current-spore-infestation", async (CharacterIntValueEditDto characterCurrentSporeInfestation, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterCurrentSporeInfestationAsync(characterCurrentSporeInfestation);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/dinar", async (CharacterIntValueEditDto characterDinar, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterDinarAsync(characterDinar);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/ego", async (CharacterIntValueEditDto characterEgo, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterEgoAsync(characterEgo);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/fleshwounds", async (CharacterIntValueEditDto characterFleshWounds, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterFleshWoundsAsync(characterFleshWounds);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/permanent-spore-infestation", async (CharacterIntValueEditDto characterPermanentSporeInfestation, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterPermanentSporeInfestationAsync(characterPermanentSporeInfestation);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/rank", async (CharacterGuidValueEditDto characterRank, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterRankAsync(characterRank);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/trauma", async (CharacterIntValueEditDto characterTrauma, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterTraumaAsync(characterTrauma);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/xp", async (CharacterIntValueEditDto characterXp, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterXpAsync(characterXp);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterService service) =>
        {
            var success = await service.DeleteCharacterAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
