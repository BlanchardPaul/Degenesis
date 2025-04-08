using Business.Characters;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class BackgroundEndpoints
{
    public static void MapBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/backgrounds").WithTags("Backgrounds").RequireAuthorization();

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
        group.MapPost("/", async (BackgroundCreateDto background, IBackgroundService service) =>
        {
            var created = await service.CreateBackgroundAsync(background);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        // PUT /backgrounds/{id}
        group.MapPut("/", async (Background background, IBackgroundService service) =>
        {
            var success = await service.UpdateBackgroundAsync(background);
            return success ? Results.Ok() : Results.NotFound();
        });

        // DELETE /backgrounds/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var success = await service.DeleteBackgroundAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
