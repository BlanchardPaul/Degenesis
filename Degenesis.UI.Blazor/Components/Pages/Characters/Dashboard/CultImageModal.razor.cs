using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultImageModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CultDto Cult { get; set; } = null!;

    private IBrowserFile? _imageFile;
    private string? _previewUrl;

    private string ApiRoot => new Uri(Client!.BaseAddress!, "..").GetLeftPart(UriPartial.Authority);
    private string _cacheBuster = Guid.NewGuid().ToString("N");

    private string? CurrentImageUrl =>
        Cult.ImageName is null ? null : $"{ApiRoot}/cult-images/{Cult.ImageName}?v={_cacheBuster}";

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
            return;
        }

        if (file.Size > 2 * 1024 * 1024)
        {
            Snackbar.Add("Image too large (max 2 MB)", Severity.Warning);
            _imageFile = null;
            _previewUrl = CurrentImageUrl;
            return;
        }

        byte[] bytes;
        using (var ms = new MemoryStream())
        {
            await file.OpenReadStream(2 * 1024 * 1024).CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        _imageFile = file;
        _previewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
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

        Cult.ImageName = $"{Cult.Id}.jpg";
        _cacheBuster = Guid.NewGuid().ToString("N");
        _previewUrl = CurrentImageUrl;
        Snackbar.Add("Image updated", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private async Task ConfirmRemoveImage()
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", "Remove image ?" }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await RemoveImage();
        }
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