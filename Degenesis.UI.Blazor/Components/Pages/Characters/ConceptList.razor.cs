using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters
{
    public partial class ConceptList
    {
        private List<ConceptDto>? concepts;
        private List<SkillDto> skills = [];
        private List<AttributeDto> attributes = [];

        protected override async Task OnInitializedAsync()
        {
            await LoadConcepts();
        }

        private async Task LoadConcepts()
        {
            concepts = (await ConceptService.GetConceptsAsync()).ToList();
            skills = (await SkillService.GetSkillsAsync()).ToList();
            attributes = (await AttributeService.GetAttributesAsync()).ToList();
        }

        private async Task ShowCreateDialog()
        {
            var parameters = new DialogParameters
            {
                { "Concept", new ConceptDto() },
                { "Skills", skills },
                { "Attributes", attributes }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<ConceptModal>("Create Concept", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadConcepts();
            }
        }

        private async Task ShowEditDialog(Guid conceptId)
        {
            var concept = concepts?.FirstOrDefault(c => c.Id == conceptId);
            if (concept != null)
            {
                var parameters = new DialogParameters
                {
                    { "Concept", concept },
                    { "Skills", skills },
                    { "Attributes", attributes }
                };

                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

                var dialog = await DialogService.ShowAsync<ConceptModal>("Edit Concept", parameters, options);
                var result = await dialog.Result;

                if (result is not null && !result.Canceled)
                {
                    await LoadConcepts();
                }
            }
        }

        private async Task DeleteConcept(Guid conceptId)
        {
            await ConceptService.DeleteConceptAsync(conceptId);
            await LoadConcepts();
        }
    }
}
