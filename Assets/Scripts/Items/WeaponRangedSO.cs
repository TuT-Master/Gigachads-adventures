using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRanged", menuName = "Scriptable objects/WeaponRanged")]
public class WeaponRangedSO : ScriptableObject
{
    public string itemName;
    public string description;

    public int amount;

    public bool isStackable;
    public int stackSize;
    public bool emitsLight;
    public bool fullAuto;

    public bool twoHanded;
    public bool AoE;

    public Item.WeaponType weaponType;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float attackSpeed = 0;
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
        };
    }

    public List<ProjectileSO> ammo;
}
