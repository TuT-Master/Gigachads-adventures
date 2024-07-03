using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

    public int amount;

    public int inventoryCapacity;

    public bool twoHanded;
    public bool AoE;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;
    public Sprite sprite_equip;
    public Sprite sprite_equipBack;

    public List<ProjectileSO> ammo;

    // Crafting
    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public Dictionary<ScriptableObject, int> recipe;

    public bool isRecipe;

    // Upgrading
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
        return index switch
        {
            1 => MagicCrystalType.Fire,
            2 => MagicCrystalType.Water,
            3 => MagicCrystalType.Air,
            4 => MagicCrystalType.Earth,
            5 => MagicCrystalType.Light,
            6 => MagicCrystalType.Dark,
            _ => MagicCrystalType.None,
        };
    }
    public MagicCrystalType crystalType;

    // Magic weapons
    public Dictionary<int, MagicCrystalType> magicCrystals;


    private TextMeshProUGUI text;



    public Item(WeaponMeleeSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.WeaponMelee;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
        stats = weaponSO.Stats();
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
        weaponType = weaponSO.weaponType;
        twoHanded = weaponSO.twoHanded;
        AoE = weaponSO.AoE;
        craftedIn = weaponSO.craftedIn;
        requieredCraftingLevel = weaponSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < weaponSO.recipeMaterials.Count; i++)
            recipe.Add(weaponSO.recipeMaterials[i], weaponSO.recipeMaterialsAmount[i]);
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
    }
    public Item(WeaponRangedSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description= weaponSO.description;
        slotType = Slot.SlotType.WeaponRanged;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
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
        for (int i = 0; i < weaponSO.recipeMaterials.Count; i++)
            recipe.Add(weaponSO.recipeMaterials[i], weaponSO.recipeMaterialsAmount[i]);
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
    }
    public Item(WeaponMagicSO weaponSO)
    {
        itemName = weaponSO.itemName;
        description = weaponSO.description;
        slotType = Slot.SlotType.MagicWeapon;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
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
        for (int i = 0; i < weaponSO.recipeMaterials.Count; i++)
            recipe.Add(weaponSO.recipeMaterials[i], weaponSO.recipeMaterialsAmount[i]);
        upgradedVersionOfItem = weaponSO.upgradedVersionsOfWeapon;
        magicCrystals = weaponSO.magicCrystals;
    }
    public Item(ConsumableSO consumableSO)
    {
        itemName = consumableSO.itemName;
        description = consumableSO.description;
        slotType = Slot.SlotType.Consumable;
        sprite_inventory = consumableSO.sprite_inventory;
        sprite_hand = consumableSO.sprite_hand;
        isStackable = consumableSO.isStackable;
        stackSize = consumableSO.stackSize;
        stats = consumableSO.Stats();
        craftedIn = consumableSO.craftedIn;
        requieredCraftingLevel = consumableSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < consumableSO.recipeMaterials.Count; i++)
            recipe.Add(consumableSO.recipeMaterials[i], consumableSO.recipeMaterialsAmount[i]);
    }
    public Item(ProjectileSO projectile)
    {
        slotType = Slot.SlotType.Ammo;
        itemName = projectile.itemName;
        description = projectile.description;
        sprite_inventory = projectile.sprite_inventory;
        sprite_equip = projectile.sprite_projectile;
        stats = projectile.ProjectileStats();
        isStackable = true;
        stackSize = projectile.stackSize;
        craftedIn = projectile.craftedIn;
        requieredCraftingLevel = projectile.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < projectile.recipeMaterials.Count; i++)
            recipe.Add(projectile.recipeMaterials[i], projectile.recipeMaterialsAmount[i]);
    }
    public Item(ArmorSO armorSO)
    {
        itemName = armorSO.itemName;
        description = armorSO.description;
        armorStats = armorSO.ArmorStats();
        amount = 1;
        slotType = armorSO.slotType;
        sprite_inventory = armorSO.sprite_inventory;
        sprite_equip = armorSO.sprite_equipFront;
        sprite_equipBack = armorSO.sprite_equipBack;
        craftedIn = armorSO.craftedIn;
        requieredCraftingLevel = armorSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < armorSO.recipeMaterials.Count; i++)
            recipe.Add(armorSO.recipeMaterials[i], armorSO.recipeMaterialsAmount[i]);
        upgradedVersionOfItem = armorSO.upgradedVersionsOfArmor;
    }
    public Item(BackpackSO backpackSO)
    {
        itemName = backpackSO.itemName;
        description = backpackSO.description;
        sprite_equip = backpackSO.sprite_equipFront;
        sprite_equipBack = backpackSO.sprite_equipBack;
        sprite_inventory = backpackSO.sprite_inventory;
        inventoryCapacity = backpackSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = backpackSO.BackpackStats();
        slotType = Slot.SlotType.Backpack;
        craftedIn = backpackSO.craftedIn;
        requieredCraftingLevel = backpackSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < backpackSO.recipeMaterials.Count; i++)
            recipe.Add(backpackSO.recipeMaterials[i], backpackSO.recipeMaterialsAmount[i]);
    }
    public Item(BeltSO beltSO)
    {
        itemName = beltSO.itemName;
        description = beltSO.description;
        sprite_equip = beltSO.sprite_equipFront;
        sprite_equipBack = beltSO.sprite_equipBack;
        sprite_inventory = beltSO.sprite_inventory;
        inventoryCapacity = beltSO.inventoryCapacity;
        isStackable = false;
        stackSize = 1;
        stats = beltSO.BeltStats();
        slotType = Slot.SlotType.Belt;
        craftedIn = beltSO.craftedIn;
        requieredCraftingLevel = beltSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < beltSO.recipeMaterials.Count; i++)
            recipe.Add(beltSO.recipeMaterials[i], beltSO.recipeMaterialsAmount[i]);
    }
    public Item(ShieldSO shieldSO)
    {
        itemName = shieldSO.itemName;
        description = shieldSO.description;
        sprite_equip = shieldSO.sprite_equipFront;
        sprite_equipBack = shieldSO.sprite_equipBack;
        sprite_inventory = shieldSO.sprite_inventory;
        isStackable = false;
        stackSize = 1;
        stats = shieldSO.Stats();
        slotType = Slot.SlotType.Shield;
        craftedIn = shieldSO.craftedIn;
        requieredCraftingLevel = shieldSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < shieldSO.recipeMaterials.Count; i++)
            recipe.Add(shieldSO.recipeMaterials[i], shieldSO.recipeMaterialsAmount[i]);
        upgradedVersionOfItem = shieldSO.upgradedVersionsOfShield;
    }
    public Item(MaterialSO materialSO)
    {
        itemName = materialSO.itemName;
        description = materialSO.description;
        sprite_inventory = materialSO.sprite_inventory;
        stackSize= materialSO.stackSize;
        isStackable = true;
        stats = materialSO.Stats();
        if (itemName.ToLower().Contains("crystal"))
            slotType = Slot.SlotType.MagicCrystal;
        else
            slotType = Slot.SlotType.Material;
        craftedIn = materialSO.craftedIn;
        requieredCraftingLevel = materialSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < materialSO.recipeMaterials.Count; i++)
            recipe.Add(materialSO.recipeMaterials[i], materialSO.recipeMaterialsAmount[i]);
        crystalType = materialSO.crystalType;
    }
    public Item(ThrowableSO throwableSO)
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
        for (int i = 0; i < throwableSO.recipeMaterials.Count; i++)
            recipe.Add(throwableSO.recipeMaterials[i], throwableSO.recipeMaterialsAmount[i]);
    }
    public Item(EquipableSO equipableSO)
    {
        itemName = equipableSO.itemName;
        description = equipableSO.description;
        sprite_inventory = equipableSO.sprite_inventory;
        isStackable = true;
        stats = equipableSO.ArmorStats();
        slotType = Slot.SlotType.Material;
        craftedIn = equipableSO.craftedIn;
        requieredCraftingLevel = equipableSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < equipableSO.recipeMaterials.Count; i++)
            recipe.Add(equipableSO.recipeMaterials[i], equipableSO.recipeMaterialsAmount[i]);
    }
    public Item(TrapSO trapSO)
    {
        itemName = trapSO.itemName;
        description = trapSO.description;
        sprite_inventory = trapSO.sprite_inventory;
        stackSize = trapSO.stackSize;
        isStackable = true;
        stats = trapSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = trapSO.craftedIn;
        requieredCraftingLevel = trapSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < trapSO.recipeMaterials.Count; i++)
            recipe.Add(trapSO.recipeMaterials[i], trapSO.recipeMaterialsAmount[i]);
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
        sprite_hand = item.sprite_hand;
        sprite_equip = item.sprite_equip;
        sprite_equipBack = item.sprite_equipBack;
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
    }


    private void Start()
    {
        if (isRecipe)
            return;
        text = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = sprite_inventory;
        recipe ??= new();

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

        UpdateMagicCrystalsByAge((int)FindAnyObjectByType<PlayerStats>().playerStats["age"]);
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
    }

    public void UpdateMagicCrystalsByAge(int age)
    {
        if (weaponType != WeaponType.MagicWeapon)
            return;

        Dictionary<int, MagicCrystalType> oldMagicCrystals = new();
        if(magicCrystals != null)
            oldMagicCrystals = magicCrystals;

        switch (age)
        {
            case < 2:
                magicCrystals = new(){
                        {0, MagicCrystalType.None },
                    };
                break;
            case < 4:
                magicCrystals = new(){
                        {0, MagicCrystalType.None },
                        {1, MagicCrystalType.None },
                    };
                break;
            default:
                magicCrystals = new(){
                        {0, MagicCrystalType.None },
                        {1, MagicCrystalType.None },
                        {2, MagicCrystalType.None },
                    };
                break;
        }

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
            else if (recipe.GetType() == typeof(EquipableSO)) items.Add(itemDatabase.GetEquipable((recipe as EquipableSO).itemName));
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
