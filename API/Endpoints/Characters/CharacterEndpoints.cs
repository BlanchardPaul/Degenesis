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
            var result = await service.GetCharacterByUserAndRoomAsync(roomId, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CharacterCreateDto character, ICharacterService service, ClaimsPrincipal user) =>
        {
            var result = await service.CreateCharacterAsync(character, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/basic-infos", async (CharacterBasicInfosEditDto characterBasicInfos, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterBasicInfosAsync(characterBasicInfos);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
        
        group.MapPut("/chroniclermoney", async (CharacterIntValueEditDto characterChroniclerMoney, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterChroniclerMoneyAsync(characterChroniclerMoney);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/current-spore-infestation", async (CharacterIntValueEditDto characterCurrentSporeInfestation, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterCurrentSporeInfestationAsync(characterCurrentSporeInfestation);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/dinar", async (CharacterIntValueEditDto characterDinar, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterDinarAsync(characterDinar);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/ego", async (CharacterIntValueEditDto characterEgo, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterEgoAsync(characterEgo);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/fleshwounds", async (CharacterIntValueEditDto characterFleshWounds, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterFleshWoundsAsync(characterFleshWounds);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/notes", async (CharacterStringValueEditDto characterNotes, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterNotesAsync(characterNotes);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
        group.MapPut("/inventory-notes", async (CharacterStringValueEditDto characterInventoryNotes, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterInventoryNotesAsync(characterInventoryNotes);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });


        group.MapPut("/permanent-spore-infestation", async (CharacterIntValueEditDto characterPermanentSporeInfestation, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterPermanentSporeInfestationAsync(characterPermanentSporeInfestation);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/rank", async (CharacterGuidValueEditDto characterRank, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterRankAsync(characterRank);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/trauma", async (CharacterIntValueEditDto characterTrauma, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterTraumaAsync(characterTrauma);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/xp", async (CharacterIntValueEditDto characterXp, ICharacterService service) =>
        {
            var result = await service.UpdateCharacterXpAsync(characterXp);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{roomId:guid}", async (Guid roomId, ICharacterService service, ClaimsPrincipal user) =>
        {
            var result = await service.DeleteCharacterAsync(roomId, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
