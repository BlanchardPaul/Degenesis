//using Business.Weapons;
//using Domain.Weapons;

//namespace API.Endpoints.Weapons;

//public static class NPCWeaponEndpoints
//{
//    public static void MapNPCWeaponEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npc-weapons").WithTags("NPCWeapons");

//        group.MapGet("/{id}", async (INPCWeaponService service, Guid id) =>
//        {
//            var npcWeapon = await service.GetNPCWeaponByIdAsync(id);
//            return npcWeapon is not null ? Results.Ok(npcWeapon) : Results.NotFound();
//        });

//        group.MapGet("/npc/{npcId}", async (INPCWeaponService service, Guid npcId) =>
//        {
//            var npcWeapon = await service.GetNPCWeaponByNPCIdAsync(npcId);
//            return npcWeapon is not null ? Results.Ok(npcWeapon) : Results.NotFound();
//        });

//        group.MapPost("/", async (INPCWeaponService service, NPCWeapon npcWeapon) =>
//        {
//            await service.CreateNPCWeaponAsync(npcWeapon);
//            return Results.Created($"/npc-weapons/{npcWeapon.Id}", npcWeapon);
//        });

//        group.MapPut("/{id}", async (INPCWeaponService service, Guid id, NPCWeapon npcWeapon) =>
//        {
//            await service.UpdateNPCWeaponAsync(id, npcWeapon);
//            return Results.NoContent();
//        });

//        group.MapDelete("/{id}", async (INPCWeaponService service, Guid id) =>
//        {
//            await service.DeleteNPCWeaponAsync(id);
//            return Results.NoContent();
//        });

//    }
//}
