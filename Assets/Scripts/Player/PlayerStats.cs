using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDataPersistance
{
    public enum WeaponClass
    {
        None,
        OneHandDexterity,
        OneHandStrenght,
        TwoHandDexterity,
        TwoHandStrenght,
        RangeDexterity,
        RangeStrenght,
        MagicAir,
        MagicFire,
        MagicWater,
        MagicEarth,
        MagicLight,
        MagicDark,
        Projectile,
    }
    public enum StatType
    {
        // Player stats
        Hp,
        Stamina,
        Mana,
        HpMax,
        StaminaMax,
        ManaMax,
        HpRegen,
        StaminaRegen,
        ManaRegen,
        Armor,
        IncreaseArmorByPercentage,
        Player_Defense,
        BonusDefense,
        Evade,
        MagicResistance,
        BleedingResistance,
        PoisonResistance,
        Weight,
        Speed,
        BackpackSize,
        BeltSize,
        PocketSize,
        Age,
        Player_Exp,
        Player_Level,
        SkillPoints,
        SkillIssuePoints,
        Price,
        AdditionalInventorySlots,
        Knockback,
        ArmorIgnore,
        // Skill stats
        NotConsumeStaminaChance,
        StaminaConsumptionReduction,
        AccuracyBonus,
        StunChance,
        BleedingChance,
        PoisonChance,
        Range,
        // Item stats
        Damage,
        PoisonDamage,
        BleedingDamage,
        BurningChance,
        Penetration,
        AttackSpeed,
        CritDamage,
        CritChance,
        StaminaCost,
        ManaCost,
        RangeX,
        RangeY,
        CurrentMagazine,
        MagazineSize,
        ReloadTime,
        Spread,
        SplashDamage,
        SplashRadius,
        Piercing,
        ProjectileSpeed,
        Cooldown,
    }

    [SerializeField] private Transform respawnPoint;

    // Skill bonuses
    private Dictionary<WeaponClass, Skill> skillLevels = new()
    {
        { WeaponClass.OneHandDexterity, new Skill(0, 0, 50) },
        { WeaponClass.OneHandStrenght, new Skill(0, 0, 50) },
        { WeaponClass.TwoHandDexterity, new Skill(0, 0, 50) },
        { WeaponClass.TwoHandStrenght, new Skill(0, 0, 50) },
        { WeaponClass.RangeDexterity, new Skill(0, 0, 50) },
        { WeaponClass.RangeStrenght, new Skill(0, 0, 50) },
        { WeaponClass.MagicAir, new Skill(0, 0, 50) },
        { WeaponClass.MagicFire, new Skill(0, 0, 50) },
        { WeaponClass.MagicWater, new Skill(0, 0, 50) },
        { WeaponClass.MagicEarth, new Skill(0, 0, 50) },
        { WeaponClass.MagicLight, new Skill(0, 0, 50) },
        { WeaponClass.MagicDark, new Skill(0, 0, 50) },
    };
    public Dictionary<WeaponClass, Dictionary<StatType, float>> bonusesFromSkills = new()
    {
        {WeaponClass.OneHandDexterity, new Dictionary<StatType, float>() },
        {WeaponClass.OneHandStrenght, new Dictionary<StatType, float>() },
        {WeaponClass.TwoHandDexterity, new Dictionary<StatType, float>() },
        {WeaponClass.TwoHandStrenght, new Dictionary<StatType, float>() },
        {WeaponClass.RangeDexterity, new Dictionary<StatType, float>() },
        {WeaponClass.RangeStrenght, new Dictionary<StatType, float>() },
        {WeaponClass.MagicAir, new Dictionary<StatType, float>() },
        {WeaponClass.MagicFire, new Dictionary<StatType, float>() },
        {WeaponClass.MagicWater, new Dictionary<StatType, float>() },
        {WeaponClass.MagicEarth, new Dictionary<StatType, float>() },
        {WeaponClass.MagicLight, new Dictionary<StatType, float>() },
        {WeaponClass.MagicDark, new Dictionary<StatType, float>() },
    };

    [Header("Player default stats")]
    public Dictionary<StatType, float> playerStats;
    public Dictionary<StatType, float> playerBaseStats;

    [Header("UI message object")]
    [SerializeField] GameObject messageObj;

    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    private PlayerFight playerFight;

    private List<Item> armors;
    private List<Item> equipment;
    private List<Item> backpackInventory;

    [SerializeField] private GameObject backpackSlot;
    [SerializeField] private GameObject beltSlot;

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


        playerBaseStats = FindAnyObjectByType<DataPersistanceManager>().gameData.baseStats;
        foreach(WeaponClass weaponClass in System.Enum.GetValues(typeof(WeaponClass)))
            if (bonusesFromSkills.ContainsKey(weaponClass))
                bonusesFromSkills[weaponClass] = new()
                {
                    {StatType.Damage, 0 },
                    {StatType.AccuracyBonus, 0 },
                    {StatType.Penetration, 0 },
                    {StatType.ArmorIgnore, 0 },
                    {StatType.BleedingChance, 0 },
                    {StatType.BleedingDamage, 0 },
                    {StatType.PoisonChance, 0 },
                    {StatType.PoisonDamage, 0 },
                    {StatType.StunChance, 0 },
                    {StatType.Range, 0 },
                    {StatType.AttackSpeed, 0 },
                    {StatType.CritChance, 0 },
                    {StatType.Knockback, 0 },
                    {StatType.IncreaseArmorByPercentage, 0 },
                    {StatType.NotConsumeStaminaChance, 0 },
                    {StatType.StaminaConsumptionReduction, 0 },
                    {StatType.Evade, 0 },
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

        if (playerStats[StatType.Hp] <= 0)
        {
            // Show dead message
            ShowMessage("You died", 1.5f);

            // Increase skill issue
            playerStats[StatType.SkillIssuePoints]++;

            // Empty player inventory


            // Teleport player home
            transform.position = respawnPoint.position;

            // Refill stats
            playerStats[StatType.Hp] = playerStats[StatType.HpMax];
            playerStats[StatType.Stamina] = playerStats[StatType.StaminaMax];
            playerStats[StatType.ManaMax] = playerStats[StatType.ManaMax];

            // Reset exp

        }

        // Checking whether can regen stats or not
        if (CanRegenStats())
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
        if (playerStats[StatType.Hp] > playerStats[StatType.HpMax])
            playerStats[StatType.Hp] = playerStats[StatType.HpMax];
        if (playerStats[StatType.Stamina] > playerStats[StatType.StaminaMax])
            playerStats[StatType.Stamina] = playerStats[StatType.StaminaMax];
        if (playerStats[StatType.ManaMax] > playerStats[StatType.ManaMax])
            playerStats[StatType.Mana] = playerStats[StatType.ManaMax];
    }
    void FixedUpdate()
    {
        if (playerStats == null)
            return;

        // Sprint
        if (playerMovement.sprint)
            playerStats[StatType.Stamina] -= 10 * Time.fixedDeltaTime;

        // Regen stats
        if (canRegenerateHp)
            playerStats[StatType.Hp] += playerStats[StatType.HpRegen] * Time.fixedDeltaTime * 0.5f;
        if (canRegenerateStamina)
            playerStats[StatType.Stamina] += playerStats[StatType.StaminaRegen] * Time.fixedDeltaTime * 5;
        if (canRegenerateMana)
            playerStats[StatType.Mana] += playerStats[StatType.ManaRegen] * Time.fixedDeltaTime * 5;
    }



    public void AddExp(Item weapon, float exp)
    {
        skillLevels[weapon.weaponClass].AddExperience(exp, out bool levelUp);
        Debug.Log($"Item {weapon.itemName}, weaponClass {weapon.weaponClass}, progress for level up {skillLevels[weapon.weaponClass].ProgressToNextLevel() * 100}%");
        if (levelUp)
        {
            Debug.Log("Level up!");

            // Some animation?


            // Some sound?
        }
    }

    private bool CanRegenStats()
    {
        if (playerMovement.sprint || getsDamage || !playerFight.canAttackAgain || playerFight.defending)
            return false;
        return true;
    }

    public void DealDamage(float damage, float penetration, float armorIgnore)
    {
        float finalDamage = damage;
        float armor = playerStats[StatType.Armor];

        if (playerFight.defending && playerStats[StatType.Player_Defense] > 0)
        {
            finalDamage *= (100 - playerStats[StatType.Player_Defense]) / 100;
            playerStats[StatType.Stamina] -= 10;
        }

        if (armorIgnore > 0)
            armor *= armorIgnore;

        if (armor - penetration > 0)
            finalDamage -= armor - penetration;


        if (finalDamage > 0)
        {
            getsDamage = true;
            playerStats[StatType.Hp] -= finalDamage;

            canRegenerateHp = false;
            canRegenerateStamina = false;
            canRegenerateMana = false;

            StartCoroutine(StatRegen());
        }
    }
    private IEnumerator StatRegen()
    {
        StopCoroutine(StatRegen());

        yield return new WaitForSeconds(3f);

        getsDamage = false;
    }

    public Dictionary<StatType, float> GetSkillBonusStats(Item.MagicCrystalType crystalType, float multiplier)
    {
        Dictionary<StatType, float> magicBonuses = new();
        switch(crystalType)
        {
            case Item.MagicCrystalType.Fire:
                foreach(StatType key in bonusesFromSkills[WeaponClass.MagicFire].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicFire][key] * multiplier);
                break;
            case Item.MagicCrystalType.Water:
                foreach (StatType key in bonusesFromSkills[WeaponClass.MagicWater].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicWater][key] * multiplier);
                break;
            case Item.MagicCrystalType.Air:
                foreach (StatType key in bonusesFromSkills[WeaponClass.MagicAir].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicAir][key] * multiplier);
                break;
            case Item.MagicCrystalType.Earth:
                foreach (StatType key in bonusesFromSkills[WeaponClass.MagicEarth].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicEarth][key] * multiplier);
                break;
            case Item.MagicCrystalType.Light:
                foreach (StatType key in bonusesFromSkills[WeaponClass.MagicLight].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicLight][key] * multiplier);
                break;
            case Item.MagicCrystalType.Dark:
                foreach (StatType key in bonusesFromSkills[WeaponClass.MagicDark].Keys)
                    magicBonuses.Add(key, bonusesFromSkills[WeaponClass.MagicDark][key] * multiplier);
                break;
        }

        return magicBonuses;
    }

    public void UpdateEquipment()
    {
        if (!loaded)
            return;
        armors = new();
        equipment = new();
        backpackInventory = new();
        playerBaseStats = FindAnyObjectByType<DataPersistanceManager>().gameData.baseStats;
        Dictionary<StatType, float> bonusStats = new();
        foreach (StatType stat in playerBaseStats.Keys)
            bonusStats.Add(stat, 0);


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
                foreach (StatType key in item.stats.Keys)
                    if(key != StatType.Price)
                        bonusStats[key] += item.stats[key];

        // Equipment
        if (equipment.Count > 0)
            foreach (Item item in equipment)
                foreach (StatType key in item.stats.Keys)
                    bonusStats[key] += item.stats[key];

        // Backpack
        if (backpackSlot.transform.childCount > 0)
        {
            bonusStats[StatType.BackpackSize] = backpackSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;
            bonusStats[StatType.Weight] += backpackSlot.transform.GetChild(0).GetComponent<Item>().stats[StatType.Weight];
        }

        // Backpack inventory
        if (backpackInventory.Count > 0)
        {
            foreach (Item item in backpackInventory)
            {
                if(item.stats != null)
                    bonusStats[StatType.Weight] += item.stats[StatType.Weight] * item.amount;
                else
                    bonusStats[StatType.Weight] += item.stats[StatType.Weight] * item.amount;
            }
        }

        // Belt
        if (beltSlot.transform.childCount > 0)
        {
            bonusStats[StatType.BeltSize] = beltSlot.transform.GetChild(0).GetComponent<Item>().inventoryCapacity;
            bonusStats[StatType.Weight] += beltSlot.transform.GetChild(0).GetComponent<Item>().stats[StatType.Weight];
        }

        // Pockets


        // Defense
        if (playerFight.secondaryItemInHand != null && playerFight.secondaryItemInHand.stats.ContainsKey(StatType.Player_Defense))
            bonusStats[StatType.Player_Defense] += playerFight.secondaryItemInHand.stats[StatType.Player_Defense];
        else if (playerFight.activeWeapon != null && playerFight.activeWeapon.stats.ContainsKey(StatType.Player_Defense))
            bonusStats[StatType.Player_Defense] += playerFight.activeWeapon.stats[StatType.Player_Defense];

        // Send all stats to PlayerStats
        foreach (StatType key in bonusStats.Keys)
            playerStats[key] = playerBaseStats[key] + bonusStats[key];
    }
    public void UpdateSkillBonusStats()
    {
        // Reset lists
        foreach (WeaponClass weaponClass in System.Enum.GetValues(typeof(WeaponClass)))
            if (bonusesFromSkills.ContainsKey(weaponClass))
                bonusesFromSkills[weaponClass] = new()
                {
                    {StatType.Damage, 0 },
                    {StatType.AccuracyBonus, 0 },
                    {StatType.Penetration, 0 },
                    {StatType.ArmorIgnore, 0 },
                    {StatType.BleedingChance, 0 },
                    {StatType.BleedingDamage, 0 },
                    {StatType.PoisonChance, 0 },
                    {StatType.PoisonDamage, 0 },
                    {StatType.StunChance, 0 },
                    {StatType.Range, 0 },
                    {StatType.AttackSpeed, 0 },
                    {StatType.CritChance, 0 },
                    {StatType.Knockback, 0 },
                    {StatType.IncreaseArmorByPercentage, 0 },
                    {StatType.NotConsumeStaminaChance, 0 },
                    {StatType.StaminaConsumptionReduction, 0 },
                    {StatType.Evade, 0 },
                };

        // Update new values
        foreach (LearnableSkill skill in FindObjectsByType<LearnableSkill>(FindObjectsSortMode.None))
            foreach (StatType statType in skill.bonusStats.Keys)
                bonusesFromSkills[skill.weaponClass][statType] += skill.bonusStats[statType];
    }


    // Message list 'Room cleared!', 'You died', 'Boss defeated' etc.
    public void ShowMessage(string message, float delay)
    {
        messageObj.SetActive(true);
        messageObj.GetComponentInChildren<TextMeshProUGUI>().text = message;
        StartCoroutine(HideMessageDelay(delay));
    }
    private IEnumerator HideMessageDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        messageObj.SetActive(false);
    }


    // Save & Load
    public void LoadData(GameData data)
    {
        playerStats = new();
        foreach(StatType key in data.playerStats.Keys)
            playerStats.Add(key, data.playerStats[key]);
        loaded = true;
    }
    public void SaveData(ref GameData data)
    {
        data.playerStats.Clear();
        playerStats ??= playerBaseStats;
        foreach (StatType key in playerStats.Keys)
            data.playerStats.Add(key, playerStats[key]);
    }
}


public class Skill
{
    public int Level { get; private set; }
    public float Experience { get; private set; }

    private float experienceForLevelUp;
    private const float levelUpMultiplier = 1.25f;

    public Skill(int level, float experience, float experienceForFirstLevelUp)
    {
        Level = level;
        Experience = experience;
        experienceForLevelUp = experienceForFirstLevelUp;
    }

    public void AddExperience(float exp, out bool levelUp)
    {
        Experience += exp;
        levelUp = false;
        if (Experience >= experienceForLevelUp)
        {
            Experience -= experienceForLevelUp;
            Level++;
            levelUp = true;

            // Calculate new exp for level up
            experienceForLevelUp = (float)System.Math.Round(experienceForLevelUp * levelUpMultiplier, 2);
        }
    }


    public float ProgressToNextLevel()
    {
        return Mathf.Clamp01(Experience / experienceForLevelUp);
    }
}
