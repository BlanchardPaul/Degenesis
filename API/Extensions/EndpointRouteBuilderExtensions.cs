using API.Endpoints._Artifacts;
using API.Endpoints.Burns;
using API.Endpoints.Characters;
using API.Endpoints.Characters.Inventory;
using API.Endpoints.Equipments;
using API.Endpoints.Npcs;
using API.Endpoints.Protections;
using API.Endpoints.Rooms;
using API.Endpoints.Users;
using API.Endpoints.Vehicles;
using API.Endpoints.Weapons;

namespace API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapArtifactEndpoints();
        endpoints.MapCharacterArtifactEndpoints();
        endpoints.MapCharacterEndpoints();
        endpoints.MapAttributeEndpoints();
        endpoints.MapBackgroundEndpoints();
        endpoints.MapCharacterAttributeEndpoints();
        endpoints.MapCharacterBackgroundEndpoints();
        endpoints.MapCharacterPotentialEndpoints();
        endpoints.MapCharacterSkillEndpoints();
        endpoints.MapConceptEndpoints();
        endpoints.MapCultEndpoints();
        endpoints.MapCultureEndpoints();
        endpoints.MapPotentialEndpoints();
        endpoints.MapPotentialPrerequisiteEndpoints();
        endpoints.MapRankEndpoints();
        endpoints.MapRankPrerequisiteEndpoints();
        endpoints.MapSkillEndpoints();
        endpoints.MapBurnEndpoints();
        endpoints.MapCharacterBurnEndpoints();
        endpoints.MapCharacterEquipmentEndpoints();
        endpoints.MapEquipmentEndpoints();
        endpoints.MapEquipmentTypeEndpoints();        
        endpoints.MapCharacterProtectionEndpoints();
        endpoints.MapProtectionEndpoints();
        endpoints.MapCharacterVehicleEndpoints();
        endpoints.MapVehicleEndpoints();
        endpoints.MapVehicleTypeEndpoints();
        endpoints.MapCharacterWeaponEndpoints();
        endpoints.MapWeaponEndpoints();
        endpoints.MapWeaponTypeEndpoints();
        endpoints.MapBaseNPCEndpoints();
        endpoints.MapUserEndpoints();
        endpoints.MapRoomEndpoints();
        return endpoints;
    }
}