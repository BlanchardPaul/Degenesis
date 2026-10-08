using Degenesis.Shared.DTOs.Users;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Users;
public class UserService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> LoginAsync(UserLoginDto userLoginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/users/login", userLoginDto);
        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadFromJsonAsync<string?>();
        }
        return null;
    }

}