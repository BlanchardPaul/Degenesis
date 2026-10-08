using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class SkillModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public SkillDto Skill { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];

    protected override void OnParametersSet()
    {
        if (Attributes.Count != 0 && Skill.CAttributeId == Guid.Empty)
        {
            Skill.CAttributeId = Attributes.First().Id;
        }
    }

    private async Task SaveSkill()
    {
        HttpResponseMessage response;
        if (Skill.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/skills", Skill);
        else
            response = await Client!.PutAsJsonAsync("/skills", Skill);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Skill.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void OnAttributeChanged(Guid value)
    {
        Skill.CAttributeId = value;
        Skill.CAttribute = Attributes.First(a => a.Id == value);
        StateHasChanged();
    }

    private void Cancel() => MudDialog.Cancel();
}
