using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [HideInInspector] public Dictionary<string, float> stats;

    public string itemName;
    public string description;

    public Slot.SlotType slotType;

    public bool isStackable;

    public int amount;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    private TextMeshProUGUI text;


    public Consumable(ConsumableSO consumableSO)
    {
        itemName = consumableSO.itemName;
        description = consumableSO.description;
        slotType = Slot.SlotType.Consumable;
        sprite_inventory = consumableSO.sprite_inventory;
        sprite_hand = consumableSO.sprite_hand;
        isStackable = consumableSO.isStackable;
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        sprite_inventory = null;
        sprite_hand = null;
        SetItemUp();
    }

    void SetItemUp()
    {

    }
}
