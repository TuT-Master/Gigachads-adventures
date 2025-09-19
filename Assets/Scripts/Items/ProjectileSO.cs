using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable objects/Projectile")]
public class ProjectileSO : ItemSO
{
    public Sprite sprite_projectile;

    [Header("Stats")]
    public bool selfHoming;
    [SerializeField] private float damage = 0;
    [SerializeField] private float poisonDamage = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float burningChance = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float piercing = 0;
    [SerializeField] private float projectileSpeed = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;
    public Dictionary<StatType, float> Stats()
    {
        return new()
        {
            {StatType.Damage, damage },
            {StatType.PoisonDamage, poisonDamage},
            {StatType.BleedingDamage, bleedingDamage},
            {StatType.BurningChance, burningChance},
            {StatType.Penetration, penetration },
            {StatType.ArmorIgnore, armorIgnore },
            {StatType.Piercing, piercing },
            {StatType.ProjectileSpeed, projectileSpeed },
            {StatType.SplashDamage, splashDamage },
            {StatType.SplashRadius, splashRadius},
            {StatType.Weight, weight},
            {StatType.Price, price},
        };
    }

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
