using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "Scriptable objects/Throwable")]
public class ThrowableSO : ItemSO
{
    public bool emitsLight;

    public bool AoE;

    public Item.WeaponType weaponType;

    public Sprite sprite_inventory;
    public Sprite sprite_handMale_Front;
    public Sprite sprite_handMale_Back;
    public Sprite sprite_handFemale_Front;
    public Sprite sprite_handFemale_Back;

    public Sprite sprite_projectile;

    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float poisonDamage = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float burningChance = 0;
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
            {"poisonDamage", poisonDamage},
            {"bleedingDamage", bleedingDamage},
            {"burningChance", burningChance},
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

    public override Item ToItem()
    {
        return new(this);
    }
}
