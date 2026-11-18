using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class GmRoomMapList
{
    private const long MaxMapSizeBytes = 20 * 1024 * 1024;

    [Parameter] public Guid IdRoom { get; set; }

    private readonly List<RoomMapDto> _maps = [];
    private IBrowserFile? _selectedFile;
    private bool _isLoading = true;
    private bool _isUploading;
    private int _fileUploadKey;
    private string _imageVersion = Guid.NewGuid().ToString("N");
    private string ApiRoot => Client?.BaseAddress is null
        ? string.Empty
        : new Uri(Client.BaseAddress, "..").GetLeftPart(UriPartial.Authority);

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadMapsAsync();
    }

    private async Task LoadMapsAsync()
    {
        _isLoading = true;
        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapDto>>>($"/room-maps/room/{IdRoom}");
        if (result is null || result.IsError)
        {
            Snackbar.Add(result?.Error ?? "Unable to load background maps", Severity.Error);
        }
        else
        {
            _maps.Clear();
            _maps.AddRange(result.Value ?? []);
        }

        _isLoading = false;
    }

    private void OnImageSelected(IBrowserFile? file)
    {
        if (file is null)
        {
            _selectedFile = null;
            return;
        }

        if (file.Size > MaxMapSizeBytes)
        {
            Snackbar.Add("Image too large (max 20 MB)", Severity.Warning);
            _selectedFile = null;
            return;
        }

        _selectedFile = file;
    }

    private async Task UploadMap()
    {
        if (_selectedFile is null || Client is null)
            return;

        _isUploading = true;
        using var content = new MultipartFormDataContent();
        using var fileStream = _selectedFile.OpenReadStream(MaxMapSizeBytes);
        using var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_selectedFile.ContentType);
        content.Add(streamContent, "file", _selectedFile.Name);

        var response = await Client.PostAsync($"/room-maps/room/{IdRoom}", content);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unable to upload map", Severity.Error);
        }
        else
        {
            _selectedFile = null;
            _fileUploadKey++;
            _imageVersion = Guid.NewGuid().ToString("N");
            Snackbar.Add("Map saved", Severity.Success);
            await LoadMapsAsync();
        }

        _isUploading = false;
    }

    private async Task ConfirmDeleteMap(RoomMapDto map)
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", $"Delete map '{map.Name}'?" }
        };
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters,
            new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small });
        var result = await dialog.Result;
        if (result is null || result.Canceled)
            return;

        var response = await Client!.DeleteAsync($"/room-maps/{map.Id}");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(error?.Error ?? "Unable to delete map", Severity.Error);
            return;
        }

        _maps.Remove(map);
        Snackbar.Add("Map deleted", Severity.Success);
    }

    private string GetMapImageUrl(RoomMapDto map)
        => $"{ApiRoot}/map-images/{Uri.EscapeDataString(map.FileName)}?v={_imageVersion}";
}
