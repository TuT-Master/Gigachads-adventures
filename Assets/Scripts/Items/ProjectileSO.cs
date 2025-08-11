using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable objects/Projectile")]
public class ProjectileSO : ItemSO
{
    public Sprite sprite_inventory;
    public Sprite sprite_projectile;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ItemSO> recipeMaterials;
    public List<int> recipeMaterialsAmount;


    [Header("Stats")]
    public bool selfHoming;
    [SerializeField] private float damage = 0;
    [SerializeField] private float poisonDamage = 0;
    [SerializeField] private float bleedingDamage = 0;
    [SerializeField] private float burningChance = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float piercing = 0;
    [SerializeField] private float projectileSpeed = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "damage", damage },
            { "poisonDamage", poisonDamage},
            { "bleedingDamage", bleedingDamage},
            {"burningChance", burningChance},
            { "penetration", penetration },
            { "armorIgnore", armorIgnore },
            { "piercing", piercing },
            { "projectileSpeed", projectileSpeed },
            { "splashDamage", splashDamage },
            { "splashRadius", splashRadius},
            { "weight", weight},
            {"price", price},
        };
    }

    public override Item ToItem()
    {
        return new(this);
    }
}
