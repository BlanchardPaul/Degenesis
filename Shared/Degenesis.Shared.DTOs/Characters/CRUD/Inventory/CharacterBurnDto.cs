using Degenesis.Shared.DTOs.Burns;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
public class CharacterBurnCreateDto
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Guid BurnId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class CharacterBurnDto : CharacterBurnCreateDto
{
    public BurnDto Burn { get; set; } = new();
}
