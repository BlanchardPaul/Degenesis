using Business.Burns;
using Degenesis.Shared.DTOs.Burns;

namespace API.Endpoints.Burns;

public static class BurnEndpoints
{
    public static void MapBurnEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/burns").WithTags("Burns").RequireAuthorization();

        group.MapGet("/", async (IBurnService service) =>
        {
            var result = await service.GetAllAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IBurnService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (BurnCreateDto burn, IBurnService service) =>
        {
            var result = await service.CreateAsync(burn);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (BurnDto burn, IBurnService service) =>
        {
            var result = await service.UpdateAsync(burn);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IBurnService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
