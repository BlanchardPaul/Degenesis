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
            var characterEquipment = await service.GetByIdAsync(id);
            return characterEquipment is not null ? Results.Ok(characterEquipment) : Results.NotFound();
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterEquipmentService service) =>
        {
            var equipments = await service.GetByCharacterIdAsync(characterId);
            return equipments.Any() ? Results.Ok(equipments) : Results.NotFound();
        });

        group.MapPost("/", async (CharacterEquipmentCreateDto characterEquipment, ICharacterEquipmentService service) =>
        {
            var created = await service.CreateAsync(characterEquipment);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterEquipmentDto characterEquipment, ICharacterEquipmentService service) =>
        {
            var success = await service.UpdateAsync(characterEquipment);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterEquipmentService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
