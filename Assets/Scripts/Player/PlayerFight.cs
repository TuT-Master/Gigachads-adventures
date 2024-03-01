using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    public Item itemInHand;

    [HideInInspector]
    public bool canAttackAgain;
    [HideInInspector]
    public bool reloading;

    [SerializeField]
    private CapsuleCollider weaponRange;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

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
        reloading = false;
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
        if(itemInHand != null && Input.GetMouseButtonDown(0))
        {
            if (itemInHand.fullAuto && Input.GetMouseButton(0))
            {
                // Full-auto weapons (only ranged weapons)
                RangedAttack();
            }
            else if (!itemInHand.fullAuto)
            {
                // Semi-auto weapons
                if (itemInHand.slotType == Slot.SlotType.WeaponMelee)
                    MeleeAttack();
                else if (itemInHand.slotType == Slot.SlotType.WeaponRanged)
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

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && itemInHand != null && itemInHand.slotType == Slot.SlotType.WeaponRanged && !reloading)
            StartCoroutine(Reload());
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

        StartCoroutine(CanAttackAgain());
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

        StartCoroutine(CanAttackAgain());
    }

    void RangedAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;

        Debug.Log("Firing!");
        itemInHand.stats["currentMagazine"]--;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.Euler(0, GetComponent<PlayerMovement>().angleRaw, 0));
        projectile.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new(0, 0, 0)), ForceMode.VelocityChange);
        projectile.GetComponent<Projectile>().item = new(itemInHand.ammo[0]);

        if (itemInHand.stats["currentMagazine"] > 0)
            StartCoroutine(CanAttackAgain());
        else
            StartCoroutine(Reload());
    }

    IEnumerator CanAttackAgain()
    {
        if (itemInHand != null)
            yield return new WaitForSeconds(1 / itemInHand.stats["attackSpeed"]);
        else
            yield return new WaitForSeconds(1 / fistsStats["attackSpeed"]);
        canAttackAgain = true;
    }

    IEnumerator Reload()
    {
        if (itemInHand.stats["currentMagazine"] < itemInHand.stats["magazineSize"])
        {
            reloading = true;
            PlayerInventory inventory = GetComponent<PlayerInventory>();


            // TODO - Choose right ammo

            List<Item> items = inventory.HasItem(itemInHand.ammo[0].itemName);

            // Find ammo Items in inventory
            List<Item> chosenItems = new();
            bool done = false;
            int ammoCounter = 0;
            foreach (Item item in items)
            {
                if(!done)
                {
                    if (item.amount >= itemInHand.stats["magazineSize"])
                    {
                        chosenItems.Add(item);
                        done = true;
                    }
                    else if (item.amount < itemInHand.stats["magazineSize"] && !done)
                    {
                        chosenItems.Add(item);
                        ammoCounter += item.amount;
                        if(ammoCounter >= itemInHand.stats["magazineSize"])
                            done = true;
                    }
                }
            }
            if (!done)
            {
                canAttackAgain = false;
                Debug.Log("No ammo left!");
            }
            else
            {
                // Wait for reload
                if (itemInHand.stats["magazineSize"] == 1)
                {
                    animator.SetFloat("SpeedMultiplier", 1 / itemInHand.stats["attackSpeed"]);
                    animator.SetTrigger("Reload");
                    yield return new WaitForSeconds(1 / itemInHand.stats["attackSpeed"]);
                }
                else
                {
                    animator.SetFloat("SpeedMultiplier", itemInHand.stats["reloadTime"]);
                    animator.SetTrigger("Reload");
                    yield return new WaitForSeconds(itemInHand.stats["reloadTime"]);
                }

                // Reload
                for(int i = 0; i < chosenItems.Count; i++)
                {
                    if (itemInHand.stats["currentMagazine"] < itemInHand.stats["magazineSize"])
                    {
                        if (chosenItems[i].amount >= itemInHand.stats["magazineSize"] - itemInHand.stats["currentMagazine"])
                        {
                            chosenItems[i].amount -= (int)(itemInHand.stats["magazineSize"] - itemInHand.stats["currentMagazine"]);
                            itemInHand.stats["currentMagazine"] = itemInHand.stats["magazineSize"];
                        }
                        else
                        {
                            itemInHand.stats["currentMagazine"] += chosenItems[i].amount;
                            chosenItems[i].amount = 0;
                        }
                    }
                }

                canAttackAgain = true;
            }
            reloading = false;
        }
        else
            Debug.Log("No need to reload. Your ammo magazine is full!");
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