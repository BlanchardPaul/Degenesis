using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;
public static class CultureEndpoints
{
    public static void MapCultureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/cultures").WithTags("Cultures");

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

        group.MapPost("/", async (Culture culture, ICultureService cultureService) =>
        {
            var createdCulture = await cultureService.CreateCultureAsync(culture);
            return createdCulture is not null
                ? Results.Created($"/cultures/{createdCulture.Id}", createdCulture)
                : Results.BadRequest();
        });

        group.MapPut("/{id}", async (Guid id, Culture culture, ICultureService cultureService) =>
        {
            var success = await cultureService.UpdateCultureAsync(id, culture);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (Guid id, ICultureService cultureService) =>
        {
            var success = await cultureService.DeleteCultureAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
