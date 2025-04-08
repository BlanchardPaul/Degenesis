using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class PotentialEndpoints
{
    public static void MapPotentialEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/potentials").WithTags("Potentials").RequireAuthorization();

        // Get all potentials
        group.MapGet("/", async (IPotentialService potentialService) =>
        {
            var potentials = await potentialService.GetAllPotentialsAsync();
            return Results.Ok(potentials);
        });

        // Get potential by ID
        group.MapGet("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var potential = await potentialService.GetPotentialByIdAsync(id);
            return potential is not null ? Results.Ok(potential) : Results.NotFound();
        });

        // Create a new potential
        group.MapPost("/", async (PotentialCreateDto potential, IPotentialService potentialService) =>
        {
            var created = await potentialService.CreatePotentialAsync(potential);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        // Update an existing potential
        group.MapPut("/", async (PotentialDto potential, IPotentialService potentialService) =>
        {
            var success = await potentialService.UpdatePotentialAsync(potential);
            return success ? Results.Ok() : Results.NotFound();
        });

        // Delete a potential
        group.MapDelete("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var success = await potentialService.DeletePotentialAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
