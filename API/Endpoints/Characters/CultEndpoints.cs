using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class CultEndpoints
{
    public static void MapCultEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cults").WithTags("Cults").RequireAuthorization();

        group.MapGet("/", async (ICultService service) =>
        {
            var cults = await service.GetAllCultsAsync();
            return Results.Ok(cults);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICultService service) =>
        {
            var cult = await service.GetCultByIdAsync(id);
            return cult is not null ? Results.Ok(cult) : Results.NotFound();
        });

        group.MapPost("/", async (CultCreateDto cult, ICultService service) =>
        {
            var created = await service.CreateCultAsync(cult);
            return cult is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CultDto cult, ICultService service) =>
        {
            var success = await service.UpdateCultAsync(cult);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICultService service) =>
        {
            var success = await service.DeleteCultAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
