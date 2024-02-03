using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponRanged : MonoBehaviour
{
    [HideInInspector] public Dictionary<string, float> stats;

    public string itemName;
    public string description;

    public Slot.SlotType slotType;

    public bool isStackable;
    public bool emitsLight;

    public int amount;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    private TextMeshProUGUI text;


    public WeaponRanged(WeaponRangedSO weaponSO)
    {
        itemName = weaponSO.itemName;
        slotType = Slot.SlotType.WeaponRanged;
        sprite_inventory = weaponSO.sprite_inventory;
        sprite_hand = weaponSO.sprite_hand;
        stats = weaponSO.stats;
        isStackable = weaponSO.isStackable;
        emitsLight = weaponSO.emitsLight;
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        sprite_inventory = null;
        sprite_hand = null;
    }
}
