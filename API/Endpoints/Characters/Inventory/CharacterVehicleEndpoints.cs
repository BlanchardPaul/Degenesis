using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterVehicleEndpoints
{
    public static void MapCharacterVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-vehicles").WithTags("CharacterVehicles");
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterVehicleService service) =>
        {
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });
        group.MapPost("/", async (CharacterVehicleCreateDto characterVehicleCreate, ICharacterVehicleService service) =>
        {
            var result = await service.CreateAsync(characterVehicleCreate);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });
        group.MapPut("/", async (CharacterVehicleDto characterVehicleDto, ICharacterVehicleService service) =>
        {
            var result = await service.UpdateAsync(characterVehicleDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterVehicleService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}