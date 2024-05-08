using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item database", menuName = "Scriptable objects/Item database")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponMeleeSO> weaponsMelee;

    public List<WeaponRangedSO> weaponsRanged;

    public List<ArmorSO> armors;

    public List<ConsumableSO> consumables;

    public List<ProjectileSO> projectiles;

    public List<BackpackSO> backpacks;

    public List<BeltSO> belts;

    public List<ShieldSO> shields;

    public List<MaterialSO> materials;


    public Item GetWeaponMelee(string weaponName)
    {
        foreach (WeaponMeleeSO weapon in weaponsMelee)
            if (weapon.itemName == weaponName)
                return new(weapon);
        return null;
    }
    public Item GetWeaponRanged(string weaponName)
    {
        foreach (WeaponRangedSO weapon in weaponsRanged)
            if (weapon.itemName == weaponName)
                return new(weapon);
        return null;
    }
    public Item GetArmor(string armorName)
    {
        foreach(ArmorSO armor in armors)
            if (armor.itemName == armorName)
                return new(armor);
        return null;
    }
    public Item GetBackpack(string backpackName)
    {
        foreach (BackpackSO backpack in backpacks)
            if (backpack.itemName == backpackName)
                return new(backpack);
        return null;
    }
    public Item GetBelt(string beltName)
    {
        foreach (BeltSO belt in belts)
            if (belt.itemName == beltName)
                return new(belt);
        return null;
    }

    public Item GetConsumable(string consumableName)
    {
        foreach(ConsumableSO consumable in consumables)
            if(consumable.itemName == consumableName)
                return new(consumable);
        return null;
    }

    public Item GetProjectile(string projectileName)
    {
        foreach (ProjectileSO projectile in projectiles)
            if (projectile.itemName == projectileName)
                return new(projectile);
        return null;
    }

    public Item GetShield(string shieldName)
    {
        foreach (ShieldSO shield in shields)
            if (shield.itemName == shieldName)
                return new(shield);
        return null;
    }

    public Item GetMaterial(string materialName)
    {
        foreach (MaterialSO materialSO in materials)
            if (materialSO.itemName == materialName)
                return new(materialSO);
        return null;
    }

    public Item GetItemByNameAndAmount(string name, int amount)
    {
        Item item = null;

        foreach (WeaponMeleeSO weapon in weaponsMelee)
            if (weapon.itemName == name)
                item = new(weapon);
        foreach (WeaponRangedSO weapon in weaponsRanged)
            if (weapon.itemName == name)
                item = new(weapon);
        foreach (ArmorSO armor in armors)
            if (armor.itemName == name)
                item = new(armor);
        foreach (BackpackSO backpack in backpacks)
            if (backpack.itemName == name)
                item = new(backpack);
        foreach (BeltSO belt in belts)
            if (belt.itemName == name)
                item = new(belt);
        foreach (ConsumableSO consumable in consumables)
            if (consumable.itemName == name)
                item = new(consumable);
        foreach (ProjectileSO projectile in projectiles)
            if (projectile.itemName == name)
                item = new(projectile);
        foreach (ShieldSO shield in shields)
            if (shield.itemName == name)
                item = new(shield);
        Debug.Log(item);
        item.amount = amount;
        return item;
    }
}
