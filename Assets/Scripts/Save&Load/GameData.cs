using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static Item;
using static PlayerBase;
using static PlayerStats;

[Serializable]
public class GameData
{
    // Player stats
    public SerializableDictionary<StatType, float> playerStats;
    public Dictionary<StatType, float> baseStats = new()
        {
            { StatType.Hp,                          100 },
            { StatType.Stamina,                     100 },
            { StatType.Mana,                        100 },
            { StatType.HpMax,                       100 },
            { StatType.StaminaMax,                  100 },
            { StatType.ManaMax,                     100 },
            { StatType.HpRegen,                     1 },
            { StatType.StaminaRegen,                1 },
            { StatType.ManaRegen,                   1 },
            { StatType.Armor,                       0 },
            { StatType.IncreaseArmorByPercentage,   0 },
            { StatType.MagicResistance,             0 },
            { StatType.BleedingResistance,          0 },
            { StatType.PoisonResistance,            0 },
            { StatType.Evade,                       0 },
            { StatType.Player_Defense,              0 },
            { StatType.BonusDefense,                0 },
            { StatType.Weight,                      0 },
            { StatType.Speed,                       1 },
            { StatType.BackpackSize,                8 },
            { StatType.BeltSize,                    0 },
            { StatType.PocketSize,                  0 },
            { StatType.AdditionalInventorySlots,    0 },
            { StatType.Knockback,                   0 },
            { StatType.ArmorIgnore,                 0 },
            { StatType.NotConsumeStaminaChance,     0 },
            { StatType.StaminaConsumptionReduction, 0 },
            { StatType.AccuracyBonus,               0 },
            { StatType.StunChance,                  0 },
            { StatType.BleedingChance,              0 },
            { StatType.PoisonChance,                0 },
            { StatType.Range,                       0 },
            { StatType.Player_Exp,                  0 },
            { StatType.Player_Level,                0 },
            { StatType.SkillPoints,                 0 },
            { StatType.SkillIssuePoints,            0 },
            { StatType.Age,                         0 },
        };

    // Movement
    public Vector3 playerPos;

    // Player inventory
    public int backpackSize;
    public int beltSize;
    public int pocketSize;
    public SerializableDictionary<int, string> playerInventory;

    // Other inventories
    public SerializableDictionary<int, string> otherInventoriesNames;
    public SerializableDictionary<int, SerializableDictionary<int, string>> otherInventories;

    // PlayerBase
    public SerializableDictionary<BaseUpgrade, int> baseUpgrades;

    // Character creation
    public int[] characterSprites;
    public int difficulty;



    public GameData()
    {
        // Set the default stats
        playerStats = new();
        foreach(var pair in baseStats)
            playerStats[pair.Key] = pair.Value;
        playerInventory = new();
        playerPos = new Vector3(-52, 0, -52);

        otherInventories = new();

        baseUpgrades = new()
        {
            {BaseUpgrade.Bed, 0},
            {BaseUpgrade.Chest, 0},
            {BaseUpgrade.Kitchen, 0},
            {BaseUpgrade.AlchemyLab, 0},
            {BaseUpgrade.Smithy, 0},
            {BaseUpgrade.EnchantingTable, 0},
            {BaseUpgrade.Materials, 0},
            {BaseUpgrade.Upgrade, 0},
        };

        characterSprites = new int[3] { 0, 0, 0 };
        difficulty = 0;
    }
}
