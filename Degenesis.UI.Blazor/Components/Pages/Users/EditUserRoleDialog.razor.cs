using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Users;

public partial class EditUserRoleDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public string CurrentRole { get; set; } = string.Empty;

    private string? _selectedRole;

    protected override void OnInitialized()
    {
        _selectedRole = CurrentRole.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
    }

    private void Save() => MudDialog.Close(DialogResult.Ok(_selectedRole));

    private void Cancel() => MudDialog.Cancel();
}
