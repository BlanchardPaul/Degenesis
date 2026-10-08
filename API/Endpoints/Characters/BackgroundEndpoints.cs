using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;
using System;

namespace API.Endpoints.Characters;

public static class BackgroundEndpoints
{
    public static void MapBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/backgrounds").WithTags("Backgrounds").RequireAuthorization();

        group.MapGet("/", async (IBackgroundService service) =>
        {
            var result = await service.GetAllBackgroundsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var result = await service.GetBackgroundByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (BackgroundCreateDto background, IBackgroundService service) =>
        {
            var result = await service.CreateBackgroundAsync(background);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (BackgroundDto background, IBackgroundService service) =>
        {
            var result = await service.UpdateBackgroundAsync(background);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IBackgroundService service) =>
        {
            var result = await service.DeleteBackgroundAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
