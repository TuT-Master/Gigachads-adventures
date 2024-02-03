using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    public WeaponMelee meleeWeapon;
    public WeaponRanged rangedWeapon;

    private Dictionary<string, float> stats;

    [HideInInspector]
    public bool canAttackAgain;





    private void Start()
    {
        canAttackAgain = true;
    }

    private void Update()
    {
        ActiveWeapon();
        MyInput();
    }

    void MyInput()
    {
        
    }

    void ActiveWeapon()
    {
        if (meleeWeapon != null && rangedWeapon == null) // Melee weapon active
            stats = meleeWeapon.stats;
        else if (rangedWeapon != null && meleeWeapon == null) // Ranged weapon active
            stats = rangedWeapon.stats;
        else if (meleeWeapon == null && rangedWeapon == null) // No weapon active -> fists as weapon
        {
            stats = new(){
                {"damage", 2f},
                {"penetration", 0f},
                {"armorIgnore", 0f},
                {"attackSpeed", 1.5f},
                {"staminaCost", 5f},
                {"manaCost", 0f},
                {"rangeX", 0.75f},
                {"rangeY", 0.75f},
                {"AoE", 0f},
                {"twoHanded", 0f},
                {"weight", 0f},
            };
        }
        else // Error
        {
            meleeWeapon = null;
            rangedWeapon = null;
        }
    }
}