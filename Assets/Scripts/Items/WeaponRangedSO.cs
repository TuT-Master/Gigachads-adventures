using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "WeaponRanged", menuName = "Scriptable objects/WeaponRanged")]
public class WeaponRangedSO : ItemSO
{
    public bool emitsLight;
    public bool fullAuto;

    public bool twoHanded;
    public bool AoE;

    public WeaponClass weaponClass;

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
    [SerializeField] private float currentMagazine = 0;
    [SerializeField] private float magazineSize = 0;
    [SerializeField] private float reloadTime = 0;
    [SerializeField] private float spread = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float rangeX = 1;
    [SerializeField] private float rangeY = 1;
    [SerializeField] private float defense = 0;
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
            {StatType.CurrentMagazine, currentMagazine},
            {StatType.MagazineSize, magazineSize},
            {StatType.ReloadTime, reloadTime},
            {StatType.Spread, spread},
            {StatType.SplashDamage, splashDamage},
            {StatType.SplashRadius, splashRadius},
            {StatType.RangeX, rangeX},
            {StatType.RangeY, rangeY},
            {StatType.BonusDefense, defense},
            {StatType.Weight, weight},
            {StatType.Price, price},
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public WeaponRangedSO upgradedVersionsOfWeapon;

    [Header("Ammo types")]
    public List<ProjectileSO> ammo;

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
