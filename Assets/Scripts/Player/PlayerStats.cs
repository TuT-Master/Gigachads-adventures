using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDataPersistance
{
    public Dictionary<string, float> playerStats;

    #region Stats setup
    [SerializeField]
    private float hp;
    [SerializeField]
    private float stamina;
    [SerializeField]
    private float mana;
    [SerializeField]
    private float hpMax;
    [SerializeField]
    private float staminaMax;
    [SerializeField]
    private float manaMax;
    [SerializeField]
    private float armor;
    [SerializeField]
    private float evasion;
    [SerializeField]
    private float weight;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int experience;
    [SerializeField]
    private int level;
    [SerializeField]
    private float accuracyBonus;
    [SerializeField]
    private float penetrationBonus;
    [SerializeField]
    private float armorIgnoreBonus;
    [SerializeField]
    private float skillIssue;
    #endregion

    private PlayerInventory playerInventory;
    private List<Item> armors;
    private List<Item> equipment;


    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = new()
        {
            { "hp", hp },
            { "stamina", stamina },
            { "mana", mana },
            { "hpMax", hpMax },
            { "staminaMax", staminaMax },
            { "manaMax", manaMax },
            { "armor", armor },
            { "evasion", evasion },
            { "weight", weight },
            { "speed", speed },
            { "experience", experience },
            { "level", level },
            { "accuracyBonus", accuracyBonus },
            { "penetrationBonus", penetrationBonus },
            { "armorIgnoreBonus", armorIgnoreBonus },
            { "skillIssue", skillIssue },
        };
    }

    public void UpdateEquipment()
    {
        armors = new();
        equipment = new();
        Dictionary<string, float> baseStats = new()
        {
            { "hp", hp },
            { "stamina", stamina },
            { "mana", mana },
            { "hpMax", hpMax },
            { "staminaMax", staminaMax },
            { "manaMax", manaMax },
            { "armor", armor },
            { "evasion", evasion },
            { "weight", weight },
            { "speed", speed },
            { "experience", experience },
            { "level", level },
            { "accuracyBonus", accuracyBonus },
            { "penetrationBonus", penetrationBonus },
            { "armorIgnoreBonus", armorIgnoreBonus },
            { "skillIssue", skillIssue },
        };
        Dictionary<string, float> bonusStats = new()
        {
            { "hp", 0 },
            { "stamina", 0 },
            { "mana", 0 },
            { "hpMax", 0 },
            { "staminaMax", 0 },
            { "manaMax", 0 },
            { "armor", 0 },
            { "evasion", 0 },
            { "weight", 0 },
            { "speed", 0 },
            { "experience", 0 },
            { "level", 0 },
            { "accuracyBonus", 0 },
            { "penetrationBonus", 0 },
            { "armorIgnoreBonus", 0 },
            { "skillIssue", 0 },
        };


        // Updating Lists
        for (int i = 0; i < playerInventory.armorSlots.transform.childCount; i++)
            if (playerInventory.armorSlots.transform.GetChild(i).childCount > 0 && playerInventory.armorSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
                armors.Add(item);
        for (int i = 0; i < playerInventory.equipmentSlots.transform.childCount; i++)
            if (playerInventory.equipmentSlots.transform.GetChild(i).childCount > 0 && playerInventory.equipmentSlots.transform.GetChild(i).TryGetComponent(out Item item))
                equipment.Add(item);

        // Updating stats
        if (armors.Count > 0)
            foreach (Item item in armors)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];
        if (equipment.Count > 0)
            foreach (Item item in equipment)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];

        // Send all stats to PlayerStats
        foreach (string key in baseStats.Keys)
            playerStats[key] = baseStats[key] + bonusStats[key];

        GetComponent<PlayerGFXManager>().UpdateGFX();
    }

    public void LoadData(GameData data)
    {
        playerStats.Clear();
        foreach(string key in data.playerStats.Keys)
            playerStats.Add(key, data.playerStats[key]);
    }

    public void SaveData(ref GameData data)
    {
        data.playerStats.Clear();
        foreach(string key in playerStats.Keys)
            data.playerStats.Add(key, playerStats[key]);
    }
}
