//using Business.Equipments;
//using Domain.Equipments;

//namespace API.Endpoints.Equipments;

//public static class CharacterEquipmentEndpoints
//{
//    public static void MapCharacterEquipmentEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/characterequipments").WithTags("CharacterEquipments");

//        group.MapGet("/", async (ICharacterEquipmentService service) =>
//        {
//            var characterEquipments = await service.GetAllCharacterEquipmentsAsync();
//            return Results.Ok(characterEquipments);
//        });

//        group.MapGet("/{characterId:guid}/{equipmentId:guid}", async (Guid characterId, Guid equipmentId, ICharacterEquipmentService service) =>
//        {
//            var characterEquipment = await service.GetCharacterEquipmentByIdAsync(characterId, equipmentId);
//            if (characterEquipment is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(characterEquipment);
//        });

//        group.MapPost("/", async (CharacterEquipment characterEquipment, ICharacterEquipmentService service) =>
//        {
//            var createdCharacterEquipment = await service.CreateCharacterEquipmentAsync(characterEquipment);
//            return Results.Created($"/characterequipments/{createdCharacterEquipment.CharacterId}/{createdCharacterEquipment.EquipmentId}", createdCharacterEquipment);
//        });

//        group.MapPut("/{characterId:guid}/{equipmentId:guid}", async (Guid characterId, Guid equipmentId, CharacterEquipment characterEquipment, ICharacterEquipmentService service) =>
//        {
//            var updatedCharacterEquipment = await service.UpdateCharacterEquipmentAsync(characterId, equipmentId, characterEquipment);
//            if (updatedCharacterEquipment is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(updatedCharacterEquipment);
//        });

//        group.MapDelete("/{characterId:guid}/{equipmentId:guid}", async (Guid characterId, Guid equipmentId, ICharacterEquipmentService service) =>
//        {
//            var success = await service.DeleteCharacterEquipmentAsync(characterId, equipmentId);
//            if (!success)
//            {
//                return Results.NotFound();
//            }
//            return Results.NoContent();
//        });
//    }
//}
