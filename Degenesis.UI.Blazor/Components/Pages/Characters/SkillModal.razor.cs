using Microsoft.AspNetCore.Components;
using Degenesis.Shared.DTOs.Characters;
using MudBlazor;
using Degenesis.UI.Service.Features.Characters;

namespace Degenesis.UI.Blazor.Components.Pages.Characters
{
    public partial class SkillModal
    {
        [Inject] private SkillService SkillService { get; set; } = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        [Parameter] public SkillDto Skill { get; set; } = new();
        [Parameter] public List<AttributeDto> Attributes { get; set; } = new();

        protected override void OnParametersSet()
        {
            if (Attributes.Count!=0 && Skill.CAttributeId == Guid.Empty)
            {
                Skill.CAttributeId = Attributes.First().Id;
            }
        }

        private async Task SaveSkill()
        {
            if (Skill.Id == Guid.Empty)
                await SkillService.CreateSkillAsync(Skill);
            else
                await SkillService.UpdateSkillAsync(Skill);

            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
