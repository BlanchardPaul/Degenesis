using Business.Characters;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class AttributeEndpoints
{
    public static void MapAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/attributes").WithTags("Attributes").RequireAuthorization();

        group.MapGet("/{id:guid}", async (Guid id, IAttributeService service) =>
        {
            var attribute = await service.GetAttributeByIdAsync(id);
            return attribute is not null ? Results.Ok(attribute) : Results.NotFound();
        });

        group.MapGet("/", async (IAttributeService service) =>
        {
            var attributes = await service.GetAllAttributesAsync();
            return Results.Ok(attributes);
        });

        group.MapPost("/", async (AttributeCreateDto attribute, IAttributeService service) =>
        {
            var created = await service.CreateAttributeAsync(attribute);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (CAttribute attribute, IAttributeService service) =>
        {
            var success = await service.UpdateAttributeAsync(attribute);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IAttributeService service) =>
        {
            var success = await service.DeleteAttributeAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
