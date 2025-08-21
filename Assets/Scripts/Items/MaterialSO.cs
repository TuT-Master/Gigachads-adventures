using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable objects/Material")]
public class MaterialSO : ItemSO
{
    public Sprite sprite_inventory;

    public Item.MagicCrystalType crystalType;

    [Header("Stats")]
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;

    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "weight", weight},
            { "price", price},
        };
    }

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
