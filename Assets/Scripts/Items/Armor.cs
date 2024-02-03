using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [HideInInspector] public Dictionary<string, float> stats;

    public string itemName;
    public string description;

    public Slot.SlotType slotType;

    public int amount;

    public Sprite sprite_inventory;
    public Sprite sprite_equip;

    private TextMeshProUGUI text;


    public Armor(ArmorSO armorSO)
    {
        itemName = armorSO.itemName;
        description = armorSO.description;
        slotType = armorSO.slotType;
        sprite_inventory = armorSO.sprite_inventory;
        sprite_equip = armorSO.sprite_equip;
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        sprite_inventory = null;
        sprite_equip = null;
        SetItemUp();
    }

    void SetItemUp()
    {

    }
}
