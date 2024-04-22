using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDataPersistance
{
    public Dictionary<string, float> playerStats;

    public Dictionary<string, float> playerBonusStats = new()
        {
            {"damage", 0 },
            {"accuracyBonus", 0 },
            {"penetration", 0 },
            {"armorIngore", 0 },
            {"bleedingChance", 0 },
            {"bleedingDamage", 0 },
            {"poisonChance", 0 },
            {"poisonDamage", 0 },
            {"stunChance", 0 },
            {"range", 0 },
            {"attackSpeed", 0 },
            {"critChance", 0 },
            {"notConsumeStaminaChance", 0 },
            {"staminaConsumtionReduction", 0 },
            {"evade", 0 },
        };


    [Header("Player default stats")]
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
    private int skillPoints;
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
    private PlayerMovement playerMovement;

    private List<Item> armors;
    private List<Item> equipment;
    private List<Item> backpackInventory;

    [SerializeField]
    private GameObject backpackSlot;
    [SerializeField]
    private GameObject beltSlot;

    private bool canRegenerateHp;
    private bool canRegenerateStamina;
    private bool canRegenerateMana;
    private bool getsDamage;


    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerMovement = GetComponent<PlayerMovement>();


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
            { "skillPoints", skillPoints },
            { "skillIssue", skillIssue },
            { "backpackSize", backpackSize },
            { "beltSize", beltSize },
            { "pocketSize", pocketSize },
        };
        UpdateStats();


        canRegenerateHp = true;
        canRegenerateStamina = true;
        canRegenerateMana = true;
    }

    void Update()
    {
        if (playerStats["hp"] <= 0)
        {
            // Kill player and increase skill issue
            playerStats["skillIssue"]++;

            // Teleport player home
            FindAnyObjectByType<VirtualSceneManager>().LoadScene("Home");

            // Refill stats
            playerStats["hp"] = playerStats["hpMax"];
            playerStats["stamina"] = playerStats["staminaMax"];
            playerStats["mana"] = playerStats["manaMax"];
        }

        // Checking whether can regen stats or not
        if(CanRegenStats())
        {
            canRegenerateHp = true;
            canRegenerateStamina = true;
            canRegenerateMana = true;
        }
        else
        {
            canRegenerateHp = false;
            canRegenerateStamina = false;
            canRegenerateMana = false;
        }

        if (playerMovement.sprint)
            playerStats["stamina"] -= 10 * Time.deltaTime;

        // Regen stats
        if (canRegenerateHp)
            playerStats["hp"] += playerStats["hpRegen"] * Time.deltaTime * 2;
        if(canRegenerateStamina)
            playerStats["stamina"] += playerStats["staminaRegen"] * Time.deltaTime * 5;
        if (canRegenerateMana)
            playerStats["mana"] += playerStats["manaRegen"] * Time.deltaTime * 5;

        // Stops regen at max values
        if (playerStats["hp"] >= playerStats["hpMax"])
            playerStats["hp"] = playerStats["hpMax"];
        if (playerStats["stamina"] >= playerStats["staminaMax"])
            playerStats["stamina"] = playerStats["staminaMax"];
        if (playerStats["mana"] >= playerStats["manaMax"])
            playerStats["mana"] = playerStats["manaMax"];

        UpdateEquipment();
    }

    private bool CanRegenStats()
    {
        if (playerMovement.sprint || getsDamage || !GetComponent<PlayerFight>().canAttackAgain)
            return false;
        return true;
    }

    public void DealDamage(float damage, float penetration, float armorIgnore)
    {
        canRegenerateHp = false;
        canRegenerateStamina = false;
        canRegenerateMana = false;

        float finalDamage = damage;
        if (finalDamage > 0)
        {
            getsDamage = true;
            StartCoroutine(StatRegen());
        }

        playerStats["hp"] -= finalDamage;
    }

    IEnumerator StatRegen()
    {
        StopCoroutine(StatRegen());

        yield return new WaitForSeconds(3);

        getsDamage = false;
    }

    public void UpdateEquipment()
    {
        armors = new();
        equipment = new();
        backpackInventory = new();
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
            { "skillPoints", skillPoints },
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
            { "skillPoints", 0 },
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
            if (playerInventory.equipmentSlots.transform.GetChild(i).childCount > 0 && playerInventory.equipmentSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
                equipment.Add(item);
        for (int i = 0; i < playerInventory.backpackInventory.transform.childCount; i++)
            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0 && playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
                backpackInventory.Add(item);

        // Armor
        if (armors.Count > 0)
            foreach (Item item in armors)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];

        // Equipment
        if (equipment.Count > 0)
            foreach (Item item in equipment)
                foreach (string key in item.armorStats.Keys)
                    bonusStats[key] += item.armorStats[key];

        // Backpack
        if (backpackSlot.transform.childCount > 0)
        {
            bonusStats["backpackSize"] = backpackSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;
            bonusStats["weight"] += backpackSlot.transform.GetChild(0).GetComponent<Item>().stats["weight"];
        }

        // Backpack inventory
        if (backpackInventory.Count > 0)
        {
            foreach (Item item in backpackInventory)
            {
                if(item.stats != null)
                    bonusStats["weight"] += item.stats["weight"] * item.amount;
                else
                    bonusStats["weight"] += item.armorStats["weight"] * item.amount;
            }
        }

        // Belt
        if (beltSlot.transform.childCount > 0)
        {
            bonusStats["beltSize"] = beltSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;
            bonusStats["weight"] += beltSlot.transform.GetChild(0).GetComponent<Item>().stats["weight"];
        }

        // Pockets


        // Send all stats to PlayerStats
        foreach (string key in baseStats.Keys)
            playerStats[key] = baseStats[key] + bonusStats[key];


        GetComponent<PlayerGFXManager>().UpdateGFX();
    }
    public void UpdateStats()
    {
        foreach(Skill skill in FindObjectsByType<Skill>(FindObjectsSortMode.None))
            foreach (string key in skill.bonusStats.Keys)
                playerBonusStats[key] += skill.bonusStats[key];

        foreach (string key in playerBonusStats.Keys)
        {
            Debug.Log(key + ": " + playerBonusStats[key]);
        }
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
