using Degenesis.UI.Service.Features;
using Degenesis.UI.Service.Features.Characters;

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
        services.AddScoped<SkillService>();
        return services;
    }
}