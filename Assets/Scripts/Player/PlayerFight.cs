using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private BoxCollider weaponRange;

    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

    [Header("Effects")]
    [SerializeField]
    private EffectManager effectManager;
    [SerializeField]
    private Transform weaponEffectSpawnPoint;

    private Dictionary<Enemy, List<EnemyHitbox>> enemyList = new();
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
    private PlayerSkill playerSkill;

    private bool canUseConsumable = true;


    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerSkill = GetComponent<PlayerSkill>();
        canAttackAgain = true;
        reloading = false;
    }

    private void Update()
    {
        ActiveWeapon();
        MyInput();

        foreach (Enemy enemy in enemyList.Keys)
        {
            if (!enemy.CanInteract())
            {
                enemyList.Remove(enemy);
                return;
            }
        }
    }

    void MyInput()
    {
        if (GetComponent<HUDmanager>().AnyScreenOpen() | reloading)
            return;

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
                RangedAttack();
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

        // E - Use consumable
        if (Input.GetKeyDown(KeyCode.E))
            UseConsumable(GetComponent<PlayerToolbar>().GetActiveConsumable());

        // Space - Active skill
        if (Input.GetKeyDown(KeyCode.Space) && secondaryItemInHand != null && defending)
            UseActiveSkill(true);
        else if (Input.GetKeyDown(KeyCode.Space) && itemInHand != null && !defending)
            UseActiveSkill(false);
    }

    private void UseActiveSkill(bool shield)
    {
        if(shield && playerSkill.playerWeaponTypeSkillLevels[secondaryItemInHand.weaponType][PlayerSkill.SkillType.Active] > 0)
        {
            Debug.Log("Using " + secondaryItemInHand.itemName + "'s active skill!");
        }
        else if(!shield && playerSkill.playerWeaponTypeSkillLevels[itemInHand.weaponType][PlayerSkill.SkillType.Active] > 0)
        {
            Debug.Log("Using " + itemInHand.itemName + "'s active skill!");
        }
    }
    private void UseConsumable(Item consumable)
    {
        if (consumable == null | !canUseConsumable)
            return;

        canUseConsumable = false;

        playerStats.playerStats["hp"] += consumable.stats["hpRefill"];
        playerStats.playerStats["stamina"] += consumable.stats["staminaRefill"];
        playerStats.playerStats["mana"] += consumable.stats["manaRefill"];

        consumable.amount--;

        StartCoroutine(ConsumableCooldown(consumable.stats["cooldown"]));
    }
    private IEnumerator ConsumableCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseConsumable = true;
    }

    void Defend()
    {
        defending = true;
        playerStats.playerStats["stamina"] -= Time.deltaTime * 2;
    }

    void FistsAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;

        if (enemyList.Count > 0)
            enemyList.Keys.ToArray()[0].ReceiveDamage(
                fistsStats["damage"],
                fistsStats["penetration"],
                fistsStats["armorIgnore"],
                enemyList[enemyList.Keys.ToArray()[0]][0].damageMultiplier,
                out float _);

        StartCoroutine(CanAttackAgain());
    }

    void MeleeAttack()
    {
        if(!canAttackAgain)
            return;
        if(playerStats.playerStats["stamina"] - itemInHand.stats["staminaCost"] < 0)
        {
            Debug.Log("Not enough stamina for attack!");
            return;
        }
        canAttackAgain = false;
        playerStats.playerStats["stamina"] -= itemInHand.stats["staminaCost"];

        if (enemyList.Count > 0)
        {
            float finalDamage = 0f;
            if (itemInHand.AoE)
            {
                // AoE attack
            }
            else
            {
                // Get base stats of weapon
                float damage = itemInHand.stats["damage"];
                float petration = itemInHand.stats["penetration"];
                float armorIgnore = itemInHand.stats["armorIgnore"];

                // Add any bonuses to damage (skills, equipment)


                // Deal damage
                enemyList.Keys.ToArray()[0].ReceiveDamage(
                    damage,
                    petration,
                    armorIgnore,
                    enemyList[enemyList.Keys.ToArray()[0]][0].damageMultiplier,
                    out float _);
            }
            if (finalDamage > 0)
                playerStats.AddExp(itemInHand, finalDamage);
        }

        // Play animation


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

            projectile.GetComponent<Projectile>().projectile = itemInHand.ammo[0].ToItem();
            Vector3 victor = projectile.GetComponent<Projectile>().projectile.stats["projectileSpeed"] * new Vector3(VectorFromAngle(angle).z, 0.05f, VectorFromAngle(angle).x);

            projectile.GetComponent<Rigidbody>().mass = projectile.GetComponent<Projectile>().projectile.stats["weight"];
            projectile.GetComponent<Rigidbody>().AddForce(victor, ForceMode.Force);

            projectile.GetComponent<Projectile>().weapon = itemInHand;
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

    public Vector3 VectorFromAngle(float angle)
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

            List<Item> items = inventory.HasAmmo(itemInHand.ammo[0].itemName);

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
                    yield return new WaitForSeconds(1 / itemInHand.stats["attackSpeed"]);
                }
                else
                {
                    yield return new WaitForSeconds(itemInHand.stats["reloadTime"]);
                }

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
    }

    void ActiveWeapon()
    {
        if (itemInHand == null) // No active weapon -> fists as weapon
        {
            weaponRange.size = new Vector3(0.05f, 0.05f, fistsStats["rangeX"]);
            weaponRange.center = new Vector3(0, 0, weaponRange.size.z / 2);
            return;
        }
        if (itemInHand.slotType == (Slot.SlotType.WeaponMelee | Slot.SlotType.WeaponRanged)) // itemInHand is some weapon
        {
            weaponRange.size = new Vector3(0.05f, 0.05f, itemInHand.stats["rangeX"]);
            weaponRange.center = new Vector3(0, 0, weaponRange.size.z / 2);
            return;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            if (other.TryGetComponent(out EnemyHitbox enemyHitbox))
            {
                Enemy enemy = enemyHitbox.enemy;
                if (!enemyList.ContainsKey(enemy))
                    enemyList.Add(enemy, new List<EnemyHitbox> { enemyHitbox });
                else if (!enemyList[enemy].Contains(enemyHitbox))
                    enemyList[enemy].Add(enemyHitbox);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
            if (other.TryGetComponent(out EnemyHitbox enemyHitbox))
            {
                Enemy enemy = enemyHitbox.enemy;
                if (enemyList.ContainsKey(enemy) && enemyList[enemy].Contains(enemyHitbox))
                {
                    enemyList[enemy].Remove(enemyHitbox);
                    if (enemyList[enemy].Count == 0)
                        enemyList.Remove(enemy);
                }
            }
    }




    // Active skills
    public void DisarmEnemy()
    {

    }
    public void PoisonStain()
    {

    }
    public void GigaStunDamage()
    {

    }
    public void LungeAttack()
    {

    }
    public void HeadHit()
    {

    }
    public void ShieldBreak()
    {

    }
    public void AoEStun()
    {

    }
    public void ArmorDecrease()
    {

    }
    public void DamageBurst()
    {

    }
}