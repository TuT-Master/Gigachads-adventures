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

    void FixedUpdate()
    {
        if (hp <= 0)
        {
            FindAnyObjectByType<PlayerStats>().playerStats["experience"] += expForKill;

            Destroy(gameObject);
        }
    }

    public bool CanInteract()
    {
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
            hp -= finalDamage;
            Debug.Log("Hitting enemy and dealing " + finalDamage + " damage to hp!");
        }
        else
            Debug.Log("Not enough power to break through enemy armor!");
    }

    public void StunEnemy(float seconds)
    {
        StartCoroutine(GetComponent<EnemyMovement>().Stun(seconds));
        isStunned = true;
    }
}
