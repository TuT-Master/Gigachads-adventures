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
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;
    [SerializeField] private float weight;
    [SerializeField] private float price;


    public Dictionary<string, float> ArmorStats()
    {
        return new()
        {
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
            {"weight", weight },
            {"price", price},
        };
    }
}
