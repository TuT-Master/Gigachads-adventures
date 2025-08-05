using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBase;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Item database", menuName = "Scriptable objects/Item database")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponMeleeSO> weaponsMelee;

    public List<WeaponRangedSO> weaponsRanged;

    public List<WeaponMagicSO> weaponsMagic;

    public List<ThrowableSO> throwables;

    public List<TrapSO> traps;

    public List<ArmorSO> armors;

    public List<AccessorySO> equipables;

    public List<ConsumableSO> consumables;

    public List<ProjectileSO> projectiles;

    public List<BackpackSO> backpacks;

    public List<BeltSO> belts;

    public List<ShieldSO> shields;

    public List<MaterialSO> materials;

    public List<BaseUpgradeSO> baseUpgradesSO;


    private Item GetItemFromList<T>(List<T> list, string name) where T : ItemSO
    {
        foreach (ItemSO itemSO in list)
            if (itemSO.itemName == name)
                return itemSO.ToItem();
        return null;
    }
    public Item GetWeaponMelee(string name) => GetItemFromList(weaponsMelee, name);
    public Item GetWeaponRanged(string name) => GetItemFromList(weaponsRanged, name);
    public Item GetWeaponMagic(string name) => GetItemFromList(weaponsMagic, name);
    public Item GetArmor(string name) => GetItemFromList(armors, name);
    public Item GetBackpack(string name) => GetItemFromList(backpacks, name);
    public Item GetBelt(string name) => GetItemFromList(belts, name);
    public Item GetConsumable(string name) => GetItemFromList(consumables, name);
    public Item GetProjectile(string name) => GetItemFromList(projectiles, name);
    public Item GetShield(string name) => GetItemFromList(shields, name);
    public Item GetMaterial(string name) => GetItemFromList(materials, name);
    public Item GetEquipable(string name) => GetItemFromList(equipables, base.name);
    public Item GetThrowable(string name) => GetItemFromList(throwables, name);
    public Item GetTrap(string name) => GetItemFromList(traps, name);
    public Item GetItemByNameAndAmount(string name, int amount)
    {
        List<Func<string, Item>> lookups = new()
        {
            GetWeaponMelee,
            GetWeaponRanged,
            GetWeaponMagic,
            GetArmor,
            GetBackpack,
            GetBelt,
            GetConsumable,
            GetProjectile,
            GetShield,
            GetMaterial,
            GetEquipable,
            GetThrowable,
            GetTrap,
        };
        foreach (var lookup in lookups)
        {
            Item item = lookup(name);
            if (item != null)
            {
                item.amount = amount;
                return item;
            }
        }
        return null;
    }
    public List<Item> GetAllItems()
    {
        List<Item> allItems = new();
        List<IEnumerable<ItemSO>> allLists = new()
        {
            weaponsMelee,
            weaponsRanged,
            weaponsMagic,
            armors,
            backpacks,
            belts,
            consumables,
            projectiles,
            shields,
            materials,
            equipables,
            throwables,
            traps
        };
        foreach (IEnumerable<ItemSO> list in allLists)
            foreach (ItemSO item in list)
                allItems.Add(item.ToItem());
        return allItems;
    }


    public Item GetCrystalByType(Item.MagicCrystalType type)
    {
        return type switch
        {
            Item.MagicCrystalType.Fire => GetMaterial("Fire crystal"),
            Item.MagicCrystalType.Water => GetMaterial("Water crystal"),
            Item.MagicCrystalType.Air => GetMaterial("Wind crystal"),
            Item.MagicCrystalType.Earth => GetMaterial("Earth crystal"),
            Item.MagicCrystalType.Light => GetMaterial("Light crystal"),
            Item.MagicCrystalType.Dark => GetMaterial("Dark crystal"),
            _ => null
        };
    }

    public BaseUpgradeSO GetBaseUpgrade(BaseUpgrade baseUpgrade, int level)
    {
        foreach (BaseUpgradeSO upgradeSO in baseUpgradesSO)
            if (upgradeSO.baseUpgradeType == baseUpgrade && upgradeSO.levelOfUpgrade == level)
                return upgradeSO;
        return null;
    }

    public Item GetBaseUpgradeAsItem(BaseUpgrade baseUpgrade, int level)
    {
        Item item = null;
        foreach (BaseUpgradeSO upgradeSO in baseUpgradesSO)
            if (upgradeSO.baseUpgradeType == baseUpgrade && upgradeSO.levelOfUpgrade == level)
                item = new(upgradeSO);
        if(item != null)
            item.amount = 1;
        return item;
    }
}
