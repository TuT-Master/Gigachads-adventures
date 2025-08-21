using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Scriptable objects/Equipable")]
public class AccessorySO : ItemSO
{
    public Sprite sprite_inventory;
    public Sprite sprite_equipMale_Front;
    public Sprite sprite_equipMale_Back;
    public Sprite sprite_equipFemale_Front;
    public Sprite sprite_equipFemale_Back;

    [Header("Stats")]
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Bonuses")]
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;


    public Dictionary<string, float> Stats()
    {
        return new()
        {
            {"weight", weight },
            {"price", price},
        };
    }
    public Dictionary<string, float> Bonus()
    {
        return new()
        {
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
        };
    }

    public override Item ToItem()
    {
        return new(this);
    }
}
