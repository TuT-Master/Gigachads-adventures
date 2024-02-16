using System;

[Serializable]
public class GameData
{
    public SerializableDictionary<string, float> playerStats;

    public SerializableDictionary<int, Item> playerInventory;

    public GameData()
    {
        // Set the default stats
        playerStats = new()
        {
            { "hp", 100 },
            { "stamina", 100},
            { "mana", 100 },
            { "hpMax", 100 },
            { "staminaMax", 100 },
            { "manaMax", 100 },
            { "armor", 0 },
            { "evasion", 0 },
            { "weight", 0 },
            { "speed", 1 },
            { "experience", 0 },
            { "level", 0 },
            { "accuracyBonus", 0 },
            { "penetrationBonus", 0 },
            { "armorIgnoreBonus", 0 },
            { "skillIssue", 0 },
            { "backpackSize", 8 },
            { "beltSize", 0 },
            { "pocketSize", 0 },
        };
        playerInventory = new();
    }
}
