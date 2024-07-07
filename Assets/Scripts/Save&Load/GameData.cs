using System;
using UnityEngine;
using static PlayerBase;

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

    // Other inventories
    public SerializableDictionary<Transform, SerializableDictionary<int, string>> otherInventories;

    // PlayerBase
    public SerializableDictionary<PlayerBase.BaseUpgrade, int> baseUpgrades;

    // Character creation
    public int[] characterSprites;
    public int difficulty;



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
            { "defense", 0 },
            { "weight", 80 },
            { "speed", 1 },
            { "exp_player", 0 },
            { "level_player", 0 },
            { "exp_oneHandDexterity", 0 },
            { "level_oneHandDexterity", 0 },
            { "exp_oneHandStrenght", 0 },
            { "level_oneHandStrenght", 0 },
            { "exp_twoHandDexterity", 0 },
            { "level_twoHandDexterity", 0 },
            { "exp_twoHandStrenght", 0 },
            { "level_twoHandStrenght", 0 },
            { "exp_rangedDexterity", 0 },
            { "level_rangedDexterity", 0 },
            { "exp_rangedStrenght", 0 },
            { "level_rangedStrenght", 0 },
            { "exp_magicFire", 0 },
            { "level_magicFire", 0 },
            { "exp_magicAir", 0 },
            { "level_magicAir", 0 },
            { "exp_magicWater", 0 },
            { "level_magicWater", 0 },
            { "exp_magicEarth", 0 },
            { "level_magicEarth", 0 },
            { "skillPoints", 0 },
            { "accuracyBonus", 0 },
            { "penetrationBonus", 0 },
            { "armorIgnoreBonus", 0 },
            { "skillIssue", 0 },
            { "backpackSize", 8 },
            { "beltSize", 0 },
            { "pocketSize", 0 },
            { "age", 0 },
        };
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
