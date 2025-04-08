using API.Endpoints._Artifacts;
using API.Endpoints.Burns;
using API.Endpoints.Characters;
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
        endpoints.MapNPCArtifactEndpoints();

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
        endpoints.MapRankEndpoints();
        endpoints.MapRankPrerequisiteEndpoints();
        endpoints.MapSkillEndpoints();

        endpoints.MapBurnEndpoints();
        endpoints.MapCharacterBurnEndpoints();
        endpoints.MapNPCBurnEndpoints();

        endpoints.MapCharacterEquipmentEndpoints();
        endpoints.MapEquipmentEndpoints();
        endpoints.MapEquipmentTypeEndpoints();
        endpoints.MapNPCEquipmentEndpoints();
        
        endpoints.MapCharacterProtectionEndpoints();
        endpoints.MapNPCProtectionEndpoints();
        endpoints.MapProtectionEndpoints();
        endpoints.MapProtectionQualityEndpoints();

        endpoints.MapCharacterVehicleEndpoints();
        endpoints.MapVehicleEndpoints();
        endpoints.MapVehicleTypeEndpoints();

        endpoints.MapCharacterWeaponEndpoints();
        endpoints.MapNPCWeaponEndpoints();
        endpoints.MapWeaponEndpoints();
        endpoints.MapWeaponQualityEndpoints();
        endpoints.MapWeaponTypeEndpoints();

        endpoints.MapNPCEndpoints();
        endpoints.MapNPCAttributeEndpoints();
        endpoints.MapNPCPotentialEndpoints();
        endpoints.MapNPCSkillEndpoints();

        endpoints.MapUserEndpoints();

        endpoints.MapRoomEndpoints();
        return endpoints;
    }
}