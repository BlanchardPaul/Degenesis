using Business.Protections;
using Domain.Protections;

namespace API.Endpoints.Protections;

public static class CharacterProtectionEndpoints
{
    public static void MapCharacterProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-protections").WithTags("CharacterProtections");

        group.MapGet("/", async (ICharacterProtectionService service) =>
        {
            var characterProtections = await service.GetAllCharacterProtectionsAsync();
            return Results.Ok(characterProtections);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICharacterProtectionService service) =>
        {
            var characterProtection = await service.GetCharacterProtectionByIdAsync(id);
            if (characterProtection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(characterProtection);
        });

        group.MapPost("/", async (CharacterProtection characterProtection, ICharacterProtectionService service) =>
        {
            var createdCharacterProtection = await service.CreateCharacterProtectionAsync(characterProtection);
            return Results.Created($"/character-protections/{createdCharacterProtection.Id}", createdCharacterProtection);
        });

        group.MapPut("/{id:guid}", async (Guid id, CharacterProtection characterProtection, ICharacterProtectionService service) =>
        {
            var updatedCharacterProtection = await service.UpdateCharacterProtectionAsync(id, characterProtection);
            if (updatedCharacterProtection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedCharacterProtection);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterProtectionService service) =>
        {
            var success = await service.DeleteCharacterProtectionAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
