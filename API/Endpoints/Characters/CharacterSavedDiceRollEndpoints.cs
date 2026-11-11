using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;
public static class CharacterSavedDiceRollEndpoints
{
    public static void MapCharacterSavedDiceRollEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-saved-dice-rolls").WithTags("Character Saved Dice Rolls").RequireAuthorization();

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterSavedDiceRollService service) =>
        {
            var result = await service.GetCharacterSavedDiceRollByIdCharacterAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CharacterSavedDiceRollDto dto, ICharacterSavedDiceRollService service) =>
        {
            var result = await service.CreateCharacterSavedDiceRollAsync(dto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/", async (CharacterSavedDiceRollDto dto, ICharacterSavedDiceRollService service) =>
        {
            var result = await service.UpdateCharacterSavedDiceRollAsync(dto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterSavedDiceRollService service) =>
        {
            var result = await service.DeleteCharacterSavedDiceRollAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}