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
                {"weight", 0f}};


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

    void MyInput()
    {
        // LMB
        if(itemInHand != null && Input.GetMouseButton(0))
        {
            if(itemInHand.fullAuto && itemInHand.slotType != Slot.SlotType.WeaponRanged)
            {
                // Full-auto weapons (only ranged weapons)
                RangedAttack();
            }
            else if (!itemInHand.fullAuto && Input.GetMouseButtonDown(0))
            {
                // Semi-auto weapons
                if (itemInHand.slotType == Slot.SlotType.WeaponMelee)
                    MeleeAttack();
                else if (itemInHand.slotType != Slot.SlotType.WeaponRanged)
                    RangedAttack();
            }
        }
        if (itemInHand == null && Input.GetMouseButtonDown(0))
            FistsAttack();

        // RMB
        if (Input.GetMouseButton(1))
        {
            // Consume equiped consumable?
        }
    }

    void FistsAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;
        if (enemyList.Count > 0)
        {
            enemyList[0].HurtEnemy(fistsStats["damage"]);
        }

        StartCoroutine(CanAttackAgain(fistsStats["attackSpeed"]));
    }

    void MeleeAttack()
    {
        if(!canAttackAgain)
            return;
        canAttackAgain = false;
        if (enemyList.Count > 0)
        {
            enemyList[0].HurtEnemy(itemInHand.stats["damage"]);
        }

        StartCoroutine(CanAttackAgain(itemInHand.stats["attackSpeed"]));
    }

    void RangedAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;

        StartCoroutine(CanAttackAgain(itemInHand.stats["attackSpeed"]));
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
            weaponRange.center = new Vector3(0, 0, weaponRange.height * 0.5f);
            return;
        }
        if (itemInHand.slotType == (Slot.SlotType.WeaponMelee | Slot.SlotType.WeaponRanged)) // itemInHand is some weapon
        {
            weaponRange.height = itemInHand.stats["rangeX"] * 2;
            weaponRange.radius = itemInHand.stats["rangeY"] * 0.5f;
            weaponRange.center = new Vector3(0, 0, weaponRange.height * 0.5f);
        }
        else if (itemInHand.slotType == Slot.SlotType.Consumable) // itemInHand is some consumable
        {

        }
    }


    void RotateCollider() { weaponRange.gameObject.transform.rotation = Quaternion.Euler(0, GetComponent<PlayerMovement>().angleRaw, 0); }
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