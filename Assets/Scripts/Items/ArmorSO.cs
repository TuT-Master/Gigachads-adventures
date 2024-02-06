using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable objects/Armor")]
public class ArmorSO : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_equip;

    public Slot.SlotType slotType;

    // Armor bonuses
    public Dictionary<string, float> armorStats;
    [SerializeField] private float armor;


    private void Awake()
    {
        armorStats = new()
        {
            {"armor", armor },
        };
    }
}
