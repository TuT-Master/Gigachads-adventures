using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    public Item itemInHand;

    [HideInInspector]
    public bool canAttackAgain;

    [SerializeField]
    private CapsuleCollider weaponRange;
    private List<IInteractableEnemy> enemyList = new();
    private Dictionary<string, float> fistsStats = new(){
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


    private void Start()
    {
        canAttackAgain = true;
    }

    private void Update()
    {
        ActiveWeapon();
        RotateCollider();
        MyInput();
    }

    void RotateCollider()
    {
        weaponRange.gameObject.transform.rotation = Quaternion.Euler(0, GetComponent<PlayerMovement>().angleRaw, 0);
    }

    void MyInput()
    {
        if(Input.GetMouseButton(0))
        {
            // LMB
            if (itemInHand != null)
            {
                if (itemInHand.slotType == Slot.SlotType.WeaponMelee)
                    MeleeAttack();
                else if (itemInHand.slotType != Slot.SlotType.WeaponRanged)
                    RangedAttack();
            }
            else
                FistsAttack();
        }
        else if (Input.GetMouseButton(1))
        {
            // RMB

        }
    }

    void FistsAttack()
    {
        if (!canAttackAgain)
            return;

        if (enemyList.Count > 0)
        {
            enemyList[0].HurtEnemy(fistsStats["damage"]);
        }
    }
    void MeleeAttack()
    {
        if(!canAttackAgain)
            return;

        if(enemyList.Count > 0)
        {
            enemyList[0].HurtEnemy(itemInHand.stats["damage"]);
        }
    }

    void RangedAttack()
    {
        if (!canAttackAgain)
            return;

    }

    IEnumerator CanAttackAgain(float atcSpeed)
    {
        yield return new WaitForSeconds(1 / atcSpeed);
        canAttackAgain = true;
    }

    void ActiveWeapon()
    {
        if (itemInHand == null) // No active weapon -> fists as weapon
        {
            weaponRange.height = fistsStats["rangeX"] * 2;
            weaponRange.radius = fistsStats["rangeY"] * 0.5f;
            weaponRange.center = new Vector3(0, 0, (weaponRange.height - 1) * 0.5f);
            return;
        }
        if (itemInHand.slotType == (Slot.SlotType.WeaponMelee | Slot.SlotType.WeaponRanged)) // itemInHand is some weapon
        {
            weaponRange.height = itemInHand.stats["rangeX"] * 2;
            weaponRange.radius = itemInHand.stats["rangeY"] * 0.5f;
            weaponRange.center = new Vector3(0, 0, (weaponRange.height - 1) * 0.5f);
        }
        else if (itemInHand.slotType == Slot.SlotType.Consumable) // itemInHand is some consumable
        {

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<IInteractableEnemy>();
        if (enemy != null && enemy.CanInteract())
            enemyList.Add(enemy);
    }
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponentInParent<IInteractableEnemy>();
        if (enemyList.Contains(enemy))
            enemyList.Remove(enemy);
    }
}