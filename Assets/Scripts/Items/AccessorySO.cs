using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Equipable", menuName = "Scriptable objects/Equipable")]
public class AccessorySO : ItemSO
{
    [Header("Stats")]
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Bonuses")]
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;


    public Dictionary<StatType, float> Stats()
    {
        return new()
        {
            {StatType.Weight, weight },
            {StatType.Price, price},
        };
    }
    public Dictionary<StatType, float> Bonus()
    {
        return new()
        {
            {StatType.HpMax, hpMax },
            {StatType.StaminaMax, staminaMax },
            {StatType.ManaMax, manaMax },
        };
    }

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
