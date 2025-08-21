using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable objects/Armor")]
public class ArmorSO : ItemSO
{
    public Sprite sprite_inventory;

    public bool hideHairWhenEquiped;
    public bool hideBeardWhenEquiped;
    public bool hideBodyWhenEquiped;

    [Header("Stats")]
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private float bleedingResistance;
    [SerializeField] private float poisonResistance;
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Full-set bonus")]
    public string armorSetName;
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;

    public Dictionary <string, float> Stats()
    {
        return new()
        {
            {"armor", armor },
            {"magicResistance", magicResistance },
            {"bleedingResistance", bleedingResistance },
            {"poisonResistance", poisonResistance },
            {"weight", weight },
            {"price", price},
        };
    }

    public Dictionary<string, float> FullsetBonus()
    {
        return new()
        {
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public ArmorSO upgradedVersionsOfArmor;

    public override Item ToItem()
    {
        Item item = itemPrefab.GetComponent<Item>();
        item.SetItem(this);
        return item;
    }
}
