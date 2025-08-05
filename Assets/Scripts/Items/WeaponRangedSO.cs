using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRanged", menuName = "Scriptable objects/WeaponRanged")]
public class WeaponRangedSO : ItemSO
{
    public bool emitsLight;
    public bool fullAuto;

    public bool twoHanded;
    public bool AoE;

    public Item.WeaponType weaponType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public Sprite sprite_inventory;
    public Sprite sprite_handMale_Front;
    public Sprite sprite_handMale_Back;
    public Sprite sprite_handFemale_Front;
    public Sprite sprite_handFemale_Back;

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
    [SerializeField] private float currentMagazine = 0;
    [SerializeField] private float magazineSize = 0;
    [SerializeField] private float reloadTime = 0;
    [SerializeField] private float spread = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
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
            {"currentMagazine", currentMagazine},
            {"magazineSize", magazineSize},
            {"reloadTime", reloadTime},
            {"spread", spread},
            {"splashDamage", splashDamage},
            {"splashRadius", splashRadius},
            {"rangeX", rangeX},
            {"rangeY", rangeY},
            {"defense", defense},
            {"weight", weight},
            {"price", price},
        };
    }

    [Header("Upgrade")]
    public bool isUpgrade;
    public WeaponRangedSO upgradedVersionsOfWeapon;

    [Header("Ammo types")]
    public List<ProjectileSO> ammo;

    public override Item ToItem()
    {
        return new(this);
    }
}
