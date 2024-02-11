using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMelee", menuName = "Scriptable objects/WeaponMelee")]
public class WeaponMeleeSO : ScriptableObject
{
    public string itemName;
    public string description;

    public int amount;

    public bool isStackable;
    public bool emitsLight;

    public Sprite sprite_inventory;
    public Sprite sprite_hand;

    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float attackSpeed = 0;
    [SerializeField] private float staminaCost = 0;
    [SerializeField] private float manaCost = 0;
    [SerializeField] private float rangeX = 1;
    [SerializeField] private float rangeY = 1;
    [SerializeField] private float AoE = 0;
    [SerializeField] private float twoHanded = 0;
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
            {"rangeX", rangeX},
            {"rangeY", rangeY},
            {"AoE", AoE},
            {"twoHanded", twoHanded},
            {"weight", weight},
        };
    }
}
