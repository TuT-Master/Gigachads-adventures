using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Item projectile;
    public Item weapon;

    public bool alive = false;

    private List<IInteractableEnemy> enemies = new();


    private void OnTriggerEnter(Collider other)
    {
        if (!alive)
            return;

        if (other.transform.parent.TryGetComponent(out IInteractableEnemy enemy) && !enemies.Contains(enemy) && other.gameObject.layer == 10)
        {
            enemies.Add(enemy);
            Debug.Log(enemy.ToString());

            float damage = weapon.stats["damage"] + projectile.stats["damage"];
            float penetration = weapon.stats["penetration"] + projectile.stats["penetration"];
            float armorIgnore = weapon.stats["armorIgnore"] + projectile.stats["armorIgnore"];
            float finalDamage = 0f;

            foreach(IInteractableEnemy IEnemy in enemies)
            {
                IEnemy.HurtEnemy(damage, penetration, armorIgnore, out float partialFinalDamage);
                finalDamage += partialFinalDamage;
            }

            if (finalDamage > 0)
                FindAnyObjectByType<PlayerStats>().AddExp(weapon.weaponClass, finalDamage);

            if (projectile.stats["splashRadius"] > 0 && projectile.stats["splashDamage"] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
        else
        {
            if (projectile != null && projectile.stats["splashRadius"] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
    }
}
