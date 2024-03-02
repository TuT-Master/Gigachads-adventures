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
    public GameObject backpackSlot;
    public GameObject beltSlot;

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

    // Hands
    [SerializeField]
    private GameObject LeftHandSlot;
    [SerializeField]
    private GameObject RightHandSlot;
    [SerializeField]
    private SpriteRenderer weaponSpriteRenderer;

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

    private HUDmanager hudManager;


    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        hudManager = GetComponent<HUDmanager>();
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
        UpdateHands();
    }

    void UpdateHands()
    {
        if(LeftHandSlot.transform.childCount > 0 && LeftHandSlot.transform.GetChild(0).TryGetComponent(out Item item))
        {
            weaponSpriteRenderer.sprite = item.sprite_hand;
            GetComponent<PlayerFight>().itemInHand = item;
        }
        else
        {
            weaponSpriteRenderer.sprite = null;
            GetComponent<PlayerFight>().itemInHand = null;
        }
    }

    public bool TwoHandedWeaponInFirstSlot()
    {
        if (LeftHandSlot.transform.childCount > 0)
            if (LeftHandSlot.transform.GetComponentInChildren<Item>().TwoHanded())
                return true;
        return false;
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

    public List<Item> HasItem(string name)
    {
        List<Item> items = new();
        for(int i = 0; i < backpackInventory.transform.childCount; i++)
            if (backpackInventory.transform.GetChild(i).childCount > 0)
                if(backpackInventory.transform.GetChild(i).GetChild(0).GetComponent<Item>().itemName == name)
                    items.Add(backpackInventory.transform.GetChild(i).GetChild(0).GetComponent<Item>());
        return items;
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
            hudManager.ToggleInventoryScreen(!playerInventoryOpen);
    }

    public void ToggleInventory(bool toggle)
    {
        playerInventoryOpen = toggle;
        if (toggle)
        {
            inventoryCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            inventoryCanvas.SetActive(false);
            Time.timeScale = 1f;
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
        string currentMagazineString = "";

        int stage = 0;
        foreach(char c in item)
        {
            if (c == '-')
                stage++;
            else if (c == '/')
                stage++;
            else if(stage == 0)
                name += c;
            else if (stage == 1)
                amountString += c;
            else if (stage == 2)
                currentMagazineString += c;
        }

        int.TryParse(amountString, out int amount);
        Item loadedItem = itemDatabase.GetItemByNameAndAmount(name, amount);

        if(int.TryParse(currentMagazineString, out int currentMagazine))
            loadedItem.stats["currentMagazine"] = currentMagazine;

        return loadedItem;
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
        // Backpack inventory
        for(int i = 0; i < backpackInventory.transform.childCount; i++)
        {
            inventory.Add(backpackInventory.transform.GetChild(i), "");
            if(backpackInventory.transform.GetChild(i).childCount > 0 && backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[backpackInventory.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[backpackInventory.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Belt inventory
        for (int i = 0; i < beltInventory.transform.childCount; i++)
        {
            inventory.Add(beltInventory.transform.GetChild(i), "");
            if (beltInventory.transform.GetChild(i).childCount > 0 && beltInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[beltInventory.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[beltInventory.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Pocket inventory
        for (int i = 0; i < pocketsInventory.transform.childCount; i++)
        {
            inventory.Add(pocketsInventory.transform.GetChild(i), "");
            if (pocketsInventory.transform.GetChild(i).childCount > 0 && pocketsInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[pocketsInventory.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[pocketsInventory.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Hand slots
        inventory.Add(LeftHandSlot.transform, "");
        inventory.Add(RightHandSlot.transform, "");
        if (LeftHandSlot.transform.childCount > 0 && LeftHandSlot.transform.GetChild(0).TryGetComponent(out Item _item))
        {
            inventory[LeftHandSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[LeftHandSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        if (RightHandSlot.transform.childCount > 0 && RightHandSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[RightHandSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[RightHandSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        // Armor slots
        for (int i = 0; i < armorSlots.transform.childCount; i++)
        {
            inventory.Add(armorSlots.transform.GetChild(i), "");
            if (armorSlots.transform.GetChild(i).childCount > 0 && armorSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[armorSlots.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[armorSlots.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Equipment slots
        for (int i = 0; i < equipmentSlots.transform.childCount; i++)
        {
            inventory.Add(equipmentSlots.transform.GetChild(i), "");
            if (equipmentSlots.transform.GetChild(i).childCount > 0 && equipmentSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[equipmentSlots.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[equipmentSlots.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Backpack slot
        inventory.Add(backpackSlot.transform, "");
        if (backpackSlot.transform.childCount > 0 && backpackSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[backpackSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[backpackSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        // Belt slot
        inventory.Add(beltSlot.transform, "");
        if (beltSlot.transform.childCount > 0 && beltSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[beltSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[beltSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }


        data.playerInventory.Clear();
        foreach(Transform key in inventory.Keys)
            data.playerInventory.Add(key, inventory[key]);
    }
}