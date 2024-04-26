using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    [HideInInspector]
    public Item itemInHand;
    [HideInInspector]
    public Item secondaryItemInHand;

    [HideInInspector]
    public bool canAttackAgain;
    [HideInInspector]
    public bool reloading;
    [HideInInspector]
    public bool defending;

    [Header("Colliders")]
    [SerializeField]
    private GameObject freeRotation;
    [SerializeField]
    private CapsuleCollider weaponRange;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [Header("Projectile")]
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
                {"defense", 10f },
                {"weight", 0f}
    };

    private PlayerStats playerStats;


    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        canAttackAgain = true;
        reloading = false;
    }

    private void Update()
    {
        ActiveWeapon();
        MyInput();
        FreeRotation();
        animator.gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void MyInput()
    {
        if (GetComponent<HUDmanager>().AnyScreenOpen() | reloading)
            return;

        // Adjust height of projectile spawn point
        if (Input.GetAxis("Adjust projectile height") > 0)
            projectileSpawnPoint.localPosition = new(0, 0.5f, 0.6f);
        else if(Input.GetAxis("Adjust projectile height") < 0)
            projectileSpawnPoint.localPosition = new(0, 0.1f, 0.6f);
        else
            projectileSpawnPoint.localPosition = new(0, 0.25f, 0.6f);

        // LMB - Attack
        if (Input.GetMouseButtonDown(0))
        {
            // Semi-auto weapons
            if (itemInHand != null)
            {
                if (itemInHand.slotType == Slot.SlotType.WeaponMelee)
                    MeleeAttack();
                else if (itemInHand.slotType == Slot.SlotType.WeaponRanged)
                    RangedAttack();
            }
            else
                FistsAttack();
        }
        else if (Input.GetMouseButton(0))
        {
            // Full-auto weapons (only ranged weapons)
            if (itemInHand != null && itemInHand.fullAuto && canAttackAgain)
            {
                Debug.Log("Full-auto range attack!");
                RangedAttack();
            }
        }

        // RMB - Defending
        if (Input.GetMouseButton(1))
        {
            if(!defending)
                playerStats.playerStats["speed"] /= 2;
            Defend();
        }
        else
        {
            if (defending)
                playerStats.playerStats["speed"] *= 2;
            defending = false;
        }

        // Reload
        if (itemInHand != null && itemInHand.slotType == Slot.SlotType.WeaponRanged && !reloading)
            if (Input.GetKeyDown(KeyCode.R) | itemInHand.stats["currentMagazine"] == 0)
                StartCoroutine(Reload());
    }

    void Defend()
    {
        Debug.Log("Defending");
        defending = true;
        // Decrease stamina
        playerStats.playerStats["stamina"] -= Time.deltaTime * 2;

    }

    void FistsAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;
        if (enemyList.Count > 0)
            enemyList[0].HurtEnemy(fistsStats["damage"], fistsStats["penetration"], fistsStats["armorIgnore"]);

        StartCoroutine(CanAttackAgain());
    }

    void MeleeAttack()
    {
        if(!canAttackAgain)
            return;
        canAttackAgain = false;

        if (enemyList.Count > 0)
        {
            if (itemInHand.AoE)
            {
                // AoE attack
            }
            else
            {
                float damage = itemInHand.stats["damage"];
                float petration = itemInHand.stats["penetration"];
                float armorIgnore = itemInHand.stats["armorIgnore"];
                // Add any bonuses to damage (skills, equipment)

                enemyList[0].HurtEnemy(damage, petration, armorIgnore);
            }
        }

        StartCoroutine(CanAttackAgain());
    }

    void RangedAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;


        if (itemInHand.stats["currentMagazine"] > 0)
        {
            itemInHand.stats["currentMagazine"]--;

            float angle = GetComponent<PlayerMovement>().angleRaw + UnityEngine.Random.Range(-itemInHand.stats["spread"], itemInHand.stats["spread"]);
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.Euler(0, angle, 0));

            projectile.GetComponent<Projectile>().item = new(itemInHand.ammo[0]);
            Vector3 victor = projectile.GetComponent<Projectile>().item.stats["projectileSpeed"] * new Vector3(VectorFromAngle(angle).z, 0.05f, VectorFromAngle(angle).x);

            projectile.GetComponent<Rigidbody>().mass = projectile.GetComponent<Projectile>().item.stats["weight"];
            projectile.GetComponent<Rigidbody>().AddForce(victor, ForceMode.Force);

            projectile.GetComponent<Projectile>().alive = true;

            StartCoroutine(CanAttackAgain());
        }
        else
        {
            Debug.Log("No ammo!");

            if (itemInHand.stats["magazineSize"] == 1)
                StartCoroutine(Reload());
            else
                canAttackAgain = true;
        }
    }

    Vector3 VectorFromAngle(float angle)
    {
        angle = (angle + 90) * (float)Math.PI / 180;
        return new((float)Math.Sin(angle), 0, -(float)Math.Cos(angle));
    }

    IEnumerator CanAttackAgain()
    {
        if (itemInHand != null)
            yield return new WaitForSeconds(itemInHand.stats["attackSpeed"]);
        else
            yield return new WaitForSeconds(fistsStats["attackSpeed"]);
        canAttackAgain = true;
    }

    IEnumerator Reload()
    {
        if (itemInHand.stats["currentMagazine"] < itemInHand.stats["magazineSize"])
        {
            reloading = true;
            PlayerInventory inventory = GetComponent<PlayerInventory>();


            // TODO - Choose ammo

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
            if (ammoCounter > 0)
                done = true;
            if (!done)
                canAttackAgain = false;
            else
            {
                // Wait for reload
                if (itemInHand.stats["magazineSize"] == 1)
                {
                    animator.SetFloat("SpeedMultiplier", itemInHand.stats["attackSpeed"]);
                    animator.SetTrigger("Reload");
                    yield return new WaitForSeconds(1 / itemInHand.stats["attackSpeed"]);
                }
                else
                {
                    animator.SetFloat("SpeedMultiplier", 1 / itemInHand.stats["reloadTime"]);
                    animator.SetTrigger("Reload");
                    yield return new WaitForSeconds(itemInHand.stats["reloadTime"]);
                }
                Debug.Log("Reloaded!");

                // Reload
                for (int i = 0; i < chosenItems.Count; i++)
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


    void FreeRotation() { freeRotation.transform.rotation = Quaternion.Euler(0, GetComponent<PlayerMovement>().angleRaw, 0); }
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