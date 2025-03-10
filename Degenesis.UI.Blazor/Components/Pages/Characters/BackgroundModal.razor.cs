using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters
{
    public partial class BackgroundModal
    {
        [Inject] private Degenesis.UI.Service.Features.Characters.BackgroundService BackgroundService { get; set; } = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public BackgroundDto Background { get; set; } = new();

        private async Task SaveBackground()
        {
            if (Background.Id == Guid.Empty)
                await BackgroundService.CreateBackgroundAsync(Background);
            else
                await BackgroundService.UpdateBackgroundAsync(Background);

            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel() => MudDialog.Cancel();

    }
}
