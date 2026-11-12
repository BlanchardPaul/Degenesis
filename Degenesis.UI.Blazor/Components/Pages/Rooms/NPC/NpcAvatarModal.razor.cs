using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.NPC;

public partial class NpcAvatarModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public NPCDto NPC { get; set; } = null!;

    private IBrowserFile? _imageFile;
    private string? _previewUrl;

    private const int RequiredWidth = 512;
    private const int RequiredHeight = 512;

    private string ApiRoot => new Uri(Client!.BaseAddress!, "..").GetLeftPart(UriPartial.Authority);
    private string? CurrentAvatarUrl => NPC.AvatarName is null
        ? null
        : $"{ApiRoot}/npc-avatars/{NPC.AvatarName}";

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

        if (!await IsValidDimensionsAsync(bytes))
        {
            Snackbar.Add($"Avatar must be exactly {RequiredWidth}×{RequiredHeight} px", Severity.Warning);
            _imageFile = null;
            _previewUrl = CurrentAvatarUrl;
            return;
        }

        _imageFile = file;
        _previewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
    }

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
            return true;
        }
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

        var response = await Client.PostAsync($"/npcs/{NPC.Id}/avatar", content);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        NPC.AvatarName = $"{NPC.Id}{Path.GetExtension(_imageFile.Name).ToLowerInvariant()}";
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
        var response = await Client!.DeleteAsync($"/npcs/{NPC.Id}/avatar");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        NPC.AvatarName = null;
        Snackbar.Add("Avatar removed", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
