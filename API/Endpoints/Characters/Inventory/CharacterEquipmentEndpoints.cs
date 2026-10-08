using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterEquipmentEndpoints
{
    public static void MapCharacterEquipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-equipments").WithTags("CharacterEquipments");

        group.MapGet("/{id:guid}", async (Guid id, ICharacterEquipmentService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterEquipmentService service) =>
        {
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CharacterEquipmentCreateDto characterEquipment, ICharacterEquipmentService service) =>
        {
            var result = await service.CreateAsync(characterEquipment);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CharacterEquipmentDto characterEquipment, ICharacterEquipmentService service) =>
        {
            var result = await service.UpdateAsync(characterEquipment);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterEquipmentService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
