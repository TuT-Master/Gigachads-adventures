using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable objects/Shield")]
public class ShieldSO : ItemSO
{
    public bool emitsLight;

    public Sprite sprite_inventory;
    public Sprite sprite_equipMale_Front;
    public Sprite sprite_equipMale_Back;
    public Sprite sprite_equipFemale_Front;
    public Sprite sprite_equipFemale_Back;

    public Item.WeaponType weaponType;

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

    public override Item ToItem()
    {
        return new(this);
    }
}
