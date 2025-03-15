using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.UI.Service.Features;
using Degenesis.UI.Service.Features.Characters;
using Degenesis.UI.Service.Features.Equipments;
using Degenesis.UI.Service.Features.Protections;
using Degenesis.UI.Service.Features.Vehicles;

namespace Degenesis.UI.Blazor.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7274/")
        });

        services.AddScoped<ArtifactService>();
        services.AddScoped<BurnService>();

        services.AddScoped<AttributeService>();
        services.AddScoped<Service.Features.Characters.BackgroundService>();
        services.AddScoped<ConceptService>();
        services.AddScoped<CultService>();
        services.AddScoped<CultureService>();
        services.AddScoped<EquipmentService>();
        services.AddScoped<EquipmentTypeService>();
        services.AddScoped<PotentialService>();
        services.AddScoped<ProtectionService>();
        services.AddScoped<ProtectionQualityService>();
        services.AddScoped<RankService>();
        services.AddScoped<RankPrerequisiteService>();
        services.AddScoped<SkillService>();
        services.AddScoped<VehicleService>();
        services.AddScoped<VehicleTypeService>();
        return services;
    }
}