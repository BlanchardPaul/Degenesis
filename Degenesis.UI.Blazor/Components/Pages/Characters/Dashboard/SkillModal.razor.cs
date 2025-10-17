using Microsoft.AspNetCore.Components;
using MudBlazor;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class SkillModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public SkillDto Skill { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }
    protected override void OnParametersSet()
    {
        if (Attributes.Count != 0 && Skill.CAttributeId == Guid.Empty)
        {
            Skill.CAttributeId = Attributes.First().Id;
        }
    }

    private async Task SaveSkill()
    {
        if (Skill.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/skills", Skill);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync("/skills", Skill);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
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
