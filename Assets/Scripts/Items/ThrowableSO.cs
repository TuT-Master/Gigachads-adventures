using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "Scriptable objects/Throwable")]
public class ThrowableSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int amount;

    public bool isStackable;
    public int stackSize;
    public bool emitsLight;

    public bool AoE;

    public Item.WeaponType weaponType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public Sprite sprite_inventory;
    public Sprite sprite_projectile;

    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float attackSpeed = 0;
    [SerializeField] private float critDamage = 0;
    [SerializeField] private float critChance = 0;
    [SerializeField] private float staminaCost = 0;
    [SerializeField] private float manaCost = 0;
    [SerializeField] private float spread = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float rangeX = 1;
    [SerializeField] private float rangeY = 1;
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
            {"critDamage", critDamage},
            {"critChance", critChance},
            {"staminaCost", staminaCost},
            {"manaCost", manaCost},
            {"spread", spread},
            {"splashDamage", splashDamage},
            {"splashRadius", splashRadius},
            {"rangeX", rangeX},
            {"rangeY", rangeY},
            {"weight", weight},
            {"price", price},
        };
    }
}
