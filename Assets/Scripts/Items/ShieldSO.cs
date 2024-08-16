using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable objects/Shield")]
public class ShieldSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int amount;

    public bool isStackable;
    public bool emitsLight;

    public Sprite sprite_inventory;
    public Sprite sprite_equipMale_Front;
    public Sprite sprite_equipMale_Back;
    public Sprite sprite_equipFemale_Front;
    public Sprite sprite_equipFemale_Back;

    public Item.WeaponType weaponType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    [Header("Stats")]
    [SerializeField] private float defense = 0;
    [SerializeField] private float staminaPerBlock = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;

    public Dictionary<string, float> Stats()
    {
        return new Dictionary<string, float>()
        {
            {"defense", defense},
            {"staminaPerBlock", staminaPerBlock},
            {"weight", weight},
            {"price", price},
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public ShieldSO upgradedVersionsOfShield;
}
