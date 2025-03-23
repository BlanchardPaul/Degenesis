using Degenesis.Shared.DTOs.Users;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Users;
public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RegisterAsync(UserCreateDto userCreateDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/users/register", userCreateDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> LoginAsync(UserLoginDto userLoginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/users/login", userLoginDto);
        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadFromJsonAsync<string?>();
        }
        return null;
    }

}