using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Backpack", menuName = "Scriptable objects/Backpack")]
public class BackpackSO : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_equipFront;
    public Sprite sprite_equipBack;

    public int inventoryCapacity;

    public int weight;
}
