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
    public Sprite sprite_hand;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public bool isStackable;
    public int stackSize;

    [SerializeField] private float hpRefill = 0;
    [SerializeField] private float staminaRefill = 0;
    [SerializeField] private float manaRefill = 0;
    [SerializeField] private float cooldown = 0;
    [SerializeField] private float weight = 0;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "hpRefill", hpRefill},
            { "staminaRefill", staminaRefill},
            { "manaRefill", manaRefill},
            { "cooldown", cooldown},
            { "weight", weight},
        };
    }
}
