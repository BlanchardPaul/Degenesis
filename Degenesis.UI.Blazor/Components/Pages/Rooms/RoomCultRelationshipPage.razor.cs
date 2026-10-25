using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomCultRelationshipPage
{
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;
    [Parameter] public Guid IdRoom { get; set; }
    [Parameter] public bool IsGM { get; set; } = false;
    private string ApiRoot => new Uri(Client!.BaseAddress!, "..").GetLeftPart(UriPartial.Authority);
    private HubConnection? hubConnection;
    private List<CultRelationshipDto>? CultRelationships;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        // Reuse the hub connection already created by RoomPage (same circuit)
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        var userName = auth.User.FindFirst("unique_name")?.Value;
        if (!string.IsNullOrEmpty(userName))
            hubConnection = await RoomConnection.GetOrCreateAsync(IdRoom, userName);

        // Live updates when the GM discovers a cult or edits a relationship
        await ReloadCultRelationships();
        hubConnection?.On("CultRelationshipUpdated", async () =>
        {
            Console.WriteLine(">>> CultRelationshipUpdated received");
            await InvokeAsync(ReloadCultRelationships);
        });
    }

    private async Task ReloadCultRelationships()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<CultRelationshipDto>>>(
            $"/rooms/{IdRoom}/cult-relationships")
            ?? new Result<List<CultRelationshipDto>> { IsError = true, Error = "Unknown error" };

        if (result.IsError)
        {
            Snackbar.Add("Error loading cults relationships: " + result.Error, Severity.Error);
            CultRelationships = [];
        }
        else
            CultRelationships = result.Value ?? [];

        StateHasChanged();
    }

    private async Task DiscoverCult(CultRelationshipDto cult)
    {
        var response = await Client!.PostAsync(
            $"/rooms/{IdRoom}/cult-relationships/{cult.Id}/discover", null);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add($"{cult.CultName} discovered", Severity.Success);
    }

    private async Task UpdateRelationshipAsync(CultRelationshipDto cult)
    {
        var response = await Client!.PutAsJsonAsync($"/rooms/cult-relationships/relationship", new IdIntEditDto { Id = cult.Id, Value = cult.Relationship });

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        StateHasChanged();  
    }

    private static string FormatRelationship(int relationship) =>
        relationship > 0 ? $"+{relationship}" : relationship.ToString();

    private static string GetRelationshipStyle(int relationship) => relationship switch
    {
        > 0 => "color: var(--mud-palette-success);",
        < 0 => "color: var(--mud-palette-error);",
        _ => "color: var(--mud-palette-text-secondary);"
    };

}