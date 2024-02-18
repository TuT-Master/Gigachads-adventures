using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, IDataPersistance
{
    public bool playerInventoryOpen;

    public GameObject armorSlots;
    public GameObject equipmentSlots;

    [SerializeField]
    private GameObject inventoryCanvas;
    [SerializeField]
    private GameObject backpackInventory;
    public int backpackSize;
    [SerializeField]
    private GameObject beltInventory;
    public int beltSize;
    [SerializeField]
    private GameObject pocketsInventory;
    public int pocketsSize;

    private PlayerStats playerStats;

    [SerializeField]
    private ItemDatabase itemDatabase;

    // Prefabs
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject itemOnDaFloorPrefab;
    [SerializeField]
    private GameObject itemCardPrefab;

    // ItemCard
    private GameObject itemCard;
    private bool isItemCardOpen;


    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        isItemCardOpen = false;
        ToggleInventory(false);
    }

    private void Update()
    {
        UpdatePlayerStats();


        MyInput();
        UpdateBackpack();
        UpdateBelt();
        UpdatePockets();
    }


    // !!-.-.-.- MEGA IMPORTANT -.-.-.-!!
    void UpdatePlayerStats()
    {
        backpackSize = (int)playerStats.playerStats["backpackSize"];
        beltSize = (int)playerStats.playerStats["beltSize"];
        pocketsSize = (int)playerStats.playerStats["pocketSize"];
    }


    public void OpenItemCard(Item item)
    {
        if(isItemCardOpen)
        {
            if(itemCard != null)
                Destroy(itemCard);
            isItemCardOpen = false;
        }
        else
        {
            itemCard = Instantiate(itemCardPrefab, Vector3.zero, Quaternion.identity, inventoryCanvas.transform);
            Vector3 itemPos = item.gameObject.transform.position;
            if(itemPos.x > 1300)
                itemPos = new(itemPos.x - 570, itemPos.y);
            else
                itemPos = new(itemPos.x + 70, itemPos.y);
            if (itemPos.y > 650)
                itemPos = new(itemPos.x, itemPos.y - 570);
            else
                itemPos = new(itemPos.x, itemPos.y - 250);
            itemCard.transform.position = itemPos;


            isItemCardOpen = true;
            // TODO - set up itemCard by item
            // ItemCard GFX
            //itemCard.transform.GetChild(0).GetComponent<Image>().sprite = 
            // Item image
            itemCard.transform.GetChild(1).GetComponent<Image>().sprite = item.sprite_inventory;
            // Item name
            itemCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;
            // Item description
            itemCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;
            // Item's stats
            //itemCard.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = stats
        }
    }
    public void CloseItemCard()
    {
        if (itemCard != null)
            Destroy(itemCard);
        isItemCardOpen = false;
    }


    void UpdateBackpack()
    {
        for (int i = 0; i < backpackInventory.transform.childCount; i++)
        {
            if (i < backpackSize)
            {
                backpackInventory.transform.GetChild(i).gameObject.SetActive(true);
                backpackInventory.transform.GetChild(i).GetComponent<Slot>().isActive = true;
            }
            else
                RemoveSlot(backpackInventory.transform, i);
        }
    }
    void UpdateBelt()
    {
        for (int i = 0; i < beltInventory.transform.childCount; i++)
        {
            if (i < beltSize)
            {
                beltInventory.transform.GetChild(i).gameObject.SetActive(true);
                beltInventory.transform.GetChild(i).GetComponent<Slot>().isActive = true;
            }
            else
                RemoveSlot(beltInventory.transform, i);
        }
    }
    void UpdatePockets()
    {
        for (int i = 0; i < pocketsInventory.transform.childCount; i++)
        {
            if (i < pocketsSize)
            {
                pocketsInventory.transform.GetChild(i).gameObject.SetActive(true);
                pocketsInventory.transform.GetChild(i).GetComponent<Slot>().isActive = true;
            }
            else
                RemoveSlot(pocketsInventory.transform, i);
        }
    }


    void RemoveSlot(Transform parent, int index)
    {
        // Check if there are any items in it
        if (parent.GetChild(index).childCount > 0)
        {
            DropItemOnDaFloor(parent.GetChild(index).GetChild(0).GetComponent<Item>());
            Destroy(parent.GetChild(index).GetChild(0).gameObject);
        }
        parent.GetChild(index).GetComponent<Slot>().isActive = false;
    }
    void DropItemOnDaFloor(Item item)
    {
        GameObject droppedItem = Instantiate(itemOnDaFloorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        droppedItem.GetComponent<ItemOnDaFloor>().SetUpItemOnDaFloor(item);
    }


    public void AddItem(Item item)
    {
        ToggleInventory(true);

        bool done = false;
        int freeSpaceId = -1;
        for(int i = 0; i < backpackInventory.transform.childCount; i++)
        {
            if(backpackInventory.transform.GetChild(i).gameObject.activeInHierarchy && !done)
            {
                if (backpackInventory.transform.GetChild(i).childCount == 0)
                {
                    if (freeSpaceId == -1)
                        freeSpaceId = i;
                }
                else if(
                    backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().slotType == item.slotType &&
                    backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().isStackable)
                {
                    if(backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().amount + item.amount <= backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().stackSize)
                    {
                        backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().amount += item.amount;
                        done = true;
                    }
                    else
                    {
                        item.amount -= (backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().stackSize - backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().amount);
                        backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().amount = backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().stackSize;
                    }
                }
            }
        }
        if(!done && freeSpaceId != -1)
        {
            GameObject newItem = Instantiate(itemPrefab, backpackInventory.transform.GetChild(freeSpaceId));
            newItem.GetComponent<Item>().SetUpByItem(item);
            done = true;
        }
        if (!done)
            Debug.Log("Item could not be placed - no free slot in backpack!");

        Debug.Log(item.itemName + " added!");
        ToggleInventory(false);
    }
    public void AddItemToSlot(Item item, Transform slot)
    {
        ToggleInventory(true);
        GameObject newItem = Instantiate(itemPrefab, slot);
        newItem.GetComponent<Item>().SetUpByItem(item);
        ToggleInventory(false);
    }

    void MyInput()
    {
        if (Input.GetButtonDown("Toggle inventory"))
            ToggleInventory(!playerInventoryOpen);
    }

    public void ToggleInventory(bool open)
    {
        if(open)
        {
            inventoryCanvas.SetActive(true);
            Time.timeScale = 0f;
            playerInventoryOpen = true;
        }
        else
        {
            inventoryCanvas.SetActive(false);
            Time.timeScale = 1f;
            playerInventoryOpen = false;
        }
    }

    IEnumerator LoadingDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (Transform transform in data.playerInventory.Keys)
            if (data.playerInventory[transform] != "")
                AddItemToSlot(GetItemForLoading(data.playerInventory[transform]), transform);
    }
    Item GetItemForLoading(string item)
    {
        string name = "";
        string amountString = "";
        bool readingName = true;
        foreach(char c in item)
        {
            if(readingName)
            {
                if (c == '-')
                    readingName = false;
                else
                    name += c;
            }
            else
            {
                if(c != '-')
                    amountString += c;
            }
        }
        int.TryParse(amountString, out int amount);
        return itemDatabase.GetItemByNameAndAmount(name, amount);
    }
    public void LoadData(GameData data)
    {
        backpackSize = data.backpackSize;
        beltSize = data.beltSize;
        pocketsSize = data.pocketSize;
        StartCoroutine(LoadingDelay(data));
    }
    public void SaveData(ref GameData data)
    {
        data.backpackSize = backpackSize;
        data.beltSize = beltSize;
        data.pocketSize = pocketsSize;

        // Inventory saving
        Dictionary<Transform, string> inventory = new();
        for(int i = 0; i < backpackInventory.transform.childCount; i++)
        {
            inventory.Add(backpackInventory.transform.GetChild(i), "");
            if(backpackInventory.transform.GetChild(i).childCount > 0 && backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
                inventory[backpackInventory.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
        }
        data.playerInventory.Clear();
        foreach(Transform key in inventory.Keys)
            data.playerInventory.Add(key, inventory[key]);
    }
}