using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;
public static class CultureEndpoints
{
    public static void MapCultureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/cultures").WithTags("Cultures").RequireAuthorization();

        group.MapGet("/", async (ICultureService cultureService) =>
        {
            var result = await cultureService.GetAllCulturesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id}", async (Guid id, ICultureService cultureService) =>
        {
            var result = await cultureService.GetCultureByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CultureCreateDto culture, ICultureService cultureService) =>
        {
            var result = await cultureService.CreateCultureAsync(culture);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CultureDto culture, ICultureService cultureService) =>
        {
            var result = await cultureService.UpdateCultureAsync(culture);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id}", async (Guid id, ICultureService cultureService) =>
        {
            var result = await cultureService.DeleteCultureAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
