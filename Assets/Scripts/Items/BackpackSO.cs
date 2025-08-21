using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Backpack", menuName = "Scriptable objects/Backpack")]
public class BackpackSO : ItemSO
{
    public Sprite sprite_inventory;

    [Header("Stats")]
    public int inventoryCapacity;
    [SerializeField] private float weight;
    [SerializeField] private float price;

    public Dictionary<string, float> Stats()
    {
        return new()
        {
            {"backpackSize", inventoryCapacity },
            {"weight", weight},
            {"price", price},
        };
    }

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
