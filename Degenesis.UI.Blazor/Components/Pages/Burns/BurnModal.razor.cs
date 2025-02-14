using Degenesis.Shared.DTOs.Burns;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnModal
{
    [Parameter] public string Title { get; set; } = "Modal";
    [Parameter] public BurnDto Burn { get; set; } = new();
    [Parameter] public EventCallback OnSave { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public bool IsVisible { get; set; }

    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }

    private async Task Close()
    {
        await IsVisibleChanged.InvokeAsync(false);
        await OnCancel.InvokeAsync();
    }
}