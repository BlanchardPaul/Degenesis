using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class PotentialEndpoints
{
    public static void MapPotentialEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/potentials").WithTags("Potentials").RequireAuthorization();

        group.MapGet("/", async (IPotentialService potentialService) =>
        {
            var result = await potentialService.GetAllPotentialsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var result = await potentialService.GetPotentialByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (PotentialCreateDto potential, IPotentialService potentialService) =>
        {
            var result = await potentialService.CreatePotentialAsync(potential);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (PotentialDto potential, IPotentialService potentialService) =>
        {
            var result = await potentialService.UpdatePotentialAsync(potential);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var result = await potentialService.DeletePotentialAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
