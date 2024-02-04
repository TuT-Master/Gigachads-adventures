using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Dictionary<string, float> stats;

    public string itemName;
    public string description;

    public Slot.SlotType slotType;

    public bool isStackable;
    public bool emitsLight;

    public int amount;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;
    public Sprite sprite_equip;

    private TextMeshProUGUI text;


    public Item(WeaponMeleeSO weaponSO)
    {
        itemName = weaponSO.itemName;
        slotType = Slot.SlotType.WeaponMelee;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
        stats = weaponSO.stats;
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
    }
    public Item(WeaponRangedSO weaponSO)
    {
        itemName = weaponSO.itemName;
        slotType = Slot.SlotType.WeaponRanged;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
        stats = weaponSO.stats;
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
    }
    public Item(ConsumableSO consumableSO)
    {
        itemName = consumableSO.itemName;
        description = consumableSO.description;
        slotType = Slot.SlotType.Consumable;
        sprite_inventory = consumableSO.sprite_inventory;
        sprite_hand = consumableSO.sprite_hand;
        isStackable = consumableSO.isStackable;
    }
    public Item(ProjectileSO projectile)
    {
        slotType = Slot.SlotType.Ammo;
        sprite_inventory = projectile.sprite_inventory;
        sprite_equip = projectile.sprite_equip;
        stats = projectile.projectileStats;
    }
    public Item(ArmorSO armorSO)
    {
        itemName = armorSO.itemName;
        description = armorSO.description;
        slotType = armorSO.slotType;
        sprite_inventory = armorSO.sprite_inventory;
        sprite_equip = armorSO.sprite_equip;
    }


    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        sprite_inventory = null;
        sprite_hand = null;
    }
}
