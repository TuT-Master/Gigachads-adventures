using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;
    public int amount;
    public bool isStackable;
    public int stackSize;

    public abstract Item ToItem();
}
