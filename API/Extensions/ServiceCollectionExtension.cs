using Business;
using Business.Artifacts;
using Business.Burns;
using Business.Characters;
using Business.Equipments;
using Business.NPCs;
using Business.Protections;
using Business.Vehicles;
using Business.Weapons;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IArtifactService, ArtifactService>();
        services.AddScoped<ICharacterArtifactService, CharacterArtifactService>();
        services.AddScoped<INPCArtifactService, NPCArtifactService>();

        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<IBackgroundService, Business.Characters.BackgroundService>();
        services.AddScoped<ICharacterAttributeService, CharacterAttributeService>();
        services.AddScoped<ICharacterBackgroundService, CharacterBackgroundService>();
        services.AddScoped<ICharacterPotentialService, CharacterPotentialService>();
        services.AddScoped<ICharacterSkillService, CharacterSkillService>();
        services.AddScoped<IConceptService, ConceptService>();
        services.AddScoped<ICultService, CultService>();
        services.AddScoped<ICultureService, CultureService>();
        services.AddScoped<IPotentialService, PotentialService>();
        services.AddScoped<IRankService, RankService>();
        services.AddScoped<IRankPrerequisiteService, RankPrerequisiteService>();
        services.AddScoped<ISkillService, SkillService>();

        services.AddScoped<IBurnService, BurnService>();
        services.AddScoped<ICharacterBurnService, CharacterBurnService>();
        services.AddScoped<INPCBurnService, NPCBurnService>();

        services.AddScoped<ICharacterEquipmentService, CharacterEquipmentService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
        services.AddScoped<INPCEquipmentService,NPCEquipmentService>();

        services.AddScoped<ICharacterProtectionService, CharacterProtectionService>();
        services.AddScoped<INPCProtectionService, NPCProtectionService>();
        services.AddScoped<IProtectionService, ProtectionService>();
        services.AddScoped<IProtectionQualityService, ProtectionQualityService>();

        services.AddScoped<ICharacterVehicleService, CharacterVehicleService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();

        services.AddScoped<ICharacterWeaponService, CharacterWeaponService>();
        services.AddScoped<INPCWeaponService, NPCWeaponService>();
        services.AddScoped<IWeaponService, WeaponService>();
        services.AddScoped<IWeaponQualityService, WeaponQualityService>();
        services.AddScoped<IWeaponTypeService, WeaponTypeService>();

        services.AddScoped<INPCService, NPCService>();
        services.AddScoped<INPCAttributeService, NPCAttributeService>();
        services.AddScoped<INPCPotentialService, NPCPotentialService>();
        services.AddScoped<INPCSkillService, NPCSkillService>();
        return services;
    }
}