using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable objects/Consumable")]
public class ConsumableSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_handMale_Front;
    public Sprite sprite_handMale_Back;
    public Sprite sprite_handFemale_Front;
    public Sprite sprite_handFemale_Back;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public bool isStackable;
    public int stackSize;

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
}
