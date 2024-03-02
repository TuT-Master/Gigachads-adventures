using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IInteractableEnemy
{
    [SerializeField]
    private float hp;
    [SerializeField]
    private float armor;
    [SerializeField]
    private float evasion;

    public bool isStunned;

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public void HurtEnemy(float damage)
    {
        hp -= damage;
    }

    public void StunEnemy(float seconds)
    {
        StartCoroutine(GetComponent<EnemyMovement>().Stun(seconds));
        isStunned = true;
    }
}
