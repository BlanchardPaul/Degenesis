using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters
{
    public partial class CultList
    {
        [Inject] private IDialogService DialogService { get; set; } = default!;
        private List<CultDto>? cults;
        private List<SkillDto> skills = [];

        protected override async Task OnInitializedAsync()
        {
            await LoadCults();
        }

        private async Task LoadCults()
        {
            cults = await CultService.GetCultsAsync();
            skills = await SkillService.GetSkillsAsync();
        }

        private async Task ShowCreateDialog()
        {
            var parameters = new DialogParameters { { "Cult", new CultDto() }, { "Skills", skills } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<CultModal>("Create Cult", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadCults();
            }
        }

        private async Task ShowEditDialog(Guid cultId)
        {
            var cult = cults?.FirstOrDefault(c => c.Id == cultId);
            if (cult != null)
            {
                var parameters = new DialogParameters { { "Cult", cult }, { "Skills", skills } };
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

                var dialog = await DialogService.ShowAsync<CultModal>("Edit Cult", parameters, options);
                var result = await dialog.Result;

                if (result is not null && !result.Canceled)
                {
                    await LoadCults();
                }
            }
        }

        private async Task DeleteCult(Guid cultId)
        {
            await CultService.DeleteCultAsync(cultId);
            await LoadCults();
        }
    }
}