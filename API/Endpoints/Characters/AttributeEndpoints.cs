using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class AttributeEndpoints
{
    public static void MapAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/attributes").WithTags("Attributes").RequireAuthorization();

        group.MapGet("/", async (IAttributeService service) =>
        {
            var result = await service.GetAllAttributesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IAttributeService service) =>
        {
            var result = await service.GetAttributeByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (AttributeCreateDto attribute, IAttributeService service) =>
        {
            var result = await service.CreateAttributeAsync(attribute);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (AttributeDto attribute, IAttributeService service) =>
        {
            var result = await service.UpdateAttributeAsync(attribute);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IAttributeService service) =>
        {
            var result = await service.DeleteAttributeAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
