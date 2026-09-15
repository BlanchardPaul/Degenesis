using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

public static class CharacterBurnEndpoints
{
    public static void MapCharacterBurnEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-burns").WithTags("CharacterBurns");

        group.MapGet("/{id:guid}", async (Guid id, ICharacterBurnService service) =>
        {
            var characterBurn = await service.GetByIdAsync(id);
            return characterBurn is not null ? Results.Ok(characterBurn) : Results.NotFound();
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterBurnService service) =>
        {
            var burns = await service.GetByCharacterIdAsync(characterId);
            return burns.Any() ? Results.Ok(burns) : Results.NotFound();
        });

        group.MapPost("/", async (CharacterBurnCreateDto characterBurn, ICharacterBurnService service) =>
        {
            var created = await service.CreateAsync(characterBurn);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterBurnDto characterBurn, ICharacterBurnService service) =>
        {
            var success = await service.UpdateAsync(characterBurn);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterBurnService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
