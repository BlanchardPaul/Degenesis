using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Extensions;

public abstract class AuthenticatedComponentBase : ComponentBase
{
    [Inject]
    protected AuthenticatedHttpClientService HttpClientService { get; set; } = null!;

    protected HttpClient? Client { get; private set; }
    private bool _isInitialized = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_isInitialized)
        {
            _isInitialized = true;
            Client = await HttpClientService.GetClientAsync();
            await OnAuthenticatedInitializedAsync();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Ensures that the component is initialized after the authenticated HttpClient is available.
    /// </summary>
    protected virtual async Task OnAuthenticatedInitializedAsync()
    {
        await Task.CompletedTask;
    }
}