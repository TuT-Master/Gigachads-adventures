using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IInteractableEnemy
{
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float evasion;
    [SerializeField] private float defense;

    [SerializeField] private float expForKill;

    public bool isStunned;

    private EffectManager effectManager;


    void Start()
    {
        effectManager = FindAnyObjectByType<EffectManager>();
    }

    void Update()
    {
        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        FindAnyObjectByType<PlayerStats>().playerStats["exp"] += expForKill;

        Destroy(gameObject);
    }

    public bool CanInteract()
    {
        if (hp <= 0)
            return false;
        else
            return true;
    }

    public void HurtEnemy(float damage, float penetration, float armorIgnore)
    {
        float finalDamage;
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
