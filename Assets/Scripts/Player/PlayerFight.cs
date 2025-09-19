using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerStats;

public class PlayerFight : MonoBehaviour
{
    [HideInInspector] public Item activeWeapon;
    private Item previousActiveWeapon;
    [HideInInspector] public Item secondaryItemInHand;

    [HideInInspector] public bool canAttackAgain;
    [HideInInspector] public bool reloading;
    [HideInInspector] public bool defending;

    [Header("Colliders")]
    [SerializeField] private BoxCollider weaponRange;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Effects")]
    [SerializeField] private EffectManager effectManager;
    [SerializeField] private Transform weaponEffectSpawnPoint;

    [Header("UI items rendering")]
    [SerializeField] private Transform mainHand_transform;
    [SerializeField] private Transform secondHand_transform;
    [SerializeField] private Transform twoHanded_transform;
    private WeaponInHand mainHand_weapon;
    private WeaponInHand secondHand_weapon;



    private Dictionary<Enemy, List<EnemyHitbox>> enemyList = new();

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
        if (GetComponent<HUDmanager>().AnyScreenOpen() || reloading)
            return;

        // LMB - Attack
        if (Input.GetMouseButtonDown(0))
        {
            // Semi-auto weapons
            if (activeWeapon != null)
            {
                if (activeWeapon.slotType == Slot.SlotType.WeaponMelee)
                    MeleeAttack();
                else if (activeWeapon.slotType == Slot.SlotType.WeaponRanged)
                    RangedAttack();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            // Full-auto weapons (only ranged weapons)
            if (activeWeapon != null && activeWeapon.fullAuto && canAttackAgain)
                RangedAttack();
        }

        // RMB - Defending
        if (Input.GetMouseButton(1))
        {
            if(!defending)
                playerStats.playerStats[StatType.Speed] /= 2;
            Defend();
        }
        else
        {
            if (defending)
                playerStats.playerStats[StatType.Speed] *= 2;

            // Play animation
            if (defending)
                ToggleBlockAnimation(false);

            defending = false;
        }

        // Reload
        if (activeWeapon != null && activeWeapon.slotType == Slot.SlotType.WeaponRanged && !reloading)
            if (Input.GetKeyDown(KeyCode.R) | activeWeapon.stats[StatType.CurrentMagazine] == 0)
                StartCoroutine(Reload());

        // E - Use consumable
        if (Input.GetKeyDown(KeyCode.E))
            UseConsumable(GetComponent<PlayerToolbar>().GetActiveConsumable());

        // Space - Active skill
        if (Input.GetKeyDown(KeyCode.Space) && secondaryItemInHand != null && defending)
            UseActiveSkill(true);
        else if (Input.GetKeyDown(KeyCode.Space) && activeWeapon != null && !defending)
            UseActiveSkill(false);
    }

    private void UseActiveSkill(bool shield)
    {
        if(shield && playerSkill.playerWeaponTypeSkillLevels[secondaryItemInHand.itemType][PlayerSkill.SkillType.Active] > 0)
        {
            Debug.Log("Using " + secondaryItemInHand.itemName + "'s active skill!");
        }
        else if(!shield && playerSkill.playerWeaponTypeSkillLevels[activeWeapon.itemType][PlayerSkill.SkillType.Active] > 0)
        {
            Debug.Log("Using " + activeWeapon.itemName + "'s active skill!");
        }
    }
    private void UseConsumable(Item consumable)
    {
        if (consumable == null | !canUseConsumable)
            return;

        canUseConsumable = false;

        playerStats.playerStats[StatType.Hp] += consumable.stats[StatType.HpRegen];
        playerStats.playerStats[StatType.Stamina] += consumable.stats[StatType.StaminaRegen];
        playerStats.playerStats[StatType.Mana] += consumable.stats[StatType.ManaRegen];

        consumable.amount--;

        StartCoroutine(ConsumableCooldown(consumable.stats[StatType.Cooldown]));
    }
    private IEnumerator ConsumableCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseConsumable = true;
    }

    private void Defend()
    {
        playerStats.playerStats[StatType.Stamina] -= Time.deltaTime * 2;

        // Player animation
        if (!defending)
            ToggleBlockAnimation(true);

        defending = true;
    }
    private void ToggleBlockAnimation(bool toggle) => mainHand_weapon.PlayAnimation(toggle ? WeaponInHand.AnimationType.BlockStart : WeaponInHand.AnimationType.BlockEnd);

    private void MeleeAttack()
    {
        if(!canAttackAgain)
            return;

        if(playerStats.playerStats[StatType.Stamina] - activeWeapon.stats[StatType.StaminaCost] < 0)
        {
            Debug.Log("Not enough stamina for attack!");
            return;
        }
        canAttackAgain = false;
        playerStats.playerStats[StatType.Stamina] -= activeWeapon.stats[StatType.StaminaCost];

        if (enemyList.Count > 0)
        {
            float finalDamage = 0f;
            if (activeWeapon.AoE)
            {
                // AoE attack
            }
            else
            {
                // Get base stats of weapon
                float damage = activeWeapon.stats[StatType.Damage];
                float petration = activeWeapon.stats[StatType.Penetration];
                float armorIgnore = activeWeapon.stats[StatType.ArmorIgnore];

                // Add any bonuses to damage (skills, equipment)


                // Deal damage
                enemyList.Keys.ToArray()[0].ReceiveDamage(
                    damage,
                    petration,
                    armorIgnore,
                    enemyList[enemyList.Keys.ToArray()[0]][0].damageMultiplier,
                    out finalDamage);
            }
            if (finalDamage > 0)
                playerStats.AddExp(activeWeapon, finalDamage);
        }

        // Play animation
        mainHand_weapon.PlayAnimation(WeaponInHand.AnimationType.Attack);

        StartCoroutine(CanAttackAgain());
    }

    private void RangedAttack()
    {
        if (!canAttackAgain)
            return;
        canAttackAgain = false;


        if (activeWeapon.stats[StatType.CurrentMagazine] > 0)
        {
            activeWeapon.stats[StatType.CurrentMagazine]--;

            float angle = GetComponent<PlayerMovement>().angleRaw + UnityEngine.Random.Range(-activeWeapon.stats[StatType.Spread], activeWeapon.stats[StatType.Spread]);
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.Euler(0, angle, 0));

            projectile.GetComponent<Projectile>().projectile = activeWeapon.ammo[0].ToItem();
            Vector3 victor = projectile.GetComponent<Projectile>().projectile.stats[StatType.ProjectileSpeed] * new Vector3(VectorFromAngle(angle).z, 0.05f, VectorFromAngle(angle).x);

            projectile.GetComponent<Rigidbody>().mass = projectile.GetComponent<Projectile>().projectile.stats[StatType.Weight];
            projectile.GetComponent<Rigidbody>().AddForce(victor, ForceMode.Force);

            projectile.GetComponent<Projectile>().weapon = activeWeapon;
            projectile.GetComponent<Projectile>().alive = true;

            StartCoroutine(CanAttackAgain());
        }
        else
        {
            Debug.Log("No ammo!");

            if (activeWeapon.stats[StatType.MagazineSize] == 1)
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

    private IEnumerator CanAttackAgain()
    {
        if (activeWeapon != null)
            yield return new WaitForSeconds(activeWeapon.stats[StatType.AttackSpeed]);
        canAttackAgain = true;
    }

    private IEnumerator Reload()
    {
        if (activeWeapon.stats[StatType.CurrentMagazine] < activeWeapon.stats[StatType.MagazineSize])
        {
            reloading = true;
            PlayerInventory inventory = GetComponent<PlayerInventory>();


            // TODO - Choose ammo

            List<Item> items = inventory.HasAmmo(activeWeapon.ammo[0].itemName);

            // Find ammo Items in inventory
            List<Item> chosenItems = new();
            bool done = false;
            int ammoCounter = 0;
            foreach (Item item in items)
            {
                if(!done)
                {
                    if (item.amount >= activeWeapon.stats[StatType.MagazineSize])
                    {
                        chosenItems.Add(item);
                        done = true;
                    }
                    else if (item.amount < activeWeapon.stats[StatType.MagazineSize] && !done)
                    {
                        chosenItems.Add(item);
                        ammoCounter += item.amount;
                        if(ammoCounter >= activeWeapon.stats[StatType.MagazineSize])
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
                if (activeWeapon.stats[StatType.MagazineSize] == 1)
                {
                    yield return new WaitForSeconds(1 / activeWeapon.stats[StatType.AttackSpeed]);
                }
                else
                {
                    yield return new WaitForSeconds(activeWeapon.stats[StatType.ReloadTime]);
                }

                // Reload
                for (int i = 0; i < chosenItems.Count; i++)
                {
                    if (activeWeapon.stats[StatType.CurrentMagazine] < activeWeapon.stats[StatType.MagazineSize])
                    {
                        if (chosenItems[i].amount >= activeWeapon.stats[StatType.MagazineSize] - activeWeapon.stats[StatType.CurrentMagazine])
                        {
                            chosenItems[i].amount -= (int)(activeWeapon.stats[StatType.MagazineSize] - activeWeapon.stats[StatType.CurrentMagazine]);
                            activeWeapon.stats[StatType.CurrentMagazine] = activeWeapon.stats[StatType.MagazineSize];
                        }
                        else
                        {
                            activeWeapon.stats[StatType.CurrentMagazine] += chosenItems[i].amount;
                            chosenItems[i].amount = 0;
                        }
                    }
                }
                canAttackAgain = true;
            }
            reloading = false;
        }
    }

    public void ActiveWeapon(Item activeItem)
    {
        activeWeapon = activeItem;

        if (activeWeapon == previousActiveWeapon)
            return;

        if (mainHand_transform.childCount > 0)
            Destroy(mainHand_transform.GetChild(0).gameObject);
        if (twoHanded_transform.childCount > 0)
            Destroy(twoHanded_transform.GetChild(0).gameObject);

        if (activeWeapon.slotType == Slot.SlotType.WeaponMelee || activeWeapon.slotType == Slot.SlotType.WeaponRanged) // itemInHand is some weapon
        {
            weaponRange.size = new Vector3(0.05f, 0.05f, activeWeapon.stats[StatType.RangeX]);
            weaponRange.center = new Vector3(0, 0, weaponRange.size.z / 2);

            if (activeWeapon.itemModel == null) return;

            Debug.Log("Set mainHand weapon to " + activeWeapon.itemName);

            GameObject item = Instantiate(activeWeapon.itemModel, activeWeapon.twoHanded ? twoHanded_transform : mainHand_transform);
            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            mainHand_weapon = item.GetComponent<WeaponInHand>();

            previousActiveWeapon = activeWeapon;
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