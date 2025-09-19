using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable objects/Armor")]
public class ArmorSO : ItemSO
{
    public bool hideHairWhenEquiped;
    public bool hideBeardWhenEquiped;
    public bool hideBodyWhenEquiped;

    [Header("Stats")]
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private float bleedingResistance;
    [SerializeField] private float poisonResistance;
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Full-set bonus")]
    public string armorSetName;
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;

    public Dictionary <StatType, float> Stats()
    {
        return new()
        {
            {StatType.Armor, armor },
            {StatType.MagicResistance, magicResistance },
            {StatType.BleedingResistance, bleedingResistance },
            {StatType.PoisonResistance, poisonResistance },
            {StatType.Weight, weight },
            {StatType.Price, price},
        };
    }
    public Dictionary<StatType, float> FullsetBonus()
    {
        return new()
        {
            {StatType.HpMax, hpMax },
            {StatType.StaminaMax, staminaMax },
            {StatType.ManaMax, manaMax },
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public ArmorSO upgradedVersionsOfArmor;

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
