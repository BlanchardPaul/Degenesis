using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class BackgroundEndpoints
{
    public static void MapBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/backgrounds").WithTags("Backgrounds").RequireAuthorization();

        group.MapGet("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var background = await service.GetBackgroundByIdAsync(id);
            return background is not null ? Results.Ok(background) : Results.NotFound();
        });

        group.MapGet("/", async (IBackgroundService service) =>
        {
            var backgrounds = await service.GetAllBackgroundsAsync();
            return Results.Ok(backgrounds);
        });

        group.MapPost("/", async (BackgroundCreateDto background, IBackgroundService service) =>
        {
            var created = await service.CreateBackgroundAsync(background);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (BackgroundDto background, IBackgroundService service) =>
        {
            var success = await service.UpdateBackgroundAsync(background);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var success = await service.DeleteBackgroundAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
