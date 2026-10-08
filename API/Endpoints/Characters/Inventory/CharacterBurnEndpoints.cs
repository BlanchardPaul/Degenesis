using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterBurnEndpoints
{
    public static void MapCharacterBurnEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-burns").WithTags("CharacterBurns");

        group.MapGet("/{id:guid}", async (Guid id, ICharacterBurnService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterBurnService service) =>
        {
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CharacterBurnCreateDto characterBurn, ICharacterBurnService service) =>
        {
            var result = await service.CreateAsync(characterBurn);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CharacterBurnDto characterBurn, ICharacterBurnService service) =>
        {
            var result = await service.UpdateAsync(characterBurn);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterBurnService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
