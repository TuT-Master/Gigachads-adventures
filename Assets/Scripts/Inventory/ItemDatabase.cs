using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBase;

[CreateAssetMenu(fileName = "Item database", menuName = "Scriptable objects/Item database")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponMeleeSO> weaponsMelee;

    public List<WeaponRangedSO> weaponsRanged;

    public List<WeaponMagicSO> weaponsMagic;

    public List<ThrowableSO> throwables;

    public List<TrapSO> traps;

    public List<ArmorSO> armors;

    public List<AccessorySO> accessories;

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
            {
                Item item = itemSO.ToItem();
                item.amount = 1;
                return item;
            }
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
    public Item GetAccessory(string name) => GetItemFromList(accessories, name);
    public Item GetThrowable(string name) => GetItemFromList(throwables, name);
    public Item GetTrap(string name) => GetItemFromList(traps, name);
    private Dictionary<string, ItemSO> allItemsSO;
    private void Awake()
    {
        allItemsSO = new();
        foreach (WeaponMeleeSO item in weaponsMelee)
            allItemsSO[item.itemName] = item;
        foreach (WeaponRangedSO item in weaponsRanged)
            allItemsSO[item.itemName] = item;
        foreach (WeaponMagicSO item in weaponsMagic)
            allItemsSO[item.itemName] = item;
        foreach (ArmorSO item in armors)
            allItemsSO[item.itemName] = item;
        foreach (BackpackSO item in backpacks)
            allItemsSO[item.itemName] = item;
        foreach (BeltSO item in belts)
            allItemsSO[item.itemName] = item;
        foreach (ConsumableSO item in consumables)
            allItemsSO[item.itemName] = item;
        foreach (ProjectileSO item in projectiles)
            allItemsSO[item.itemName] = item;
        foreach (ShieldSO item in shields)
            allItemsSO[item.itemName] = item;
        foreach (MaterialSO item in materials)
            allItemsSO[item.itemName] = item;
        foreach (AccessorySO item in accessories)
            allItemsSO[item.itemName] = item;
        foreach (ThrowableSO item in throwables)
            allItemsSO[item.itemName] = item;
        foreach (TrapSO item in traps)
            allItemsSO[item.itemName] = item;
    }
    public Item GetItemByNameAndAmount(string name, int amount)
    {
        Debug.Log(allItemsSO.Count);
        Item item = allItemsSO.TryGetValue(name, out ItemSO itemSO) ? itemSO.ToItem() : null;
        item.amount = amount;
        return item;
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
            accessories,
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
