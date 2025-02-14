using Business.Vehicles;
using Business.Weapons;
using Domain.Characters;
using Domain.Vehicles;
using Domain.Weapons;

namespace API.Endpoints.Weapons;

public static class CharacterWeaponEndpoints
{
    public static void MapCharacterWeaponEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-weapons").WithTags("CharacterWeapons");

        group.MapGet("/{id}", async (ICharacterWeaponService service, Guid id) =>
        {
            var characterWeapon = await service.GetCharacterWeaponByIdAsync(id);
            return characterWeapon is not null ? Results.Ok(characterWeapon) : Results.NotFound();
        });

        group.MapGet("/character/{characterId}", async (ICharacterWeaponService service, Guid characterId) =>
        {
            var characterWeapon = await service.GetCharacterWeaponByCharacterIdAsync(characterId);
            return characterWeapon is not null ? Results.Ok(characterWeapon) : Results.NotFound();
        });

        group.MapPost("/", async (ICharacterWeaponService service, CharacterWeapon characterWeapon) =>
        {
            await service.CreateCharacterWeaponAsync(characterWeapon);
            return Results.Created($"/character-weapons/{characterWeapon.Id}", characterWeapon);
        });

        group.MapPut("/{id}", async (ICharacterWeaponService service, Guid id, CharacterWeapon characterWeapon) =>
        {
            await service.UpdateCharacterWeaponAsync(id, characterWeapon);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (ICharacterWeaponService service, Guid id) =>
        {
            await service.DeleteCharacterWeaponAsync(id);
            return Results.NoContent();
        });

    }

}
