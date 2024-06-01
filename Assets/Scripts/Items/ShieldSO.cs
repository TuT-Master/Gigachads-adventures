using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable objects/Shield")]
public class ShieldSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int amount;

    public bool isStackable;
    public bool emitsLight;

    public Sprite sprite_inventory;
    public Sprite sprite_equipFront;
    public Sprite sprite_equipBack;

    public Item.WeaponType weaponType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    [SerializeField] private float defense = 0;
    [SerializeField] private float weight = 0;

    public Dictionary<string, float> Stats()
    {
        return new Dictionary<string, float>()
        {
            {"defense", defense},
            {"weight", weight},
        };
    }
}
