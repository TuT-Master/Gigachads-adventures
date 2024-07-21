using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable objects/Material")]
public class MaterialSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int stackSize;

    public Sprite sprite_inventory;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public Item.MagicCrystalType crystalType;

    [Header("Stats")]
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "weight", weight},
            {"price", price},
        };
    }
}
