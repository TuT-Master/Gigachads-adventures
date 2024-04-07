using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class OtherInventory : MonoBehaviour, IInteractable, IDataPersistance
{
    public string otherInventoryGUID = "";
    [ContextMenu("Generate GUID for inventory")]
    public void GenerateGUID() { otherInventoryGUID = System.Guid.NewGuid().ToString(); }

    public bool isLocked = false;

    public bool isOpened;

    public bool isTemp = true;

    public Dictionary<int, Item> inventory;

    public int inventorySize = 8;


    void Start()
    {
        if(inventory == null)
        {
            isLocked = false;
            isOpened = false;
            inventory = new();
            for(int i = 0; i < inventorySize; i++)
                inventory.Add(i, null);
        }
        if(otherInventoryGUID == "")
            GenerateGUID();
    }

    public void SetUpInventory(Dictionary<int, Item> inventory, bool locked)
    {
        this.inventory = inventory;
        inventorySize = inventory.Count;
        isLocked = locked;
        isOpened = false;
    }

    public void Interact()
    {
        isOpened = !isOpened;
        HUDmanager hudManager = FindAnyObjectByType<HUDmanager>();
        hudManager.ToggleOtherInventoryScreen(isOpened);

        PlayerOtherInventoryScreen otherInventoryScreen = FindAnyObjectByType<PlayerOtherInventoryScreen>();
        otherInventoryScreen.UpdateOtherInventory(this);
    }

    public bool CanInteract() { return !isLocked; }

    public void LoadData(GameData data)
    {
        if(isTemp)
            return;


    }

    public void SaveData(ref GameData data)
    {
        if (isTemp)
            return;

        SerializableDictionary<int, string> items = new();
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i] != null)
                items.Add(i, inventory[i].itemName + "-" + inventory[i].amount.ToString());
            else
                items.Add(i, "");
        }

        data.otherInventories.Add(otherInventoryGUID, items);
    }
}
