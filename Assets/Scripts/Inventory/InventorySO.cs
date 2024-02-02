using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable objects/Inventory")]
public class InventorySO : ScriptableObject
{
    public int inventorySize = 0;

    public Dictionary<int, Item> inventory;
}
