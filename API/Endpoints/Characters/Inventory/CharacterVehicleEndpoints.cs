using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterVehicleEndpoints
{
    public static void MapCharacterVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-vehicles").WithTags("CharacterVehicles");
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterVehicleService service) =>
        {
            var characterVehicles = await service.GetByCharacterIdAsync(characterId);
            return Results.Ok(characterVehicles);
        });
        group.MapPost("/", async (CharacterVehicleCreateDto characterVehicleCreate, ICharacterVehicleService service) =>
        {
            var createdCharacterVehicle = await service.CreateAsync(characterVehicleCreate);
            if (createdCharacterVehicle is null)
            {
                return Results.BadRequest("Failed to create CharacterVehicle.");
            }
            return Results.Created($"/character-vehicles/{createdCharacterVehicle.Id}", createdCharacterVehicle);
        });
        group.MapPut("/", async (CharacterVehicleDto characterVehicleDto, ICharacterVehicleService service) =>
        {
            var success = await service.UpdateAsync(characterVehicleDto);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterVehicleService service) =>
        {
            var success = await service.DeleteAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}