using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Trap", menuName = "Scriptable objects/Trap")]
public class TrapSO : ItemSO
{
    public bool emitsLight;

    public bool AoE;

    public WeaponClass weaponClass;

    public GameObject model;

    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float poisonDamage = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float burningChance = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float knockback = 0;
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
            {StatType.Knockback, knockback},
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
