using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable objects/Material")]
public class MaterialSO : ItemSO
{

    public Item.MagicCrystalType crystalType;

    [Header("Stats")]
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;

    public Dictionary<StatType, float> Stats()
    {
        return new()
        {
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
