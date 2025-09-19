using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

public class Projectile : MonoBehaviour
{
    public Item projectile;
    public Item weapon;

    public bool alive = false;

    private List<Enemy> enemies = new();


    private void OnTriggerEnter(Collider other)
    {
        if (!alive)
            return;

        if (other.transform.parent.TryGetComponent(out Enemy enemy) && !enemies.Contains(enemy) && other.gameObject.layer == 10)
        {
            enemies.Add(enemy);
            Debug.Log(enemy.ToString());

            float damage = weapon.stats[StatType.Damage] + projectile.stats[StatType.Damage];
            float penetration = weapon.stats[StatType.Penetration] + projectile.stats[StatType.Penetration];
            float armorIgnore = weapon.stats[StatType.ArmorIgnore] + projectile.stats[StatType.ArmorIgnore];
            float finalDamage = 0f;

            foreach(Enemy IEnemy in enemies)
            {
                IEnemy.ReceiveDamage(damage, penetration, armorIgnore, 1f, out float partialFinalDamage);
                finalDamage += partialFinalDamage;
            }

            if (finalDamage > 0)
                FindAnyObjectByType<PlayerStats>().AddExp(weapon, finalDamage);

            if (projectile.stats[StatType.SplashRadius] > 0 && projectile.stats[StatType.SplashDamage] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
        else
        {
            if (projectile != null && projectile.stats[StatType.SplashRadius] > 0)
            {
                // TODO - splash damage
            }
            else
                Destroy(gameObject);
        }
    }
}
