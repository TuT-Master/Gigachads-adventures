using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IInteractableEnemy
{
    public string enemyName = "";
    public bool isChampion;

    [Header("Basic stats")]
    [SerializeField] private float hp;
    [SerializeField] private float stamina;
    [SerializeField] private float mana;
    private float hpMax;
    private float staminaMax;
    private float manaMax;

    [Header("Fight stats")]
    [SerializeField] private float armor;
    [SerializeField] private float evasion;
    [SerializeField] private float defense;
    [SerializeField] private float agroRange;
    [HideInInspector] public bool isStunned;

    private EffectManager effectManager;
    private float color = 255;



    void Start()
    {
        hpMax = hp;
        staminaMax = stamina;
        manaMax = mana;

        effectManager = FindAnyObjectByType<EffectManager>();
        if(!isChampion)
            enemyName = gameObject.name;
        transform.Find("AgroRange").GetComponent<SphereCollider>().radius = agroRange;
    }

    void Update()
    {
        if (hp <= 0)
            Die();

        if (color < 255)
        {
            color += 3;
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(255, color / 255f, color / 255f);
        }
        else
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void Die()
    {
        GetComponent<EnemyDropLoot>().DropLoot();
        Destroy(gameObject);
    }

    public bool CanInteract()
    {
        if (hp <= 0)
            return false;
        else
            return true;
    }

    public void PoisonDamage()
    {

    }

    public void BleedingDamage()
    {

    }

    public void HurtEnemy(float damage, float penetration, float armorIgnore, out float finalDamage)
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
            effectManager.SpawnStain(transform, EffectManager.StainType.Blood);

            // Play some sound


            // Hit animation
            color = 0f;
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;


            // Decrease hp
            hp -= finalDamage;
        }
        else
        {
            Debug.Log("Not enough power to break through enemy's armor!");
            // Play some sound
        }
    }

    public void StunEnemy(float seconds)
    {
        StartCoroutine(GetComponent<EnemyMovement>().Stun(seconds));
        isStunned = true;
    }
}
