using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class OtherInventory : MonoBehaviour, IInteractable, IDataPersistance
{
    public bool isLocked = false;

    public bool isOpened;

    public bool isTemp = true;

    public Dictionary<int, Item> inventory;

    public int inventorySize = 8;

    [SerializeField]
    private ItemDatabase itemDatabase;



    void Start()
    {
        if(!isTemp)
        {
            isLocked = false;
            isOpened = false;
            inventory = new();
            for(int i = 0; i < inventorySize; i++)
                inventory.Add(i, null);
        }
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


    Item GetItemForLoading(string item)
    {
        string name = "";
        string amountString = "";
        string currentMagazineString = "";

        int stage = 0;
        foreach (char c in item)
        {
            if (c == '-')
                stage++;
            else if (c == '/')
                stage++;
            else if (stage == 0)
                name += c;
            else if (stage == 1)
                amountString += c;
            else if (stage == 2)
                currentMagazineString += c;
        }

        int.TryParse(amountString, out int amount);
        Item loadedItem = itemDatabase.GetItemByNameAndAmount(name, amount);

        if (int.TryParse(currentMagazineString, out int currentMagazine))
            loadedItem.stats["currentMagazine"] = currentMagazine;

        return loadedItem;
    }

    public void LoadData(GameData data)
    {
        if(isTemp || !data.otherInventories.ContainsKey(transform))
            return;

        inventory = new();
        for (int i = 0; i < data.otherInventories[transform].Count; i++)
        {
            if (data.otherInventories[transform][i] != "")
                inventory.Add(i, GetItemForLoading(data.otherInventories[transform][i]));
            else
                inventory.Add(i, null);
            Debug.Log(inventory[i]);
        }

        isLocked = false;
        isOpened = false;
        inventorySize = inventory.Count;
    }
    public void SaveData(ref GameData data)
    {
        if (isTemp)
            return;

        SerializableDictionary<int, string> items = new();
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i] != null)
            {
                items.Add(i, inventory[i].itemName + "-" + inventory[i].amount.ToString());
                if (inventory[i].stats != null && inventory[i].stats.ContainsKey("currentMagazine"))
                    items[i] += "/" + inventory[i].stats["currentMagazine"].ToString();
            }
            else
                items.Add(i, "");
        }

        if(data.otherInventories.ContainsKey(transform))
            data.otherInventories.Remove(transform);
        data.otherInventories.Add(transform, items);
    }
}
