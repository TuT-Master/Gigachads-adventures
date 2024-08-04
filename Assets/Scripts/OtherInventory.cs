using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherInventory : MonoBehaviour, IInteractable, IDataPersistance
{
    public string otherInventoryName;

    public int otherInventoryId;

    public bool isLocked = false;

    public bool isOpened;

    public bool saveInv;

    public Dictionary<int, string> inventory;

    public int inventorySize = 8;

    [HideInInspector]
    public ItemDatabase itemDatabase;


    void Start() { otherInventoryName = "Chest"; }
    public void SetUpInventory(Dictionary<int, string> inventory, bool locked)
    {
        this.inventory = inventory;
        inventorySize = inventory.Count;
        isLocked = locked;
        isOpened = false;
    }

    public void Interact()
    {
        isOpened = !isOpened;

        CreateInventoryIfNull();

        if (isOpened)
            FindAnyObjectByType<PlayerOtherInventoryScreen>().UpdateOtherInventory(gameObject);

        FindAnyObjectByType<HUDmanager>().ToggleOtherInventoryScreen(isOpened);
    }

    private void CreateInventoryIfNull()
    {
        if(inventory == null)
        {
            inventory = new();
            for (int i = 0; i < inventorySize; i++)
                inventory.Add(i, null);
        }
    }

    public bool CanInteract() { return !isLocked; }
    public Transform GetTransform() { return transform; }

    private IEnumerator LoadingDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        inventory = new();
        // Loading data from file
        for (int i = 0; i < data.otherInventories[otherInventoryId].Count; i++)
        {
            if (data.otherInventories[otherInventoryId][i] != "")
                inventory.Add(i, data.otherInventories[otherInventoryId][i]);
            else
                inventory.Add(i, null);
        }
        otherInventoryName = data.otherInventoriesNames[otherInventoryId];
        isLocked = false;
        isOpened = false;
        inventorySize = inventory.Count;
    }

    public void LoadData(GameData data)
    {
        if(!saveInv || !data.otherInventories.ContainsKey(otherInventoryId))
            return;

        CreateInventoryIfNull();

        // Loading data
        StartCoroutine(LoadingDelay(data));
    }
    public void SaveData(ref GameData data)
    {
        if (!saveInv)
            return;

        CreateInventoryIfNull();

        SerializableDictionary<int, string> items = new();
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i] != null)
                items.Add(i, inventory[i]);
            else
                items.Add(i, "");
        }

        // Name
        data.otherInventoriesNames ??= new();
        if(data.otherInventoriesNames.ContainsKey(otherInventoryId))
            data.otherInventoriesNames.Remove(otherInventoryId);
        data.otherInventoriesNames.Add(otherInventoryId, otherInventoryName);
        // Inventory
        if(data.otherInventories.ContainsKey(otherInventoryId))
            data.otherInventories.Remove(otherInventoryId);
        data.otherInventories.Add(otherInventoryId, items);
    }
}
