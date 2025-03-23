using Business.Vehicles;
using Domain.Vehicles;

namespace API.Endpoints.Vehicles;

public static class CharacterVehicleEndpoints
{
    public static void MapCharacterVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-vehicles").WithTags("CharacterVehicles");

        group.MapGet("/", async (ICharacterVehicleService service) =>
        {
            var characterVehicles = await service.GetAllCharacterVehiclesAsync();
            return Results.Ok(characterVehicles);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICharacterVehicleService service) =>
        {
            var characterVehicle = await service.GetCharacterVehicleByIdAsync(id);
            if (characterVehicle is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(characterVehicle);
        });

        group.MapPost("/", async (CharacterVehicle characterVehicle, ICharacterVehicleService service) =>
        {
            var createdCharacterVehicle = await service.CreateCharacterVehicleAsync(characterVehicle);
            return Results.Created($"/character-vehicles/{createdCharacterVehicle.Id}", createdCharacterVehicle);
        });

        group.MapPut("/{id:guid}", async (Guid id, CharacterVehicle characterVehicle, ICharacterVehicleService service) =>
        {
            var updatedCharacterVehicle = await service.UpdateCharacterVehicleAsync(id, characterVehicle);
            if (updatedCharacterVehicle is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedCharacterVehicle);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterVehicleService service) =>
        {
            var success = await service.DeleteCharacterVehicleAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
