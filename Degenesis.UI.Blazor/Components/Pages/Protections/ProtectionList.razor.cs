using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Protections;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;


public partial class ProtectionList
{
    private List<ProtectionDto>? Protections;
    private List<CultDto> Cults = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadProtections();
    }

    private async Task LoadProtections()
    {
        Protections = await Client!.GetFromJsonAsync<List<ProtectionDto>>("/protections") ?? [];
        Cults = await Client!.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
        var protectionResult = await Client!.GetFromJsonAsync<Result<List<ProtectionDto>>>("/protections") ?? new Result<List<ProtectionDto>> { IsError = true, Error = "Unknown error" };
        if (protectionResult.IsError)
        {
            Snackbar.Add($"Error loading potentials: {protectionResult.Error}", Severity.Error);
            Protections = [];
        }
        else
            Protections = protectionResult.Value ?? [];
        
        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultResult.Value ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Protection", new ProtectionDto() },
                { "Cults", Cults }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<ProtectionModal>("Create Protection", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadProtections();
        }
    }

    private async Task ShowEditDialog(Guid protectionId)
    {
        var protection = Protections?.FirstOrDefault(p => p.Id == protectionId);
        if (protection != null)
        {
            var parameters = new DialogParameters
                {
                    { "Protection", protection },
                    { "Cults", Cults }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<ProtectionModal>("Edit Protection", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadProtections();
            }
        }
    }

    private async Task DeleteProtection(Guid protectionId)
    {
        var response = await Client!.DeleteAsync($"/protections/{protectionId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
        await LoadProtections();
    }
    private bool FilterFunc(ProtectionDto protection)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (protection.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;
        return false;
    }
}