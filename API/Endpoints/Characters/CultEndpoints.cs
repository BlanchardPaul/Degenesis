using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CultEndpoints
{
    public static void MapCultEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cults").WithTags("Cults");

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

        group.MapPost("/", async (Cult cult, ICultService service) =>
        {
            var createdCult = await service.CreateCultAsync(cult);
            return Results.Created($"/cults/{createdCult.Id}", createdCult);
        });

        group.MapPut("/{id:guid}", async (Guid id, Cult cult, ICultService service) =>
        {
            var success = await service.UpdateCultAsync(id, cult);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICultService service) =>
        {
            var success = await service.DeleteCultAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
