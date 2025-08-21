using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecipeItemAmount
{
    public ItemSO item;
    public int amount;
}

public abstract class ItemSO : ScriptableObject
{
    [Header("Basic info")]
    public string itemName;
    [TextArea]
    public string description;
    public int amount;
    public bool isStackable;
    public int stackSize;

    [Header("Slot type")]
    public Slot.SlotType slotType;

    [Header("Crafting")]
    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<RecipeItemAmount> recipeItems;
    public Dictionary<ItemSO, int> recipe
    {
        get
        {
            Dictionary<ItemSO, int> dict = new();
            foreach (var recipeItem in recipeItems)
                if (recipeItem.item != null)
                    dict[recipeItem.item] = recipeItem.amount;
            return dict;
        }
    }
    public abstract Item ToItem();
}