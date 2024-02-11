using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<string, float> playerStats;

    public float hp;
    public float stamina;
    public float mana;
    public float hpMax;
    public float staminaMax;
    public float manaMax;
    
    public float armor;
    public float evasion;

    public float weight;
    public float speed;

    public int experience;
    public int level;

    public float accuracyBonus;
    public float penetrationBonus;
    public float armorIgnoreBonus;

    public float skillIssue;


    private PlayerInventory playerInventory;
    private List<Item> armors = new();
    private List<Item> equipment = new();


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

    private void Update()
    {
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
        if(armors.Count > 0)
            foreach (Item item in armors)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];
        if (equipment.Count > 0)
            foreach (Item item in equipment)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];

        // Send all stats to PlayerStats

        foreach (string key in baseStats.Keys)
        {
            playerStats[key] = baseStats[key] + bonusStats[key];
            if(bonusStats[key] > 0)
                Debug.Log(playerStats[key].ToString());
        }
    }

    public void UpdateEquipment()
    {

    }
}
