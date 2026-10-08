using Business.Characters;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class CultEndpoints
{
    public static void MapCultEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cults").WithTags("Cults").RequireAuthorization();

        group.MapGet("/", async (ICultService service) =>
        {
            var result = await service.GetAllCultsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICultService service) =>
        {
            var result = await service.GetCultByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CultCreateDto cult, ICultService service) =>
        {
            var result = await service.CreateCultAsync(cult);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CultDto cult, ICultService service) =>
        {
            var result = await service.UpdateCultAsync(cult);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICultService service) =>
        {
            var result = await service.DeleteCultAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
