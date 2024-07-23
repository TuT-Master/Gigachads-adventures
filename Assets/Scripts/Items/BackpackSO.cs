using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Backpack", menuName = "Scriptable objects/Backpack")]
public class BackpackSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_equipFront;
    public Sprite sprite_equipBack;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

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
}
