using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable objects/Consumable")]
public class ConsumableSO : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    public bool isStackable;
    public int stackSize;

    [SerializeField] private float weight = 0;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "weight", weight},
        };
    }
}
