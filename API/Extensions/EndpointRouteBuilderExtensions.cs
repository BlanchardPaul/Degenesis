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
        endpoints.MapAttributeEndpoints();
        endpoints.MapBackgroundEndpoints();
        endpoints.MapBaseNPCEndpoints();
        endpoints.MapBurnEndpoints();
        endpoints.MapCharacterArtifactEndpoints();

        endpoints.MapCharacterAttributeEndpoints();
        endpoints.MapCharacterBackgroundEndpoints();
        endpoints.MapCharacterBurnEndpoints();
        endpoints.MapCharacterEndpoints();
        endpoints.MapCharacterEquipmentEndpoints();
        endpoints.MapCharacterPotentialEndpoints();
        endpoints.MapCharacterProtectionEndpoints();
        endpoints.MapCharacterSkillEndpoints();
        endpoints.MapCharacterVehicleEndpoints();
        endpoints.MapCharacterWeaponEndpoints();
        endpoints.MapConceptEndpoints();
        endpoints.MapCultEndpoints();
        endpoints.MapCultureEndpoints();
        endpoints.MapEquipmentEndpoints();
        endpoints.MapEquipmentTypeEndpoints();
        endpoints.MapNPCEndpoints();
        endpoints.MapPotentialEndpoints();
        endpoints.MapPotentialPrerequisiteEndpoints();
        endpoints.MapProtectionEndpoints();
        endpoints.MapRankEndpoints();
        endpoints.MapRankPrerequisiteEndpoints();
        endpoints.MapRoomEndpoints();
        endpoints.MapSkillEndpoints();
        endpoints.MapUserEndpoints();
        endpoints.MapVehicleEndpoints();
        endpoints.MapVehicleTypeEndpoints();
        endpoints.MapWeaponEndpoints();
        endpoints.MapWeaponTypeEndpoints();
        return endpoints;
    }
}