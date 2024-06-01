using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDataPersistance
{
    public Dictionary<string, float> playerStats;
    public Dictionary<string, float> playerStats_default;

    // Skill bonuses
    public Dictionary <string, float> playerSkillBonusStats_OneHandedStrength;
    public Dictionary <string, float> playerSkillBonusStats_OneHandedDexterity;
    public Dictionary <string, float> playerSkillBonusStats_TwoHandedStrength;
    public Dictionary <string, float> playerSkillBonusStats_TwoHandedDexterity;
    public Dictionary<string, float> playerSkillBonusStats_RangedStrength;
    public Dictionary<string, float> playerSkillBonusStats_RangedDexterity;
    public Dictionary<string, float> playerSkillBonusStats_MagicFire;
    public Dictionary<string, float> playerSkillBonusStats_MagicWater;
    public Dictionary<string, float> playerSkillBonusStats_MagicEarth;
    public Dictionary<string, float> playerSkillBonusStats_MagicAir;

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
    private float evade;
    [SerializeField]
    private float defense;
    [SerializeField]
    private float weight;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int exp_player;
    [SerializeField]
    private int level_player;
    [SerializeField]
    private int exp_oneHandDexterity;
    [SerializeField]
    private int level_oneHandDexterity;
    [SerializeField]
    private int exp_oneHandStrenght;
    [SerializeField]
    private int level_oneHandStrenght;
    [SerializeField]
    private int exp_twoHandDexterity;
    [SerializeField]
    private int level_twoHandDexterity;
    [SerializeField]
    private int exp_twoHandStrenght;
    [SerializeField]
    private int level_twoHandStrenght;
    [SerializeField]
    private int exp_rangedDexterity;
    [SerializeField]
    private int level_rangedDexterity;
    [SerializeField]
    private int exp_rangedStrenght;
    [SerializeField]
    private int level_rangedStrenght;
    [SerializeField]
    private int exp_magicFire;
    [SerializeField]
    private int level_magicFire;
    [SerializeField]
    private int exp_magicAir;
    [SerializeField]
    private int level_magicAir;
    [SerializeField]
    private int exp_magicWater;
    [SerializeField]
    private int level_magicWater;
    [SerializeField]
    private int exp_magicEarth;
    [SerializeField]
    private int level_magicEarth;
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

    public enum WeaponClass
    {
        None,
        OneHandDexterity,
        OneHandStrenght,
        TwoHandDexterity,
        TwoHandStrenght,
        RangeDexterity,
        RangeStrenght,
        Magic
    }



    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    private PlayerFight playerFight;

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

    private bool loaded;



    private void Start()
    {
        Application.targetFrameRate = 69;
        playerInventory = GetComponent<PlayerInventory>();
        playerMovement = GetComponent<PlayerMovement>();
        playerFight = GetComponent<PlayerFight>();


        playerStats_default = new()
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
            { "evade", evade },
            { "defense", defense },
            { "weight", weight },
            { "speed", speed },
            { "exp_player", exp_player },
            { "level_player", level_player },
            { "exp_oneHandDexterity", exp_oneHandDexterity },
            { "level_oneHandDexterity", level_oneHandDexterity },
            { "exp_oneHandStrenght", exp_oneHandStrenght },
            { "level_oneHandStrenght", level_oneHandStrenght },
            { "exp_twoHandDexterity", exp_twoHandDexterity },
            { "level_twoHandDexterity", level_twoHandDexterity },
            { "exp_twoHandStrenght", exp_twoHandStrenght },
            { "level_twoHandStrenght", level_twoHandStrenght },
            { "exp_rangedDexterity", exp_rangedDexterity },
            { "level_rangedDexterity", level_rangedDexterity },
            { "exp_rangedStrenght", exp_rangedStrenght },
            { "level_rangedStrenght", level_rangedStrenght },
            { "exp_magicFire", exp_magicFire },
            { "level_magicFire", level_magicFire },
            { "exp_magicAir", exp_magicAir },
            { "level_magicAir", level_magicAir },
            { "exp_magicWater", exp_magicWater },
            { "level_magicWater", level_magicWater },
            { "exp_magicEarth", exp_magicEarth },
            { "level_magicEarth", level_magicEarth },
            { "skillPoints", skillPoints },
            { "skillIssue", skillIssue },
            { "backpackSize", backpackSize },
            { "beltSize", beltSize },
            { "pocketSize", pocketSize },
        };
        playerSkillBonusStats_OneHandedDexterity = playerSkillBonusStats_OneHandedStrength =
        playerSkillBonusStats_TwoHandedDexterity = playerSkillBonusStats_TwoHandedStrength =
        playerSkillBonusStats_RangedDexterity = playerSkillBonusStats_RangedStrength =
        playerSkillBonusStats_MagicAir = playerSkillBonusStats_MagicEarth =
        playerSkillBonusStats_MagicWater = playerSkillBonusStats_MagicFire = new()
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
        UpdateSkillBonusStats();


        canRegenerateHp = true;
        canRegenerateStamina = true;
        canRegenerateMana = true;
    }
    void Update()
    {
        if (playerStats == null)
            return;

        UpdateEquipment();

        if (playerStats["hp"] <= 0)
        {
            // Increase skill issue
            playerStats["skillIssue"]++;

            // Empty player inventory


            // Teleport player home
            FindAnyObjectByType<VirtualSceneManager>().LoadScene("Home");

            // Refill stats
            playerStats["hp"] = playerStats["hpMax"];
            playerStats["stamina"] = playerStats["staminaMax"];
            playerStats["mana"] = playerStats["manaMax"];

            // Reset exp

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

        // Stops regen at max values
        if (playerStats["hp"] > playerStats["hpMax"])
            playerStats["hp"] = playerStats["hpMax"];
        if (playerStats["stamina"] > playerStats["staminaMax"])
            playerStats["stamina"] = playerStats["staminaMax"];
        if (playerStats["mana"] > playerStats["manaMax"])
            playerStats["mana"] = playerStats["manaMax"];

        // Checking for level up
        CheckForLevelUp();
    }
    void FixedUpdate()
    {
        if (playerStats == null)
            return;

        // Sprint
        if (playerMovement.sprint)
            playerStats["stamina"] -= 10 * Time.fixedDeltaTime;
        // Regen stats
        if (canRegenerateHp)
            playerStats["hp"] += playerStats["hpRegen"] * Time.fixedDeltaTime * 0.5f;
        if (canRegenerateStamina)
            playerStats["stamina"] += playerStats["staminaRegen"] * Time.fixedDeltaTime * 5;
        if (canRegenerateMana)
            playerStats["mana"] += playerStats["manaRegen"] * Time.fixedDeltaTime * 5;
    }

    readonly Dictionary<string, string> levelExpPairs = new()
    {
        { "level_oneHandDexterity", "exp_oneHandDexterity" },
        { "level_oneHandStrenght", "exp_oneHandStrenght" },
        { "level_twoHandDexterity", "exp_twoHandDexterity" },
        { "level_twoHandStrenght", "exp_twoHandStrenght" },
        { "level_rangedDexterity", "exp_rangedDexterity" },
        { "level_rangedStrenght", "exp_rangedStrenght" },
        { "level_magicFire", "exp_magicFire" },
        { "level_magicAir", "exp_magicAir" },
        { "level_magicWater", "exp_magicWater" },
        { "level_magicEarth", "exp_magicEarth" },
    };
    private void CheckForLevelUp()
    {
        foreach (string level in levelExpPairs.Keys)
            if (playerStats[levelExpPairs[level]] >= 20 + (playerStats[level] * 5))
            {
                playerStats[levelExpPairs[level]] -= 20 + (playerStats[level] * 5);
                playerStats[level]++;
                playerStats["skillPoints"]++;
                Debug.Log(level + " increased to level " + playerStats[level]);
                Debug.Log("For next level you need " + (20 + (playerStats[level] * 5) - playerStats[levelExpPairs[level]]) + " more EXPs");
            }
    }

    public void AddExp(Item weapon, float exp)
    {
        switch(weapon.weaponClass)
        {
            case WeaponClass.OneHandDexterity:
                playerStats["exp_oneHandDexterity"] += exp;
                break;
            case WeaponClass.OneHandStrenght:
                playerStats["exp_oneHandStrenght"] += exp;
                break;
            case WeaponClass.TwoHandDexterity:
                playerStats["exp_twoHandDexterity"] += exp;
                break;
            case WeaponClass.TwoHandStrenght:
                playerStats["exp_twoHandStrenght"] += exp;
                break;
            case WeaponClass.RangeDexterity:
                playerStats["exp_rangedDexterity"] += exp;
                break;
            case WeaponClass.RangeStrenght:
                playerStats["exp_rangedStrenght"] += exp;
                break;
            case WeaponClass.Magic:
                switch (weapon.weaponType)
                {
                    case Item.WeaponType.MagicWeapon_air:
                        playerStats["exp_magicAir"] += exp;
                        break;
                    case Item.WeaponType.MagicWeapon_fire:
                        playerStats["exp_magicFire"] += exp;
                        break;
                    case Item.WeaponType.MagicWeapon_water:
                        playerStats["exp_magicWater"] += exp;
                        break;
                    case Item.WeaponType.MagicWeapon_earth:
                        playerStats["exp_magicEarth"] += exp;
                        break;
                }
                break;
            default:
                Debug.Log("Developer (TuT) is kokot!");
                break;
        }
    }

    private bool CanRegenStats()
    {
        if (playerMovement.sprint || getsDamage || !GetComponent<PlayerFight>().canAttackAgain || playerFight.defending)
            return false;
        return true;
    }

    public void DealDamage(float damage, float penetration, float armorIgnore)
    {
        float finalDamage = damage;
        float armor = playerStats["armor"];

        if (armorIgnore > 0)
            armor *= armorIgnore;

        if (armor - penetration > 0)
            finalDamage -= armor - penetration;

        if (playerStats["defense"] > 0)
            finalDamage *= playerStats["defense"] / 100;

        if (finalDamage > 0)
        {
            if(playerFight.defending)
            {
                finalDamage *= 0.5f;
                playerStats["stamina"] -= 10;
            }
            getsDamage = true;
            playerStats["hp"] -= finalDamage;

            canRegenerateHp = false;
            canRegenerateStamina = false;
            canRegenerateMana = false;

            StartCoroutine(StatRegen());
        }
    }
    IEnumerator StatRegen()
    {
        StopCoroutine(StatRegen());

        yield return new WaitForSeconds(3);

        getsDamage = false;
    }

    public void UpdateEquipment()
    {
        if (!loaded)
            return;
        armors = new();
        equipment = new();
        backpackInventory = new();
        playerStats_default = new()
        {
            { "hpMax", hpMax },
            { "staminaMax", staminaMax },
            { "manaMax", manaMax },
            { "hpRegen", hpRegen },
            { "staminaRegen", staminaRegen },
            { "manaRegen", manaRegen },
            { "armor", armor },
            { "evade", evade },
            { "defense", defense },
            { "weight", weight },
            { "speed", speed },
            { "backpackSize", backpackSize },
            { "beltSize", beltSize },
            { "pocketSize", pocketSize },
        };
        Dictionary<string, float> bonusStats = new()
        {
            { "hpMax", 0 },
            { "staminaMax", 0 },
            { "manaMax", 0 },
            { "hpRegen", 0 },
            { "staminaRegen", 0 },
            { "manaRegen", 0 },
            { "armor", 0 },
            { "evade", 0 },
            { "defense", 0 },
            { "weight", 0 },
            { "speed", 0 },
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


        // Defense
        if (playerFight.secondaryItemInHand != null && playerFight.secondaryItemInHand.stats.ContainsKey("defense"))
            bonusStats["defense"] += playerFight.secondaryItemInHand.stats["defense"];
        else if (playerFight.itemInHand != null && playerFight.itemInHand.stats.ContainsKey("defense"))
            bonusStats["defense"] += playerFight.itemInHand.stats["defense"];

        // Send all stats to PlayerStats
        foreach (string key in bonusStats.Keys)
            playerStats[key] = playerStats_default[key] + bonusStats[key];

        GetComponent<PlayerGFXManager>().UpdateGFX();
    }
    public void UpdateSkillBonusStats()
    {
        // Reset lists
        playerSkillBonusStats_OneHandedDexterity = playerSkillBonusStats_OneHandedStrength =
        playerSkillBonusStats_TwoHandedDexterity = playerSkillBonusStats_TwoHandedStrength =
        playerSkillBonusStats_RangedDexterity = playerSkillBonusStats_RangedStrength =
        playerSkillBonusStats_MagicAir = playerSkillBonusStats_MagicEarth =
        playerSkillBonusStats_MagicWater = playerSkillBonusStats_MagicFire = new()
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

        // Update new values
        foreach (Skill skill in FindObjectsByType<Skill>(FindObjectsSortMode.None))
            foreach (string key in skill.bonusStats.Keys)
            {
                switch (skill.weaponClass)
                {
                    case WeaponClass.OneHandDexterity:
                        playerSkillBonusStats_OneHandedDexterity[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.OneHandStrenght:
                        playerSkillBonusStats_OneHandedStrength[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.TwoHandDexterity:
                        playerSkillBonusStats_TwoHandedDexterity[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.TwoHandStrenght:
                        playerSkillBonusStats_TwoHandedStrength[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.RangeDexterity:
                        playerSkillBonusStats_RangedDexterity[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.RangeStrenght:
                        playerSkillBonusStats_RangedStrength[key] += skill.bonusStats[key];
                        break;
                    case WeaponClass.Magic:
                        switch (skill.weaponType)
                        {
                            case Item.WeaponType.MagicWeapon_fire:
                                playerSkillBonusStats_MagicFire[key] += skill.bonusStats[key];
                                break;
                            case Item.WeaponType.MagicWeapon_water:
                                playerSkillBonusStats_MagicWater[key] += skill.bonusStats[key];
                                break;
                            case Item.WeaponType.MagicWeapon_air:
                                playerSkillBonusStats_MagicAir[key] += skill.bonusStats[key];
                                break;
                            case Item.WeaponType.MagicWeapon_earth:
                                playerSkillBonusStats_MagicEarth[key] += skill.bonusStats[key];
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
    }


    public void LoadData(GameData data)
    {
        playerStats = new();
        foreach(string key in data.playerStats.Keys)
            playerStats.Add(key, data.playerStats[key]);
        StartCoroutine(UpdateGFXDelay());
    }
    IEnumerator UpdateGFXDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerGFXManager>().UpdateGFX();
        loaded = true;
    }
    public void SaveData(ref GameData data)
    {
        data.playerStats.Clear();
        foreach(string key in playerStats.Keys)
            data.playerStats.Add(key, playerStats[key]);
    }
}
