using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Scriptable objects/Equipable")]
public class EquipableSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_equipFront;
    public Sprite sprite_equipBack;

    public Slot.SlotType slotType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;


    [Header("Stats")]
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Bonuses")]
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;


    public Dictionary<string, float> Stats()
    {
        return new()
        {
            {"weight", weight },
            {"price", price},
        };
    }
    public Dictionary<string, float> Bonus()
    {
        return new()
        {
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
        };
    }
}
