namespace Business.Characters;

public static class CharacterXpCalculator
{
    private static readonly string[] FocusPreferredAttributes = ["INTELLECT", "AGILITY", "PSYCHE"];
    private static readonly string[] PrimalPreferredAttributes = ["BODY", "CHARISMA", "INSTINCT"];

    public static bool IsPreferred(string attributeName, bool isFocusOriented)
        => isFocusOriented
            ? FocusPreferredAttributes.Contains(attributeName.ToUpper())
            : PrimalPreferredAttributes.Contains(attributeName.ToUpper());

    public static int AttributeCost(string attributeName, bool isFocusOriented, int oldLevel, int newLevel)
    {
        int factor = IsPreferred(attributeName, isFocusOriented) ? 10 : 12;
        return Math.Max(oldLevel, newLevel) * factor;
    }

    public static int SkillCost(string parentAttributeName, bool isFocusOriented, int oldLevel, int newLevel)
    {
        int factor = IsPreferred(parentAttributeName, isFocusOriented) ? 4 : 5;
        return Math.Max(oldLevel, newLevel) * factor;
    }

    public static int PotentialCost(int totalPotentialLevels, int oldLevel, int newLevel)
    {
        int cost = newLevel > oldLevel ? totalPotentialLevels + 1 : totalPotentialLevels;
        return cost * 10;
    }
}