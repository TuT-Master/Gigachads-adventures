using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable objects/Consumable")]
public class ConsumableSO : ItemSO
{
    [Header("Stats")]
    [SerializeField] private float hpRefill;
    [SerializeField] private float staminaRefill;
    [SerializeField] private float manaRefill;
    [SerializeField] private float cooldown;
    [SerializeField] private float weight;
    [SerializeField] private float price;


    public Dictionary<StatType, float> Stats()
    {
        return new()
        {
            {StatType.HpRegen, hpRefill},
            {StatType.StaminaRegen, staminaRefill},
            {StatType.ManaRegen, manaRefill},
            {StatType.Cooldown, cooldown},
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
