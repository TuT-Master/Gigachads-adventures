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
    }
    public Item(MaterialSO materialSO)
    {
        itemName = materialSO.itemName;
        description = materialSO.description;
        sprite_inventory = materialSO.sprite_inventory;
        stackSize= materialSO.stackSize;
        isStackable = true;
        stats = materialSO.Stats();
        slotType = Slot.SlotType.Material;
        craftedIn = materialSO.craftedIn;
        requieredCraftingLevel = materialSO.requieredCraftingLevel;
        recipe = new();
        for (int i = 0; i < materialSO.recipeMaterials.Count; i++)
            recipe.Add(materialSO.recipeMaterials[i], materialSO.recipeMaterialsAmount[i]);
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
            default:
                Debug.Log(itemName);
                break;
        }
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

    private IEnumerator DestroyItem()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
