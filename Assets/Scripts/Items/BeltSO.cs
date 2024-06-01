using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Belt", menuName = "Scriptable objects/Belt")]
public class BeltSO : ScriptableObject
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

    public int inventoryCapacity;

    public float weight;

    public Dictionary<string, float> BeltStats()
    {
        return new()
        {
            {"backpackSize", inventoryCapacity },
            {"weight", weight},
        };
    }
}
