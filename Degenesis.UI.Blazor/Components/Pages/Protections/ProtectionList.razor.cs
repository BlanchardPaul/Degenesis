using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Protections;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Protections;


public partial class ProtectionList
{
    private List<ProtectionDto>? protections;
    private List<CultDto> Cults = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadProtections();
    }

    private async Task LoadProtections()
    {
        protections = await Client!.GetFromJsonAsync<List<ProtectionDto>>("/protections") ?? [];
        Cults = await Client!.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
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
        var protection = protections?.FirstOrDefault(p => p.Id == protectionId);
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
        var result = await Client!.DeleteAsync($"/protections/{protectionId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
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