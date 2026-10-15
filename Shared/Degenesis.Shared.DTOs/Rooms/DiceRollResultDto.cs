namespace Degenesis.Shared.DTOs.Rooms;
public class DiceRollResultDto
{
    public string Roller { get; set; } = string.Empty;
    public string RollDescription { get; set; } = string.Empty;
    public int DiceCount { get; set; }
    public List<int> Rolls { get; set; } = [];
    public int Botches { get; set; }
    public int Failures { get; set; }
    public int Successes { get; set; }
    public int Triggers { get; set; }
    public int Total { get; set; }
    public bool IsActionRoll { get; set; }
    public int AutoSuccesses { get; set; }
}
