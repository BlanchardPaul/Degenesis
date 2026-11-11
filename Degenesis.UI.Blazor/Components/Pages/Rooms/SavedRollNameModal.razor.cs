using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class SavedRollNameModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private string _name = string.Empty;

    private void Save() => MudDialog.Close(DialogResult.Ok(_name.Trim()));
    private void Cancel() => MudDialog.Cancel();
}