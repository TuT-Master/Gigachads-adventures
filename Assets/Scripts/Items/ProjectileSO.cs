using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable objects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int stackSize;

    public Sprite sprite_inventory;
    public Sprite sprite_projectile;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;


    [Header("Stats")]
    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float projectileSpeed = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float weight = 0;
    [SerializeField] private float price;
    public bool selfHoming;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "damage", damage },
            { "penetration", penetration },
            { "armorIgnore", armorIgnore },
            { "projectileSpeed", projectileSpeed },
            { "splashDamage", splashDamage },
            { "splashRadius", splashRadius},
            { "weight", weight},
            {"price", price},
        };
    }
}
