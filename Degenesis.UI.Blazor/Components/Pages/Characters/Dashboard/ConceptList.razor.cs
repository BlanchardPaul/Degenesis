using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class ConceptList
{
    private List<ConceptDto>? concepts;
    private List<SkillDto> skills = [];
    private List<AttributeDto> attributes = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadConcepts();
    }

    private async Task LoadConcepts()
    {
        var conceptResult = await Client!.GetFromJsonAsync<Result<List<ConceptDto>>>("/concepts") ?? new Result<List<ConceptDto>> { IsError = true, Error = "Unknown error" };
        if (conceptResult.IsError)
        {
            Snackbar.Add("Error loading concepts: " + conceptResult.Error);
            concepts = [];
        }
        else
            concepts = conceptResult.Value ?? [];

        var skillsResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillsResult.IsError)
        {
            Snackbar.Add("Error loading skills: " + skillsResult.Error);
            skills = [];
        }
        else
            skills = skillsResult.Value ?? [];

        var attributesResult = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if (attributesResult.IsError)
        {
            Snackbar.Add("Error loading attributes: " + attributesResult.Error);
            attributes = [];
        }
        else
            attributes = attributesResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/concepts/{conceptId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadConcepts();
    }

    private bool FilterFunc(ConceptDto concept)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (concept.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
