using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using static Item;

public class Enemy : MonoBehaviour
{
    public string enemyName = "";
    public bool isChampion;
    [SerializeField] private bool rndChampionFirstName;
    [SerializeField] private bool rndChampionLastName;
    private DungeonDatabase dungeonDatabase;
    [HideInInspector] public Animator animator;

    [Header("Basic stats")]
    [SerializeField] private float hpMax;
    private float hp;

    [Header("Movement")]
    [SerializeField] private float speed;
    private bool getNewRandomPoint;
    [HideInInspector] public NavMeshAgent agent;

    [Header("Fight stats")]
    private PlayerStats playerStats;
    [HideInInspector] public GameObject target;
    private Item weapon;
    [SerializeField] private ItemSO enemyWeapon;
    [SerializeField] private SphereCollider agroCollider;
    [SerializeField] private float damage;
    [SerializeField] private float penetration;
    [SerializeField] private float armorIgnore;
    [SerializeField] private float armor;
    [SerializeField] private float evasion;
    [SerializeField] private float defense;
    [SerializeField] private float agroRange;
    [HideInInspector] public bool canAttackAgain;
    [HideInInspector] public bool isStunned;


    // Stats
    public void SetStats()
    {
        // Dependencies
        playerStats = FindAnyObjectByType<PlayerStats>();
        agent = GetComponent<NavMeshAgent>();
        dungeonDatabase = FindObjectOfType<DungeonDatabase>();
        animator = GetComponent<Animator>();

        // Stats
        hp = hpMax;
        agent.speed = speed;

        // Name
        if (isChampion)
        {
            StringBuilder sb = new(enemyName);
            if (rndChampionFirstName)
                sb.Append(dungeonDatabase.GetRandomFirstChampionName());
            if(rndChampionLastName)
                sb.Append(dungeonDatabase.GetRandomLastChampionName());
            enemyName = sb.ToString();
        }
        else
            enemyName = gameObject.name;

        // Attack
        agroCollider.radius = agroRange;
        weapon = enemyWeapon != null ? enemyWeapon.ToItem() : null;

        // Setting bools
        canAttackAgain = true;
        getNewRandomPoint = true;
    }
    public void CheckHealth()
    {
        if (hp <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }


    // Interacting
    public bool CanInteract()
    {
        if (hp <= 0)
            return false;
        else
            return true;
    }


    // Receiving damage and effects
    public void ReceivePoisonDamage()
    {

    }
    public void ReceiveBleedingDamage()
    {

    }
    public void StunEnemy(float seconds)
    {
        isStunned = true;
        StartCoroutine(Stun(seconds));
    }
    public void ReceiveDamage(float damage, float penetration, float armorIgnore, float damageMultipler, out float finalDamage)
    {
        float armorTemp = armor;

        if (armorIgnore > 0)
            armorTemp *= armorIgnore;

        if (armorTemp - penetration > 0)
            finalDamage = damage - (armorTemp - penetration);
        else
            finalDamage = damage;

        if (defense > 0)
            finalDamage *= defense / 100;

        if (finalDamage > 0)
        {
            Debug.Log("Hitting enemy and dealing " + finalDamage + " damage to hp!");
            // Spawn blood stain


            // Play some sound


            // Decrease hp
            hp -= finalDamage;
        }
        else
        {
            Debug.Log("Not enough power to break through enemy's armor!");
            // Play some sound
        }
    }


    // Loot
    public void DropLoot()
    {

    }
    private int GetRandomItemAmount()
    {
        return new System.Random().Next(0, 100) switch
        {
            <= 20 => 1,
            > 20 and <= 65 => 2,
            > 65 and <= 90 => 3,
            > 90 => 4
        };
    }


    // Attack
    public bool CanAttack()
    {
        return target != null && canAttackAgain && !isStunned;
    }
    public void Attack()
    {
        if (target == null)
            return;

        canAttackAgain = false;

        if (weapon != null)
            playerStats.DealDamage(weapon.stats["damage"], weapon.stats["penetration"], weapon.stats["armorIgnore"]);
        else
            playerStats.DealDamage(damage, penetration, armorIgnore);

        StopMovement();
        StartCoroutine(AttackAgain());
    }
    private IEnumerator AttackAgain()
    {
        yield return new WaitForSeconds(1f);
        ResumeMovement();
        canAttackAgain = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
            target = other.transform.parent.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
            target = null;
    }


    // Movement
    private IEnumerator GetRandomPointDelay()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        getNewRandomPoint = true;
    }
    Vector3 GetRandomPoint()
    {
        return new(transform.position.x + Random.Range(-10, 10), 0, transform.position.z + Random.Range(-10, 10));
    }
    private IEnumerator Stun(float seconds)
    {
        float defaultSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(seconds);
        speed = defaultSpeed;
        isStunned = false;
    }
    public void PlayWalkAnimation()
    {
        if(agent.velocity.magnitude > 0.01f && !isStunned)
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);
    }
    public void StopMovement() => agent.speed = 0f;
    public void ResumeMovement() => agent.speed = speed;
}
