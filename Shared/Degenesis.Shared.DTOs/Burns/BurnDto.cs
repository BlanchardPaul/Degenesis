namespace Degenesis.Shared.DTOs.Burns;
public class BurnDto : BurnCreateDto
{
    public Guid Id { get; set; }
}
public class BurnCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Chakra { get; set; } = string.Empty;
    public string EarthChakra { get; set; } = string.Empty;
    public string Effect { get; set; } = string.Empty;
    public int WeakCost { get; set; }
    public int PotentCost { get; set; }
}
