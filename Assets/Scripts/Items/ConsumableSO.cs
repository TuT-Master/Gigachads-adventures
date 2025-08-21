using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable objects/Consumable")]
public class ConsumableSO : ItemSO
{
    public Sprite sprite_inventory;

    [Header("Stats")]
    [SerializeField] private float hpRefill;
    [SerializeField] private float staminaRefill;
    [SerializeField] private float manaRefill;
    [SerializeField] private float cooldown;
    [SerializeField] private float weight;
    [SerializeField] private float price;


    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "hpRefill", hpRefill},
            { "staminaRefill", staminaRefill},
            { "manaRefill", manaRefill},
            { "cooldown", cooldown},
            { "weight", weight},
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
