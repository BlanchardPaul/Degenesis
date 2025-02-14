using Degenesis.Shared.DTOs.Burns;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnList
{
    private List<BurnDto>? burns;
    private BurnDto currentBurn = new();
    private bool isModalVisible = false;
    private string modalTitle = "Create Burn";

    protected override async Task OnInitializedAsync()
    {
        burns = await BurnService.GetBurnsAsync();
    }

    private void ShowCreateDialog()
    {
        Console.WriteLine("ShowCreateDialog appelé");
        modalTitle = "Create Burn";
        currentBurn = new BurnDto();
        isModalVisible = true;
    }

    private void ShowEditDialog(Guid burnId)
    {
        Console.WriteLine("ShowEditDialog appelé");
        var burn = burns?.FirstOrDefault(b => b.Id == burnId);
        if (burn != null)
        {
            modalTitle = "Edit Burn";
            currentBurn = new BurnDto
            {
                Id = burn.Id,
                Name = burn.Name,
                Description = burn.Description,
                Chakra = burn.Chakra,
                EarthChakra = burn.EarthChakra,
                Effect = burn.Effect,
                WeakCost = burn.WeakCost,
                PotentCost = burn.PotentCost
            };
            isModalVisible = true;
        }
    }

    private async Task SaveBurn()
    {
        if (currentBurn.Id == Guid.Empty)
            await BurnService.CreateBurnAsync(currentBurn);
        else
            await BurnService.UpdateBurnAsync(currentBurn);

        burns = await BurnService.GetBurnsAsync();
        await CloseModal();
    }

    private async Task DeleteBurn(Guid burnId)
    {
        await BurnService.DeleteBurnAsync(burnId);
        burns = await BurnService.GetBurnsAsync();
    }

    private async Task CloseModal()
    {
        Console.WriteLine("Fermeture modale");
        await InvokeAsync(() =>
        {
            isModalVisible = false;
        });
    }
}