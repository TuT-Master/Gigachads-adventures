using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMelee", menuName = "Scriptable objects/WeaponMelee")]
public class WeaponMeleeSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int amount;

    public bool isStackable;
    public bool emitsLight;

    public bool twoHanded;
    public bool AoE;

    public Item.WeaponType weaponType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float attackSpeed = 0;
    [SerializeField] private float staminaCost = 0;
    [SerializeField] private float manaCost = 0;
    [SerializeField] private float rangeX = 1;
    [SerializeField] private float rangeY = 1;
    [SerializeField] private float defense = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;

    public Dictionary<string, float> Stats()
    {
        return new Dictionary<string, float>()
        {
            {"damage", damage},
            {"penetration", penetration},
            {"armorIgnore", armorIgnore},
            {"attackSpeed", attackSpeed},
            {"staminaCost", staminaCost},
            {"manaCost", manaCost},
            {"rangeX", rangeX},
            {"rangeY", rangeY},
            {"defense", defense},
            {"weight", weight},
            {"price", price},
        };
    }

    [Header("Upgrade")]
    public WeaponMeleeSO upgradedVersionsOfWeapon;
}
