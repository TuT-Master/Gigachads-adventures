using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    public Item itemInHand;

    private Dictionary<string, float> weaponStats;

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
        if(Input.GetMouseButton(0))
        {
            // LMB

        }
        else if (Input.GetMouseButton(1))
        {
            // RMB

        }
    }

    void ActiveWeapon()
    {
        if (itemInHand == null) // No active weapon -> fists as weapon
        {
            weaponStats = new(){
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
            return;
        }
        if (itemInHand.slotType == (Slot.SlotType.WeaponMelee | Slot.SlotType.WeaponRanged)) // itemInHand is some weapon
            weaponStats = itemInHand.stats;
        else if (itemInHand.slotType == Slot.SlotType.Consumable) // itemInHand is some consumable
        {

        }
    }
}