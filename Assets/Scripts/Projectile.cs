using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Item item;

    public bool alive = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!alive)
            return;

        if (other.gameObject.layer == 10)
        {
            Debug.Log("Hitting an enemy!");
            other.GetComponentInParent<IInteractableEnemy>().HurtEnemy(item.stats["damage"], item.stats["penetration"], item.stats["armorIgnore"], out float finalDamage);
            if (finalDamage > 0)
                FindAnyObjectByType<PlayerStats>().AddExp(item.weaponClass, finalDamage);
            if (item != null && item.stats["splashRadius"] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
        else
        {
            Debug.Log("Hitting an object!");
            if (item != null && item.stats["splashRadius"] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
    }
}
