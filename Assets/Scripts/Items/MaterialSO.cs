using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable objects/Material")]
public class MaterialSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public int stackSize;

    public Sprite sprite_inventory;

    [SerializeField] private float weight = 0;
    public Dictionary<string, float> Stats()
    {
        return new()
        {
            { "weight", weight},
        };
    }
}
