using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable objects/Armor")]
public class ArmorSO : ScriptableObject
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

    // Armor bonuses
    [SerializeField] private float armor;
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;
    [SerializeField] private float weight;


    public Dictionary <string, float> ArmorStats()
    {
        return new()
        {
            {"armor", armor },
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
            {"weight", weight },
        };
    }
}
