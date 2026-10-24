namespace Degenesis.Shared.DTOs.Characters.CRUD;
public class CharacterBasicInfosEditDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Sex { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
}