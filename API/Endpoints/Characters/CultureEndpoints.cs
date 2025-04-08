using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;
public static class CultureEndpoints
{
    public static void MapCultureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/cultures").WithTags("Cultures").RequireAuthorization();

        group.MapGet("/", async (ICultureService cultureService) =>
        {
            var cultures = await cultureService.GetAllCulturesAsync();
            return Results.Ok(cultures);
        });

        group.MapGet("/{id}", async (Guid id, ICultureService cultureService) =>
        {
            var culture = await cultureService.GetCultureByIdAsync(id);
            return culture is not null ? Results.Ok(culture) : Results.NotFound();
        });

        group.MapPost("/", async (CultureCreateDto culture, ICultureService cultureService) =>
        {
            var created = await cultureService.CreateCultureAsync(culture);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (CultureDto culture, ICultureService cultureService) =>
        {
            var success = await cultureService.UpdateCultureAsync(culture);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (Guid id, ICultureService cultureService) =>
        {
            var success = await cultureService.DeleteCultureAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
