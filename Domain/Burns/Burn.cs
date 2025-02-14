namespace Domain.Burns;
public class Burn
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Chakra { get; set; } = string.Empty;
    public string EarthChakra { get; set; } = string.Empty;
    public string Effect { get; set; } = string.Empty;
    public int WeakCost { get; set; } = 0;
    public int PotentCost { get; set; } = 0;
}