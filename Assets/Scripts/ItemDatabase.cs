using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : ScriptableObject
{
    public List<WeaponMeleeSO> weaponsMelee;

    public List<WeaponRangedSO> weaponsRanged;

    public List<ArmorSO> armors;

    public List<ConsumableSO> consumables;

    public List<ProjectileSO> projectiles;


    public WeaponMelee GetWeaponMelee(string weaponName)
    {
        foreach (WeaponMeleeSO weapon in weaponsMelee)
            if (weapon.itemName == weaponName)
                return new(weapon);
        return null;
    }
    public WeaponRanged GetWeaponRanged(string weaponName)
    {
        foreach (WeaponRangedSO weapon in weaponsRanged)
            if (weapon.itemName == weaponName)
                return new(weapon);
        return null;
    }
    public Armor GetArmor(string armorName)
    {
        foreach(ArmorSO armor in armors)
            if (armor.itemName == armorName)
                return new(armor);
        return null;
    }

    public Consumable GetConsumable(string consumableName)
    {
        foreach(ConsumableSO consumable in consumables)
            if(consumable.itemName == consumableName)
                return new(consumable);
        return null;
    }

    public Projectile GetProjectile(string projectileName)
    {
        foreach (ProjectileSO projectile in projectiles)
            if (projectile.itemName == projectileName)
                return new(projectile);
        return null;
    }
}
