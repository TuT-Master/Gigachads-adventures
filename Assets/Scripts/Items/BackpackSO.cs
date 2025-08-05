using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Backpack", menuName = "Scriptable objects/Backpack")]
public class BackpackSO : ItemSO
{
    public Sprite sprite_inventory;
    public Sprite sprite_equipMale_Front;
    public Sprite sprite_equipMale_Back;
    public Sprite sprite_equipFemale_Front;
    public Sprite sprite_equipFemale_Back;

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

    public override Item ToItem()
    {
        return new(this);
    }
}
