using Business.Burns;
using Degenesis.Shared.DTOs.Burns;
using Domain.Burns;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Burns;

public static class BurnEndpoints
{
    public static void MapBurnEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/burns").WithTags("Burns").RequireAuthorization();

        group.MapGet("/", async (IBurnService service) =>
        {
            var burns = await service.GetAllAsync();
            return Results.Ok(burns);
        });

        group.MapGet("/{id:guid}", async (Guid id, IBurnService service) =>
        {
            var burn = await service.GetByIdAsync(id);
            if (burn is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(burn);
        });

        group.MapPost("/", async (BurnCreateDto burn, IBurnService service) =>
        {
            var createdBurn = await service.CreateAsync(burn);
            if (createdBurn is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (Burn burn, IBurnService service) =>
        {
            var success = await service.UpdateAsync(burn);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IBurnService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
