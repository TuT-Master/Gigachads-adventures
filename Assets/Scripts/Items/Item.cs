using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerStats;

public class Item : MonoBehaviour
{
    public Dictionary<StatType, float> stats;

    public string itemName;
    [TextArea]
    public string description;

    public Slot.SlotType slotType;
    public enum ItemType
    {
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


        // Armors
        Helmet,
        Chestplate,
        Leggings,
        Gauntlets,


        // Accessory
        HelmetAccessory,
        ChestplateAccessory,
        LeggingsAccessory,
        GauntletsAccessory,


        // Other
        Throwable,
        Trap,
        Projectile,
        Consumable,
        Backpack,
        Belt,
        Material,
        Pickaxe,
    }
    public ItemType itemType;
    public WeaponClass weaponClass;

    public bool isStackable;
    public int stackSize;

    public bool emitsLight;

    public bool fullAuto;

    public int amount = 1;

    public int inventoryCapacity;

    public bool twoHanded;
    public bool AoE;

    public Sprite sprite_inventory;
    public GameObject itemModel;

    public List<ProjectileSO> ammo;

    public bool hideHairWhenEquiped;
    public bool hideBeardWhenEquiped;
    public bool hideBodyWhenEquiped;

    public bool selfHoming;

    // Full-set bonus
    public Dictionary<StatType, float> fullSetBonus;

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


    public void SetItem(WeaponMeleeSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.WeaponMelee;
        sprite_inventory = weaponSO.sprite_inventory;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
        itemType = weaponSO.itemType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        isUpgrade = weaponSO.isUpgrade;
        itemModel = weaponSO.itemModel;
    }
    public void SetItem(WeaponRangedSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.WeaponRanged;
        sprite_inventory = weaponSO.sprite_inventory;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        stackSize = weaponSO.stackSize;
        emitsLight = weaponSO.emitsLight;
        fullAuto = weaponSO.fullAuto;
        ammo = weaponSO.ammo;
        itemType = weaponSO.itemType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        isUpgrade = weaponSO.isUpgrade;
        itemModel = weaponSO.itemModel;
    }
    public void SetItem(WeaponMagicSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.MagicWeapon;
        sprite_inventory = weaponSO.sprite_inventory;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        stackSize = weaponSO.stackSize;
        emitsLight = weaponSO.emitsLight;
        fullAuto = weaponSO.fullAuto;
        itemType = weaponSO.itemType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        magicCrystals = weaponSO.magicCrystals;
        isUpgrade = weaponSO.isUpgrade;
        itemModel = weaponSO.itemModel;
    }
    public void SetItem(ConsumableSO consumableSO)
    {
        itemName = consumableSO.itemName;
        description = consumableSO.description;
        slotType = Slot.SlotType.Consumable;
        weaponClass = WeaponClass.None;
        itemType = ItemType.Consumable;
        sprite_inventory = consumableSO.sprite_inventory;
        isStackable = consumableSO.isStackable;
        stackSize = consumableSO.stackSize;
        stats = consumableSO.Stats();
        craftedIn = consumableSO.craftedIn;
        requieredCraftingLevel = consumableSO.requieredCraftingLevel;
        recipe = new();
        itemModel = consumableSO.itemModel;
    }
    public void SetItem(ProjectileSO projectile)
    {
        slotType = Slot.SlotType.Ammo;
        weaponClass = WeaponClass.Projectile;
        itemType = ItemType.Projectile;
        itemName = projectile.itemName;
        description = projectile.description;
        sprite_inventory = projectile.sprite_inventory;
        stats = projectile.Stats();
        isStackable = true;
        stackSize = projectile.stackSize;
        craftedIn = projectile.craftedIn;
        requieredCraftingLevel = projectile.requieredCraftingLevel;
        recipe = new();
        selfHoming = projectile.selfHoming;
    }
    public void SetItem(ArmorSO armorSO)
    {
        itemName = armorSO.itemName;
        description = armorSO.description;
        stats = armorSO.Stats();
        amount = 1;
        slotType = armorSO.slotType;
        weaponClass = WeaponClass.None;
        itemType = armorSO.itemType;
        sprite_inventory = armorSO.sprite_inventory;
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
    public void SetItem(BackpackSO backpackSO)
    {
        itemName = backpackSO.itemName;
        description = backpackSO.description;
        sprite_inventory = backpackSO.sprite_inventory;
        inventoryCapacity = backpackSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = backpackSO.BackpackStats();
        slotType = Slot.SlotType.Backpack;
        itemType = backpackSO.itemType;
        weaponClass = WeaponClass.None;
        craftedIn = backpackSO.craftedIn;
        requieredCraftingLevel = backpackSO.requieredCraftingLevel;
        recipe = new();
    }
    public void SetItem(BeltSO beltSO)
    {
        itemName = beltSO.itemName;
        description = beltSO.description;
        sprite_inventory = beltSO.sprite_inventory;
        inventoryCapacity = beltSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = beltSO.BeltStats();
        slotType = Slot.SlotType.Belt;
        itemType = ItemType.Belt;
        weaponClass = WeaponClass.None;
        craftedIn = beltSO.craftedIn;
        requieredCraftingLevel = beltSO.requieredCraftingLevel;
        recipe = new();
    }
    public void SetItem(ShieldSO shieldSO)
    {
        itemName = shieldSO.itemName;
        description = shieldSO.description;
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
        itemModel = shieldSO.itemModel;
    }
    public void SetItem(MaterialSO materialSO)
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
        itemType = ItemType.Material;
        weaponClass = WeaponClass.None;
        craftedIn = materialSO.craftedIn;
        requieredCraftingLevel = materialSO.requieredCraftingLevel;
        recipe = new();
        crystalType = materialSO.crystalType;
    }
    public void SetItem(ThrowableSO throwableSO)
    {
        itemName = throwableSO.itemName;
        description = throwableSO.description;
        sprite_inventory = throwableSO.sprite_inventory;
        stackSize = throwableSO.stackSize;
        isStackable = true;
        stats = throwableSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = throwableSO.craftedIn;
        requieredCraftingLevel = throwableSO.requieredCraftingLevel;
        recipe = new();
        itemModel = throwableSO.itemModel;
    }
    public void SetItem(AccessorySO accessorySO)
    {
        itemName = accessorySO.itemName;
        description = accessorySO.description;
        sprite_inventory = accessorySO.sprite_inventory;
        isStackable = true;
        stats = accessorySO.Stats();
        slotType = accessorySO.slotType;
        craftedIn = accessorySO.craftedIn;
        requieredCraftingLevel = accessorySO.requieredCraftingLevel;
        recipe = new();
    }
    public void SetItem(TrapSO trapSO)
    {
        itemName = trapSO.itemName;
        description = trapSO.description;
        sprite_inventory = trapSO.sprite_inventory;
        stackSize = trapSO.stackSize;
        isStackable = true;
        stats = trapSO.Stats();
        slotType = trapSO.slotType;
        craftedIn = trapSO.craftedIn;
        requieredCraftingLevel = trapSO.requieredCraftingLevel;
        recipe = new();
        itemModel = trapSO.itemModel;
    }
    public void SetItem(BaseUpgradeSO baseUpgradeSO)
    {
        itemName = baseUpgradeSO.itemName;
        description = baseUpgradeSO.description;
        sprite_inventory = baseUpgradeSO.sprite_inventory;
        baseUpgradeType = baseUpgradeSO.baseUpgradeType;
        nextLevel = baseUpgradeSO.nextLevel;
        levelOfUpgrade = baseUpgradeSO.levelOfUpgrade;
        requieredAge = baseUpgradeSO.requieredAge;
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
        sprite_inventory = item.sprite_inventory;
        inventoryCapacity = item.inventoryCapacity;
        fullAuto = item.fullAuto;
        ammo = item.ammo;
        itemType = item.itemType;
        weaponClass = item.weaponClass;
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
        itemModel = item.itemModel;
    }


    private void Start()
    {
        SetWeaponClass();

        if (isRecipe) return;

        text = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = sprite_inventory;
        recipe ??= new();

        UpdateMagicCrystalsByAge((int)FindAnyObjectByType<PlayerStats>().playerStats[StatType.Age]);
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
        if (weaponClass == (WeaponClass.MagicAir | WeaponClass.MagicFire | WeaponClass.MagicEarth |
            WeaponClass.MagicWater | WeaponClass.MagicLight | WeaponClass.MagicDark))
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
        if (IsMagicWeapon())
            UsedSpell();
    }
    private void SetWeaponClass()
    {
        if (itemType == ItemType.Consumable ||
            itemType == ItemType.Backpack ||
            itemType == ItemType.Belt ||
            itemType == ItemType.Helmet ||
            itemType == ItemType.Chestplate ||
            itemType == ItemType.Leggings ||
            itemType == ItemType.Gauntlets ||
            itemType == ItemType.HelmetAccessory ||
            itemType == ItemType.ChestplateAccessory ||
            itemType == ItemType.LeggingsAccessory ||
            itemType == ItemType.GauntletsAccessory ||
            itemType == ItemType.Material)
        {
            weaponClass = WeaponClass.None;
        }
        else if (itemType == ItemType.Projectile)
        {
            weaponClass = WeaponClass.Projectile;
        }
        else if (itemType == ItemType.Whip ||
                 itemType == ItemType.Dagger ||
                 itemType == ItemType.Sword ||
                 itemType == ItemType.Rapier ||
                 itemType == ItemType.LightShield)
        {
            weaponClass = WeaponClass.OneHandDexterity;
        }
        else if (itemType == ItemType.Axe ||
                 itemType == ItemType.Mace ||
                 itemType == ItemType.Hammer_oneHanded ||
                 itemType == ItemType.HeavyShield)
        {
            weaponClass = WeaponClass.OneHandStrenght;
        }
        else if (itemType == ItemType.QuarterStaff ||
                 itemType == ItemType.Spear ||
                 itemType == ItemType.Longsword)
        {
            weaponClass = WeaponClass.TwoHandDexterity;
        }
        else if (itemType == ItemType.Halbert ||
                 itemType == ItemType.Hammer_twoHanded ||
                 itemType == ItemType.Zweihander)
        {
            weaponClass = WeaponClass.TwoHandStrenght;
        }
        else if (itemType == ItemType.Bow ||
                 itemType == ItemType.SMG ||
                 itemType == ItemType.Pistol ||
                 itemType == ItemType.AttackRifle ||
                 itemType == ItemType.Thrower)
        {
            weaponClass = WeaponClass.RangeDexterity;
        }
        else if (itemType == ItemType.Longbow ||
                 itemType == ItemType.Crossbow ||
                 itemType == ItemType.Shotgun ||
                 itemType == ItemType.Revolver ||
                 itemType == ItemType.Machinegun ||
                 itemType == ItemType.SniperRifle ||
                 itemType == ItemType.Launcher)
        {
            weaponClass = WeaponClass.RangeStrenght;
        }
        else if (itemType == ItemType.MagicWeapon_fire)
        {
            weaponClass = WeaponClass.MagicFire;
        }
        else if (itemType == ItemType.MagicWeapon_water)
        {
            weaponClass = WeaponClass.MagicWater;
        }
        else if (itemType == ItemType.MagicWeapon_earth)
        {
            weaponClass = WeaponClass.MagicEarth;
        }
        else if (itemType == ItemType.MagicWeapon_air)
        {
            weaponClass = WeaponClass.MagicAir;
        }
        else if (itemType == ItemType.MagicWeapon_light)
        {
            weaponClass = WeaponClass.MagicLight;
        }
        else if (itemType == ItemType.MagicWeapon_dark)
        {
            weaponClass = WeaponClass.MagicDark;
        }
        else
            Debug.LogError($"Nepodaøilo se urèit weaponClass itemu: '{itemName}' - weaponType '{itemType}'");
    }


    private void UsedSpell()
    {
        // One crystal slot available
        if (magicCrystals.Count == 1)
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
    public bool IsMagicWeapon()
    {
        return weaponClass == WeaponClass.MagicFire ||
               weaponClass == WeaponClass.MagicWater ||
               weaponClass == WeaponClass.MagicAir ||
               weaponClass == WeaponClass.MagicEarth ||
               weaponClass == WeaponClass.MagicLight ||
               weaponClass == WeaponClass.MagicDark;
    }
    public void UpdateMagicCrystalsByAge(int age)
    {
        if (!IsMagicWeapon())
            return;

        Dictionary<int, MagicCrystalType> oldMagicCrystals = magicCrystals ?? new();

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
            else Debug.LogWarning("Nepovedlo se urèit typeof ingredient pøi upgradu!");
        }

        List<int> amounts = new();
        foreach (int amount in recipe.Values)
            amounts.Add(amount);
        for (int i = 0; i < items.Count; i++)
            items[i].amount = amounts[i];

        return items;
    }
}