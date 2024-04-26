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
        // Strenght
        Axe,
        Mace,
        Hammer_oneHanded,
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

    }
    public WeaponType weaponType;

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
    }
    public Item(ProjectileSO projectile)
    {
        slotType = Slot.SlotType.Ammo;
        itemName = projectile.itemName;
        description = projectile.description;
        sprite_inventory = projectile.sprite_inventory;
        sprite_equip = projectile.sprite_equip;
        stats = projectile.ProjectileStats();
        isStackable = true;
        stackSize = projectile.stackSize;
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
    }


    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = sprite_inventory;
    }

    private void Update()
    {
        if (amount <= 0)
            StartCoroutine(DestroyItem());
        else if (amount == 1)
            text.text = "";
        else
            text.text = amount.ToString();
    }

    public bool TwoHanded()
    {
        if(stats.ContainsKey("twoHanded"))
            if (stats["twoHanded"] == 1)
                return true;
        return false;
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
