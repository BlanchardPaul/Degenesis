using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultImageModal
{
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CultDto Cult { get; set; } = null!;

    private IBrowserFile? _imageFile;
    private string? _previewUrl;

    private const int RequiredWidth = 1200;
    private const int RequiredHeight = 600;

    private string ApiRoot => new Uri(Client!.BaseAddress!, "..").GetLeftPart(UriPartial.Authority);
    private string? CurrentImageUrl =>
        Cult.ImageName is null ? null : $"{ApiRoot}/cult-images/{Cult.ImageName}";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await base.OnAuthenticatedInitializedAsync();
        _previewUrl = CurrentImageUrl;
        StateHasChanged();
    }

    private async Task OnImageSelected(IBrowserFile file)
    {
        if (file is null)
        {
            _imageFile = null;
            _previewUrl = CurrentImageUrl;
            StateHasChanged();
            return;
        }

        if (file.Size > 2 * 1024 * 1024)
        {
            Snackbar.Add("Image too large (max 2 MB)", Severity.Warning);
            return;
        }

        // Load file for preview (as data URL)
        byte[] bytes;
        using (var ms = new MemoryStream())
        {
            await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        // Client-side dimension check
        if (!await IsValidDimensionsAsync(bytes))
        {
            Snackbar.Add($"Image must be exactly {RequiredWidth}×{RequiredHeight} px", Severity.Warning);
            _imageFile = null;
            _previewUrl = CurrentImageUrl;
            return;
        }

        _imageFile = file;
        _previewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
        StateHasChanged();
    }

    // Small JS interop to decode image dimensions without a library
    private async Task<bool> IsValidDimensionsAsync(byte[] bytes)
    {
        try
        {
            using var streamRef = new DotNetStreamReference(new MemoryStream(bytes));
            var size = await JS.InvokeAsync<JsonElement>("readImageDimensions", streamRef);
            return size.GetProperty("width").GetInt32() == RequiredWidth
                && size.GetProperty("height").GetInt32() == RequiredHeight;
        }
        catch
        {
            // JS interop failed let the server validate (it does the same check anyway)
            return true;
        }
    }

    private async Task UploadImage()
    {
        if (_imageFile is null || Client is null)
            return;

        using var content = new MultipartFormDataContent();
        using var fileStream = _imageFile.OpenReadStream(2 * 1024 * 1024);
        using var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(_imageFile.ContentType);
        content.Add(streamContent, "file", _imageFile.Name);

        var response = await Client!.PostAsync($"/cults/{Cult.Id}/image", content);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Image updated", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private async Task RemoveImage()
    {
        var response = await Client!.DeleteAsync($"/cults/{Cult.Id}/image");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Image removed", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}