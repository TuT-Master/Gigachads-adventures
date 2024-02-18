using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerStats : MonoBehaviour, IDataPersistance
{
    public Dictionary<string, float> playerStats;

    [Header("Default stats setup")]
    #region Default stats setup
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
    private float hpRegen;
    [SerializeField]
    private float staminaRegen;
    [SerializeField]
    private float manaRegen;
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
    [SerializeField]
    private float backpackSize;
    [SerializeField]
    private float beltSize;
    [SerializeField]
    private float pocketSize;
    #endregion

    private PlayerInventory playerInventory;
    private List<Item> armors;
    private List<Item> equipment;

    [SerializeField]
    private GameObject backpackSlot;
    [SerializeField]
    private GameObject beltSlot;

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
            { "hpRegen", hpRegen },
            { "staminaRegen", staminaRegen },
            { "manaRegen", manaRegen },
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
            { "backpackSize", backpackSize },
            { "beltSize", beltSize },
            { "pocketSize", pocketSize },
        };
    }

    void Update()
    {
        playerStats["hp"] += playerStats["hpRegen"] * Time.deltaTime * 10;
        playerStats["stamina"] += playerStats["staminaRegen"] * Time.deltaTime * 10;
        playerStats["mana"] += playerStats["manaRegen"] * Time.deltaTime * 10;

        if (playerStats["hp"] >= playerStats["hpMax"])
            playerStats["hp"] = playerStats["hpMax"];
        if (playerStats["stamina"] >= playerStats["staminaMax"])
            playerStats["stamina"] = playerStats["staminaMax"];
        if (playerStats["mana"] >= playerStats["manaMax"])
            playerStats["mana"] = playerStats["manaMax"];
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
            { "hpRegen", hpRegen },
            { "staminaRegen", staminaRegen },
            { "manaRegen", manaRegen },
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
            { "backpackSize", backpackSize },
            { "beltSize", beltSize },
            { "pocketSize", pocketSize },
        };
        Dictionary<string, float> bonusStats = new()
        {
            { "hp", 0 },
            { "stamina", 0 },
            { "mana", 0 },
            { "hpMax", 0 },
            { "staminaMax", 0 },
            { "manaMax", 0 },
            { "hpRegen", 1 },
            { "staminaRegen", 1 },
            { "manaRegen", 1 },
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
            { "backpackSize", 0 },
            { "beltSize", 0 },
            { "pocketSize", 0 },
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

        // Backpack
        if (backpackSlot.transform.childCount > 0)
            bonusStats["backpackSize"] = backpackSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;

        // Belt
        if (beltSlot.transform.childCount > 0)
            bonusStats["beltSize"] = beltSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;

        // Pockets


        // Send all stats to PlayerStats
        foreach (string key in baseStats.Keys)
            playerStats[key] = baseStats[key] + bonusStats[key];


        GetComponent<PlayerGFXManager>().UpdateGFX();
    }


    IEnumerator UpdateGFXDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerGFXManager>().UpdateGFX();
    }
    public void LoadData(GameData data)
    {
        playerStats.Clear();
        foreach(string key in data.playerStats.Keys)
            playerStats.Add(key, data.playerStats[key]);
        StartCoroutine(UpdateGFXDelay());
    }
    public void SaveData(ref GameData data)
    {
        data.playerStats.Clear();
        foreach(string key in playerStats.Keys)
            data.playerStats.Add(key, playerStats[key]);
    }
}
