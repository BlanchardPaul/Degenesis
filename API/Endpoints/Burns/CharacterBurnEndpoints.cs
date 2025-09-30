//using Business.Burns;
//using Domain.Burns;

//namespace API.Endpoints.Burns;

//public static class CharacterBurnEndpoints
//{
//    public static void MapCharacterBurnEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/characterburns").WithTags("CharacterBurns");

//        group.MapGet("/", async (ICharacterBurnService service) =>
//        {
//            var characterBurns = await service.GetAllCharacterBurnsAsync();
//            return Results.Ok(characterBurns);
//        });

//        group.MapGet("/{characterId:guid}/{burnId:guid}", async (Guid characterId, Guid burnId, ICharacterBurnService service) =>
//        {
//            var characterBurn = await service.GetCharacterBurnByIdAsync(characterId, burnId);
//            if (characterBurn is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(characterBurn);
//        });

//        group.MapPost("/", async (CharacterBurn characterBurn, ICharacterBurnService service) =>
//        {
//            var createdCharacterBurn = await service.CreateCharacterBurnAsync(characterBurn);
//            return Results.Created($"/characterburns/{createdCharacterBurn.CharacterId}/{createdCharacterBurn.BurnId}", createdCharacterBurn);
//        });

//        group.MapPut("/{characterId:guid}/{burnId:guid}", async (Guid characterId, Guid burnId, CharacterBurn characterBurn, ICharacterBurnService service) =>
//        {
//            var updatedCharacterBurn = await service.UpdateCharacterBurnAsync(characterId, burnId, characterBurn);
//            if (updatedCharacterBurn is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(updatedCharacterBurn);
//        });

//        group.MapDelete("/{characterId:guid}/{burnId:guid}", async (Guid characterId, Guid burnId, ICharacterBurnService service) =>
//        {
//            var success = await service.DeleteCharacterBurnAsync(characterId, burnId);
//            if (!success)
//            {
//                return Results.NotFound();
//            }
//            return Results.NoContent();
//        });
//    }
//}
