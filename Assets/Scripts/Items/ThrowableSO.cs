using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Throwable", menuName = "Scriptable objects/Throwable")]
public class ThrowableSO : ItemSO
{
    public bool emitsLight;

    public bool AoE;

    public WeaponClass weaponClass;

    public Sprite sprite_projectile;

    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float poisonDamage = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float burningChance = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float attackSpeed = 0;
    [SerializeField] private float critDamage = 0;
    [SerializeField] private float critChance = 0;
    [SerializeField] private float staminaCost = 0;
    [SerializeField] private float manaCost = 0;
    [SerializeField] private float spread = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float rangeX = 1;
    [SerializeField] private float rangeY = 1;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;
    public Dictionary<StatType, float> Stats()
    {
        return new Dictionary<StatType, float>()
        {
            {StatType.Damage, damage},
            {StatType.PoisonDamage, poisonDamage},
            {StatType.BleedingDamage, bleedingDamage},
            {StatType.BurningChance, burningChance},
            {StatType.Penetration, penetration},
            {StatType.ArmorIgnore, armorIgnore},
            {StatType.AttackSpeed, attackSpeed},
            {StatType.CritDamage, critDamage},
            {StatType.CritChance, critChance},
            {StatType.StaminaCost, staminaCost},
            {StatType.ManaCost, manaCost},
            {StatType.Spread, spread},
            {StatType.SplashDamage, splashDamage},
            {StatType.SplashRadius, splashRadius},
            {StatType.RangeX, rangeX},
            {StatType.RangeY, rangeY},
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
