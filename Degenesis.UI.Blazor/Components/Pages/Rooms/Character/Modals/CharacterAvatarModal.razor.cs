using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.Character.Modals;

public partial class CharacterAvatarModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = null!;

    private IBrowserFile? _imageFile;
    private string? _previewUrl;

    private string ApiRoot => new Uri(Client!.BaseAddress!, "..").GetLeftPart(UriPartial.Authority);
    private string _cacheBuster = Guid.NewGuid().ToString("N");

    private string? CurrentAvatarUrl => Character.AvatarName is null
        ? null
        : $"{ApiRoot}/character-avatars/{Character.AvatarName}?v={_cacheBuster}";

    protected override Task OnAuthenticatedInitializedAsync()
    {
        _previewUrl = CurrentAvatarUrl;
        return Task.CompletedTask;
    }

    private async Task OnImageSelected(IBrowserFile file)
    {
        if (file is null)
        {
            _imageFile = null;
            _previewUrl = CurrentAvatarUrl;
            return;
        }

        if (file.Size > 2 * 1024 * 1024)
        {
            Snackbar.Add("Image too large (max 2 MB)", Severity.Warning);
            _imageFile = null;
            _previewUrl = CurrentAvatarUrl;
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

    private async Task UploadAvatar()
    {
        if (_imageFile is null || Client is null)
            return;

        using var content = new MultipartFormDataContent();
        using var fileStream = _imageFile.OpenReadStream(2 * 1024 * 1024);
        using var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(_imageFile.ContentType);
        content.Add(streamContent, "file", _imageFile.Name);

        var response = await Client.PostAsync($"/characters/{Character.Id}/avatar", content);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Character.AvatarName = $"{Character.Id}{Path.GetExtension(_imageFile.Name).ToLowerInvariant()}";
        _cacheBuster = Guid.NewGuid().ToString("N");
        _previewUrl = CurrentAvatarUrl;
        Snackbar.Add("Avatar updated", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private async Task ConfirmRemoveAvatar()
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", "Remove avatar?" }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
            await RemoveAvatar();
    }

    private async Task RemoveAvatar()
    {
        var response = await Client!.DeleteAsync($"/characters/{Character.Id}/avatar");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Character.AvatarName = null;
        _cacheBuster = Guid.NewGuid().ToString("N");
        _previewUrl = null;
        Snackbar.Add("Avatar removed", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
