using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace Degenesis.UI.Blazor.Extensions;

public class AuthenticatedHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ProtectedLocalStorage _localStorage;

    public AuthenticatedHttpClientService(IHttpClientFactory clientFactory, ProtectedLocalStorage localStorage)
    {
        _clientFactory = clientFactory;
        _localStorage = localStorage;
    }

    // Méthode qui récupère le client HTTP authentifié
    public async Task<HttpClient> GetClientAsync()
    {
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>("authToken");
            var token = tokenResult.Success ? tokenResult.Value : null;

            // Créer le client HTTP
            var client = _clientFactory.CreateClient("API");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
        catch (Exception)
        {
            return _clientFactory.CreateClient("API");
        }

    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>("authToken");
            return tokenResult.Success ? tokenResult.Value : null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}