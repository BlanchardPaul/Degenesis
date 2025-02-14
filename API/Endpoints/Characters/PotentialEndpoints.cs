using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class PotentialEndpoints
{
    public static void MapPotentialEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/potentials").WithTags("Potentials");

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
        group.MapPost("/", async (Potential potential, IPotentialService potentialService) =>
        {
            var createdPotential = await potentialService.CreatePotentialAsync(potential);
            return createdPotential is not null
                ? Results.Created($"/potentials/{createdPotential.Id}", createdPotential)
                : Results.BadRequest();
        });

        // Update an existing potential
        group.MapPut("/{id}", async (Guid id, Potential potential, IPotentialService potentialService) =>
        {
            var success = await potentialService.UpdatePotentialAsync(id, potential);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // Delete a potential
        group.MapDelete("/{id}", async (Guid id, IPotentialService potentialService) =>
        {
            var success = await potentialService.DeletePotentialAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
