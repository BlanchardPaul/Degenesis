using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class BackgroundEndpoints
{
    public static void MapBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/backgrounds").WithTags("Backgrounds");

        // GET /backgrounds/{id}
        group.MapGet("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var background = await service.GetBackgroundByIdAsync(id);
            return background is not null ? Results.Ok(background) : Results.NotFound();
        });

        // GET /backgrounds
        group.MapGet("/", async (IBackgroundService service) =>
        {
            var backgrounds = await service.GetAllBackgroundsAsync();
            return Results.Ok(backgrounds);
        });

        // POST /backgrounds
        group.MapPost("/", async (Background background, IBackgroundService service) =>
        {
            var created = await service.CreateBackgroundAsync(background);
            return Results.Created($"/backgrounds/{created.Id}", created);
        });

        // PUT /backgrounds/{id}
        group.MapPut("/{id:guid}", async (Guid id, Background background, IBackgroundService service) =>
        {
            var success = await service.UpdateBackgroundAsync(id, background);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE /backgrounds/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var success = await service.DeleteBackgroundAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
