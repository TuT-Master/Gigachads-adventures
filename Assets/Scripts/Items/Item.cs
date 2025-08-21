using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Dictionary<string, float> stats;
    public Dictionary<string, float> armorStats;

    public string itemName;
    [TextArea]
    public string description;

    public Slot.SlotType slotType;
    public enum WeaponType
    {
        NotAWeapon,
        // Melle ONE HANDED
        // Dexterity
        Whip,
        Dagger,
        Sword,
        Rapier,
        LightShield,
        // Strenght
        Axe,
        Mace,
        Hammer_oneHanded,
        HeavyShield,

        // Melle TWO HANDED
        // Dexterity
        QuarterStaff,
        Spear,
        Longsword,
        // Strenght
        Halbert,
        Hammer_twoHanded,
        Zweihander,

        // Range
        // Dexterity
        Bow,
        SMG,
        Pistol,
        AttackRifle,
        Thrower,
        // Strenght
        Longbow,
        Crossbow,
        Shotgun,
        Revolver,
        Machinegun,
        SniperRifle,
        Launcher,

        // Magic weapon
        MagicWeapon_fire,
        MagicWeapon_water,
        MagicWeapon_earth,
        MagicWeapon_air,
        MagicWeapon_light,
        MagicWeapon_dark,

        // Global
        Global,

        // Other
        Throwable,
        MagicWeapon,
        Trap,
    }
    public WeaponType weaponType;
    public PlayerStats.WeaponClass weaponClass;

    public bool isStackable;
    public int stackSize;

    public bool emitsLight;

    public bool fullAuto;

    public int amount = 1;

    public int inventoryCapacity;

    public bool twoHanded;
    public bool AoE;

    public Sprite sprite_inventory;
    public Sprite sprite_handMale_Front;
    public Sprite sprite_handMale_Back;
    public Sprite sprite_equipMale_Front;
    public Sprite sprite_equipMale_Back;
    public Sprite sprite_handFemale_Front;
    public Sprite sprite_handFemale_Back;
    public Sprite sprite_equipFemale_Front;
    public Sprite sprite_equipFemale_Back;

    public List<ProjectileSO> ammo;

    public bool hideHairWhenEquiped;
    public bool hideBeardWhenEquiped;
    public bool hideBodyWhenEquiped;

    public bool selfHoming;

    // Full-set bonus
    public Dictionary<string, float> fullSetBonus;

    // Crafting
    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public Dictionary<ItemSO, int> recipe;

    public bool isRecipe;

    // Upgrading
    public bool isUpgrade;
    public ScriptableObject upgradedVersionOfItem;

    // Magic crystals
    public enum MagicCrystalType
    {
        None = 0,
        Fire = 1,
        Water = 2,
        Air = 3,
        Earth = 4,
        Light = 5,
        Dark = 6,
    }
    public MagicCrystalType GetMagicCrystalTypeByInt(int index)
    {
        if (Enum.IsDefined(typeof(MagicCrystalType), index))
            return (MagicCrystalType)index;
        return MagicCrystalType.None;
    }
    public MagicCrystalType crystalType;
    public enum Spell
    {
        None,
        Fireball,
        Windblow,
        Watersplash,
        Stone,
        Lightning,
        Lifesteal,
    }
    public Spell spell;

    // Magic weapons
    public Dictionary<int, MagicCrystalType> magicCrystals;
    public Dictionary<MagicCrystalType, float> magicSkillBonuses;

    // Base upgrading
    public int requieredAge;
    public int levelOfUpgrade;
    public PlayerBase.BaseUpgrade baseUpgradeType;
    public BaseUpgradeSO nextLevel;

    // Amount text
    private TextMeshProUGUI text;


    public Item(WeaponMeleeSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.WeaponMelee;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_handMale_Front = weaponSO.sprite_handMale_Front;
        sprite_handMale_Back = weaponSO.sprite_handMale_Back;
        sprite_handFemale_Front = weaponSO.sprite_handFemale_Front;
        sprite_handFemale_Back = weaponSO.sprite_handFemale_Back;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
        weaponType = weaponSO.weaponType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        isUpgrade = weaponSO.isUpgrade;
    }
    public Item(WeaponRangedSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description= weaponSO.description;
        slotType = Slot.SlotType.WeaponRanged;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_handMale_Front = weaponSO.sprite_handMale_Front;
        sprite_handMale_Back = weaponSO.sprite_handMale_Back;
        sprite_handFemale_Front = weaponSO.sprite_handFemale_Front;
        sprite_handFemale_Back = weaponSO.sprite_handFemale_Back;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        stackSize = weaponSO.stackSize;
        emitsLight = weaponSO.emitsLight;
        fullAuto = weaponSO.fullAuto;
        ammo = weaponSO.ammo;
        weaponType = weaponSO.weaponType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        isUpgrade = weaponSO.isUpgrade;
    }
    public Item(WeaponMagicSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.MagicWeapon;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_handMale_Front = weaponSO.sprite_handMale_Front;
        sprite_handMale_Back = weaponSO.sprite_handMale_Back;
        sprite_handFemale_Front = weaponSO.sprite_handFemale_Front;
        sprite_handFemale_Back = weaponSO.sprite_handFemale_Back;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        stackSize = weaponSO.stackSize;
        emitsLight = weaponSO.emitsLight;
        fullAuto = weaponSO.fullAuto;
        weaponType = weaponSO.weaponType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        magicCrystals = weaponSO.magicCrystals;
        isUpgrade = weaponSO.isUpgrade;
    }
    public Item(ConsumableSO consumableSO)
    {
        itemName = consumableSO.itemName;
        description = consumableSO.description;
        slotType = Slot.SlotType.Consumable;
        sprite_inventory = consumableSO.sprite_inventory;
        sprite_handMale_Front = consumableSO.sprite_handMale_Front;
        sprite_handMale_Back = consumableSO.sprite_handMale_Back;
        sprite_handFemale_Front = consumableSO.sprite_handFemale_Front;
        sprite_handFemale_Back = consumableSO.sprite_handFemale_Back;
        isStackable = consumableSO.isStackable;
        stackSize = consumableSO.stackSize;
        stats = consumableSO.Stats();
        craftedIn = consumableSO.craftedIn;
        requieredCraftingLevel = consumableSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(ProjectileSO projectile)
    {
        slotType = Slot.SlotType.Ammo;
        itemName = projectile.itemName;
        description = projectile.description;
        sprite_inventory = projectile.sprite_inventory;
        sprite_equipMale_Front = projectile.sprite_projectile;
        stats = projectile.Stats();
        isStackable = true;
        stackSize = projectile.stackSize;
        craftedIn = projectile.craftedIn;
        requieredCraftingLevel = projectile.requieredCraftingLevel;
        recipe = new();
        selfHoming = projectile.selfHoming;
    }
    public Item(ArmorSO armorSO)
    {
        itemName = armorSO.itemName;
        description = armorSO.description;
        armorStats = armorSO.Stats();
        amount = 1;
        slotType = armorSO.slotType;
        sprite_inventory = armorSO.sprite_inventory;
        sprite_equipMale_Front = armorSO.sprite_equipMale_Front;
        sprite_equipMale_Back = armorSO.sprite_equipMale_Back;
        sprite_equipFemale_Front = armorSO.sprite_equipFemale_Front;
        sprite_equipFemale_Back = armorSO.sprite_equipFemale_Back;
        craftedIn = armorSO.craftedIn;
        requieredCraftingLevel = armorSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = armorSO.upgradedVersionsOfArmor;
        hideHairWhenEquiped = armorSO.hideHairWhenEquiped;
        hideBeardWhenEquiped = armorSO.hideBeardWhenEquiped;
        hideBodyWhenEquiped = armorSO.hideBodyWhenEquiped;
        fullSetBonus = armorSO.FullsetBonus();
        isUpgrade = armorSO.isUpgrade;
    }
    public Item(BackpackSO backpackSO)
    {
        itemName = backpackSO.itemName;
        description = backpackSO.description;
        sprite_equipMale_Front = backpackSO.sprite_equipMale_Front;
        sprite_equipMale_Back = backpackSO.sprite_equipMale_Back;
        sprite_equipFemale_Front = backpackSO.sprite_equipFemale_Front;
        sprite_equipFemale_Back = backpackSO.sprite_equipFemale_Back;
        sprite_inventory = backpackSO.sprite_inventory;
        inventoryCapacity = backpackSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = backpackSO.Stats();
        slotType = Slot.SlotType.Backpack;
        craftedIn = backpackSO.craftedIn;
        requieredCraftingLevel = backpackSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(BeltSO beltSO)
    {
        itemName = beltSO.itemName;
        description = beltSO.description;
        sprite_equipMale_Front = beltSO.sprite_equipMale_Front;
        sprite_equipMale_Back = beltSO.sprite_equipMale_Back;
        sprite_equipFemale_Front = beltSO.sprite_equipFemale_Front;
        sprite_equipFemale_Back = beltSO.sprite_equipFemale_Back;
        sprite_inventory = beltSO.sprite_inventory;
        inventoryCapacity = beltSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = beltSO.BeltStats();
        slotType = Slot.SlotType.Belt;
        craftedIn = beltSO.craftedIn;
        requieredCraftingLevel = beltSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(ShieldSO shieldSO)
    {
        itemName = shieldSO.itemName;
        description = shieldSO.description;
        sprite_equipMale_Front = shieldSO.sprite_equipMale_Front;
        sprite_equipMale_Back = shieldSO.sprite_equipMale_Back;
        sprite_equipFemale_Front = shieldSO.sprite_equipFemale_Front;
        sprite_equipFemale_Back = shieldSO.sprite_equipFemale_Back;
        sprite_inventory = shieldSO.sprite_inventory;
        isStackable = false;
        stackSize = 1;
        stats = shieldSO.Stats();
        slotType = Slot.SlotType.Shield;
        craftedIn = shieldSO.craftedIn;
        requieredCraftingLevel = shieldSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = shieldSO.upgradedVersionsOfShield;
        isUpgrade = shieldSO.isUpgrade;
    }
    public Item(MaterialSO materialSO)
    {
        itemName = materialSO.itemName;
        description = materialSO.description;
        sprite_inventory = materialSO.sprite_inventory;
        amount = materialSO.amount;
        stackSize = materialSO.stackSize;
        isStackable = true;
        stats = materialSO.Stats();
        if (itemName.ToLower().Contains("crystal"))
            slotType = Slot.SlotType.MagicCrystal;
        else
            slotType = Slot.SlotType.Material;
        craftedIn = materialSO.craftedIn;
        requieredCraftingLevel = materialSO.requieredCraftingLevel;
        recipe = new();
        crystalType = materialSO.crystalType;
    }
    public Item(ThrowableSO throwableSO)
    {
        itemName = throwableSO.itemName;
        description = throwableSO.description;
        sprite_inventory = throwableSO.sprite_inventory;
        sprite_handMale_Front = throwableSO.sprite_handMale_Front;
        sprite_handMale_Back = throwableSO.sprite_handMale_Back;
        sprite_handFemale_Front = throwableSO.sprite_handFemale_Front;
        sprite_handFemale_Back = throwableSO.sprite_handFemale_Back;
        stackSize = throwableSO.stackSize;
        isStackable = true;
        stats = throwableSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = throwableSO.craftedIn;
        requieredCraftingLevel = throwableSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(AccessorySO equipableSO)
    {
        itemName = equipableSO.itemName;
        description = equipableSO.description;
        sprite_inventory = equipableSO.sprite_inventory;
        sprite_equipMale_Front = equipableSO.sprite_equipMale_Front;
        sprite_equipMale_Back = equipableSO.sprite_equipMale_Back;
        sprite_equipFemale_Front = equipableSO.sprite_equipFemale_Front;
        sprite_equipFemale_Back = equipableSO.sprite_equipFemale_Back;
        isStackable = true;
        stats = equipableSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = equipableSO.craftedIn;
        requieredCraftingLevel = equipableSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(TrapSO trapSO)
    {
        itemName = trapSO.itemName;
        description = trapSO.description;
        sprite_inventory = trapSO.sprite_inventory;
        sprite_handMale_Front = trapSO.sprite_handMale_Front;
        sprite_handMale_Back = trapSO.sprite_handMale_Back;
        sprite_handFemale_Front = trapSO.sprite_handFemale_Front;
        sprite_handFemale_Back = trapSO.sprite_handFemale_Back;
        stackSize = trapSO.stackSize;
        isStackable = true;
        stats = trapSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = trapSO.craftedIn;
        requieredCraftingLevel = trapSO.requieredCraftingLevel;
        recipe = new();
    }
    public Item(BaseUpgradeSO baseUpgradeSO)
    {
        itemName = baseUpgradeSO.itemName;
        description = baseUpgradeSO.description;
        sprite_inventory = baseUpgradeSO.sprite_inventory;
        baseUpgradeType = baseUpgradeSO.baseUpgradeType;
        nextLevel = baseUpgradeSO.nextLevel;
        levelOfUpgrade = baseUpgradeSO.levelOfUpgrade;
        requieredAge = baseUpgradeSO .requieredAge;
        recipe = new();
        for (int i = 0; i < baseUpgradeSO.recipeMaterials.Count; i++)
            recipe.Add(baseUpgradeSO.recipeMaterials[i], baseUpgradeSO.recipeMaterialsAmount[i]);
    }
    public void SetUpByItem(Item item)
    {
        stats = item.stats;
        itemName = item.itemName;
        description = item.description;
        slotType = item.slotType;
        isStackable = item.isStackable;
        stackSize = item.stackSize;
        emitsLight = item.emitsLight;
        amount = item.amount;
        armorStats = item.armorStats;
        sprite_inventory = item.sprite_inventory;
        sprite_handMale_Front = item.sprite_handMale_Front;
        sprite_handMale_Back = item.sprite_handMale_Back;
        sprite_handFemale_Front = item.sprite_handFemale_Front;
        sprite_handFemale_Back = item.sprite_handFemale_Back;
        sprite_equipMale_Front = item.sprite_equipMale_Front;
        sprite_equipMale_Back = item.sprite_equipMale_Back;
        sprite_equipFemale_Front = item.sprite_equipFemale_Front;
        sprite_equipFemale_Back = item.sprite_equipFemale_Back;
        inventoryCapacity = item.inventoryCapacity;
        fullAuto = item.fullAuto;
        ammo = item.ammo;
        weaponType = item.weaponType;
        twoHanded = item.twoHanded;
        AoE = item.AoE;
        craftedIn = item.craftedIn;
        requieredCraftingLevel = item.requieredCraftingLevel;
        recipe = item.recipe;
        upgradedVersionOfItem = item.upgradedVersionOfItem;
        magicCrystals = item.magicCrystals;
        crystalType = item.crystalType;
        baseUpgradeType = item.baseUpgradeType;
        nextLevel = item.nextLevel;
        levelOfUpgrade = item.levelOfUpgrade;
        requieredAge = item.requieredAge;
        hideHairWhenEquiped = item.hideHairWhenEquiped;
        hideBeardWhenEquiped = item.hideBeardWhenEquiped;
        hideBodyWhenEquiped = item.hideBodyWhenEquiped;
        fullSetBonus = item.fullSetBonus;
        isUpgrade = item.isUpgrade;
    }


    private void Start()
    {
        switch (weaponType)
        {
            case WeaponType.NotAWeapon:
                weaponClass = PlayerStats.WeaponClass.None;
                break;
            case WeaponType.Whip:
                weaponClass = PlayerStats.WeaponClass.OneHandDexterity;
                break;
            case WeaponType.Dagger:
                weaponClass = PlayerStats.WeaponClass.OneHandDexterity;
                break;
            case WeaponType.Sword:
                weaponClass = PlayerStats.WeaponClass.OneHandDexterity;
                break;
            case WeaponType.Rapier:
                weaponClass = PlayerStats.WeaponClass.OneHandDexterity;
                break;
            case WeaponType.Axe:
                weaponClass = PlayerStats.WeaponClass.OneHandStrenght;
                break;
            case WeaponType.Mace:
                weaponClass = PlayerStats.WeaponClass.OneHandStrenght;
                break;
            case WeaponType.Hammer_oneHanded:
                weaponClass = PlayerStats.WeaponClass.OneHandStrenght;
                break;
            case WeaponType.QuarterStaff:
                weaponClass = PlayerStats.WeaponClass.TwoHandDexterity;
                break;
            case WeaponType.Spear:
                weaponClass = PlayerStats.WeaponClass.TwoHandDexterity;
                break;
            case WeaponType.Longsword:
                weaponClass = PlayerStats.WeaponClass.TwoHandDexterity;
                break;
            case WeaponType.Halbert:
                weaponClass = PlayerStats.WeaponClass.TwoHandStrenght;
                break;
            case WeaponType.Hammer_twoHanded:
                weaponClass = PlayerStats.WeaponClass.TwoHandStrenght;
                break;
            case WeaponType.Zweihander:
                weaponClass = PlayerStats.WeaponClass.TwoHandStrenght;
                break;
            case WeaponType.Bow:
                weaponClass = PlayerStats.WeaponClass.RangeDexterity;
                break;
            case WeaponType.SMG:
                weaponClass = PlayerStats.WeaponClass.RangeDexterity;
                break;
            case WeaponType.Pistol:
                weaponClass = PlayerStats.WeaponClass.RangeDexterity;
                break;
            case WeaponType.AttackRifle:
                weaponClass = PlayerStats.WeaponClass.RangeDexterity;
                break;
            case WeaponType.Thrower:
                weaponClass = PlayerStats.WeaponClass.RangeDexterity;
                break;
            case WeaponType.Longbow:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.Crossbow:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.Shotgun:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.Revolver:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.Machinegun:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.SniperRifle:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.Launcher:
                weaponClass = PlayerStats.WeaponClass.RangeStrenght;
                break;
            case WeaponType.MagicWeapon_fire:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon_water:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon_earth:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon_air:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon_light:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon_dark:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            case WeaponType.MagicWeapon:
                weaponClass = PlayerStats.WeaponClass.Magic;
                break;
            default:
                Debug.Log(itemName);
                break;
        }

        if (isRecipe)
            return;
        text = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = sprite_inventory;
        recipe ??= new();

        UpdateMagicCrystalsByAge((int)FindAnyObjectByType<PlayerStats>().playerStats["age"]);

        if(weaponClass == PlayerStats.WeaponClass.Magic)
            magicSkillBonuses = new();
    }

    private void Update()
    {
        if (isRecipe)
            return;
        if (amount <= 0)
            StartCoroutine(DestroyItem());
        else if (amount == 1)
            text.text = "";
        else if (amount == 69)
            text.text = "nice";
        else
            text.text = amount.ToString();

        // Magic skills bonus
        if (weaponClass == PlayerStats.WeaponClass.Magic)
        {
            Dictionary<MagicCrystalType, int> crystals = new()
            {
                { MagicCrystalType.Fire, 0 },
                { MagicCrystalType.Water, 0 },
                { MagicCrystalType.Air, 0 },
                { MagicCrystalType.Earth, 0 },
                { MagicCrystalType.Light, 0 },
                { MagicCrystalType.Dark, 0 }
            };

            for (int i = 0; i < magicCrystals.Count; i++)
                if (magicCrystals[i] != MagicCrystalType.None)
                    crystals[magicCrystals[i]]++;

            switch (magicCrystals.Count)
            {
                case 1:
                    if (crystals.Count == 1)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 1f;
                                break;
                            }
                    }
                    break;
                case 2:
                    if (crystals.Count == 1)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                        {
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 1f;
                                break;
                            }
                            else if (crystals[type] == 2)
                            {
                                magicSkillBonuses[type] = 1.15f;
                                break;
                            }
                        }
                    }
                    else if (crystals.Count == 2)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                        {
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 0.5f;
                                break;
                            }
                        }
                    }
                    break;
                case 3:
                    if (crystals.Count == 1)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                        {
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 1f;
                                break;
                            }
                            else if (crystals[type] == 2)
                            {
                                magicSkillBonuses[type] = 1.15f;
                                break;
                            }
                            else if (crystals[type] == 3)
                            {
                                magicSkillBonuses[type] = 1.3f;
                                break;
                            }
                        }
                    }
                    else if (crystals.Count == 2)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                        {
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 0.5f;
                                break;
                            }
                            else if (crystals[type] == 2)
                            {
                                magicSkillBonuses[type] = 1f;
                                break;
                            }
                        }
                    }
                    else if (crystals.Count == 3)
                    {
                        foreach (MagicCrystalType type in crystals.Keys)
                        {
                            if (crystals[type] == 1)
                            {
                                magicSkillBonuses[type] = 0.5f;
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        // Used spell
        if(weaponClass == PlayerStats.WeaponClass.Magic)
            UsedSpell();
    }

    private void UsedSpell()
    {
        // One crystal slot available
        if(magicCrystals.Count == 1)
        {
            spell = magicCrystals[0] switch
            {
                MagicCrystalType.Fire => Spell.Fireball,
                MagicCrystalType.Water => Spell.Watersplash,
                MagicCrystalType.Air => Spell.Windblow,
                MagicCrystalType.Earth => Spell.Stone,
                MagicCrystalType.Light => Spell.Lightning,
                MagicCrystalType.Dark => Spell.Lifesteal,
                _ => Spell.None,
            };
        }
        // Two crystal slots available
        else if (magicCrystals.Count == 2)
        {

        }
        // Three crystal slots available
        else if (magicCrystals.Count == 3)
        {

        }
    }

    public void UpdateMagicCrystalsByAge(int age)
    {
        if (weaponType != WeaponType.MagicWeapon)
            return;

        Dictionary<int, MagicCrystalType> oldMagicCrystals = new();
        if(magicCrystals != null)
            oldMagicCrystals = magicCrystals;

        magicCrystals = age switch
        {
            < 2 => new(){
                        {0, MagicCrystalType.None },
                    },
            < 4 => new(){
                        {0, MagicCrystalType.None },
                        {1, MagicCrystalType.None },
                    },
            _ => new(){
                        {0, MagicCrystalType.None },
                        {1, MagicCrystalType.None },
                        {2, MagicCrystalType.None },
                    },
        };
        for (int i = 0; i < oldMagicCrystals.Count; i++)
            magicCrystals[i] = oldMagicCrystals[i];
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    public List<Item> GetMaterials()
    {
        ItemDatabase itemDatabase = FindAnyObjectByType<PlayerInventory>().itemDatabase;

        List<Item> items = new();
        foreach (ScriptableObject recipe in recipe.Keys)
        {
            if (recipe.GetType() == typeof(ArmorSO)) items.Add(itemDatabase.GetArmor((recipe as ArmorSO).itemName));
            else if (recipe.GetType() == typeof(BackpackSO)) items.Add(itemDatabase.GetBackpack((recipe as BackpackSO).itemName));
            else if (recipe.GetType() == typeof(BeltSO)) items.Add(itemDatabase.GetBelt((recipe as BeltSO).itemName));
            else if (recipe.GetType() == typeof(ConsumableSO)) items.Add(itemDatabase.GetConsumable((recipe as ConsumableSO).itemName));
            else if (recipe.GetType() == typeof(AccessorySO)) items.Add(itemDatabase.GetAccessory((recipe as AccessorySO).itemName));
            else if (recipe.GetType() == typeof(MaterialSO)) items.Add(itemDatabase.GetMaterial((recipe as MaterialSO).itemName));
            else if (recipe.GetType() == typeof(ProjectileSO)) items.Add(itemDatabase.GetProjectile((recipe as ProjectileSO).itemName));
            else if (recipe.GetType() == typeof(ShieldSO)) items.Add(itemDatabase.GetShield((recipe as ShieldSO).itemName));
            else if (recipe.GetType() == typeof(ThrowableSO)) items.Add(itemDatabase.GetThrowable((recipe as ThrowableSO).itemName));
            else if (recipe.GetType() == typeof(TrapSO)) items.Add(itemDatabase.GetTrap((recipe as TrapSO).itemName));
            else if (recipe.GetType() == typeof(WeaponMagicSO)) items.Add(itemDatabase.GetWeaponMagic((recipe as WeaponMagicSO).itemName));
            else if (recipe.GetType() == typeof(WeaponMeleeSO)) items.Add(itemDatabase.GetWeaponMelee((recipe as WeaponMeleeSO).itemName));
            else if (recipe.GetType() == typeof(WeaponRangedSO)) items.Add(itemDatabase.GetWeaponRanged((recipe as WeaponRangedSO).itemName));
            else Debug.LogWarning("Nepovedlo se ur�it typeof ingredient p�i upgradu!");
        }

        List<int> amounts = new();
        foreach (int amount in recipe.Values)
            amounts.Add(amount);
        for (int i = 0; i < items.Count; i++)
            items[i].amount = amounts[i];

        return items;
    }
}