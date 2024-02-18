using System;
using UnityEngine;

[Serializable]
public class GameData
{
    // Player stats
    public SerializableDictionary<string, float> playerStats;

    // Movement
    public Vector3 playerPos;

    // Player inventory
    public int backpackSize;
    public int beltSize;
    public int pocketSize;
    public SerializableDictionary<Transform, string> playerInventory;

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
            { "hpRegen", 1 },
            { "staminaRegen", 1 },
            { "manaRegen", 1 },
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
            { "beltSize", 1 },
            { "pocketSize", 0 },
        };
        playerInventory = new();
        playerPos = Vector3.zero;
    }
}
