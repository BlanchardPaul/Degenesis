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
            var potentials = await potentialService.GetAllPotentialsAsync();
            return Results.Ok(potentials);
        });

        group.MapGet("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var potential = await potentialService.GetPotentialByIdAsync(id);
            return potential is not null ? Results.Ok(potential) : Results.NotFound();
        });

        group.MapPost("/", async (PotentialCreateDto potential, IPotentialService potentialService) =>
        {
            var created = await potentialService.CreatePotentialAsync(potential);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (PotentialDto potential, IPotentialService potentialService) =>
        {
            var success = await potentialService.UpdatePotentialAsync(potential);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var success = await potentialService.DeletePotentialAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
