using Degenesis.UI.Service.Features.Users;

namespace Degenesis.UI.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7274/")
        });

        services.AddScoped<UserService>();
        services.AddScoped<AuthenticatedHttpClientService>();
        return services;
    }
}