using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable objects/Shield")]
public class ShieldSO : ItemSO
{
    public bool emitsLight;

    public WeaponClass weaponClass;

    [Header("Stats")]
    [SerializeField] private float defense = 0;
    [SerializeField] private float staminaPerBlock = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;

    public Dictionary<StatType, float> Stats()
    {
        return new()
        {
            {StatType.BonusDefense, defense},
            {StatType.StaminaCost, staminaPerBlock},
            {StatType.Weight, weight},
            {StatType.Price, price},
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public ShieldSO upgradedVersionsOfShield;

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
