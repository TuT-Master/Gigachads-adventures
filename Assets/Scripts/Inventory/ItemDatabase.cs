using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item database", menuName = "Scriptable objects/Item database")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponMeleeSO> weaponsMelee;

    public List<WeaponRangedSO> weaponsRanged;

    public List<ArmorSO> armors;

    public List<ConsumableSO> consumables;

    public List<ProjectileSO> projectiles;


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
}
