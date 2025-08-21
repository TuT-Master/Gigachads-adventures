using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class PlayerInventory : MonoBehaviour, IDataPersistance
{
    public bool playerInventoryOpen;

    public GameObject armorSlots;
    public GameObject equipmentSlots;
    public GameObject backpackSlot;
    public GameObject beltSlot;

    [SerializeField]
    private GameObject inventoryCanvas;
    public GameObject backpackInventory;
    public int backpackSize;
    [SerializeField]
    private GameObject beltInventory;
    public int beltSize;
    [SerializeField]
    private GameObject pocketsInventory;
    public int pocketsSize;

    private PlayerStats playerStats;

    public ItemDatabase itemDatabase;

    // Hands
    [SerializeField]
    private GameObject LeftHandSlot;
    [SerializeField]
    private GameObject RightHandSlot;

    // Prefabs
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject itemOnDaFloorPrefab;

    // Ammo slots
    [Header("Ammo slots")]
    [SerializeField] private GameObject ammoSlots;



    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        ToggleInventory(false);
    }

    private void Update()
    {
        UpdatePlayerStats();


        UpdateBackpack();
        UpdateBelt();
        UpdatePockets();
        UpdateHands();

        if(Input.GetKeyDown(KeyCode.I))
            ToggleInventory(!playerInventoryOpen);
    }


    void UpdateHands()
    {
        // Left hand
        if(LeftHandSlot.transform.childCount > 0 && LeftHandSlot.transform.GetChild(0).TryGetComponent(out Item item))
            GetComponent<PlayerFight>().itemInHand = item;

        // Right hand
        if (RightHandSlot.transform.childCount > 0 && RightHandSlot.transform.GetChild(0).TryGetComponent(out item))
            GetComponent<PlayerFight>().secondaryItemInHand = item;
    }


    public bool TwoHandedWeaponInFirstSlot()
    {
        if (LeftHandSlot.transform.childCount > 0)
            if (LeftHandSlot.transform.GetComponentInChildren<Item>().twoHanded)
                return true;
        return false;
    }


    // !!-.-.-.- MEGA IMPORTANT -.-.-.-!!
    void UpdatePlayerStats()
    {
        if (playerStats.playerStats == null)
            return;
        backpackSize = (int)playerStats.playerStats["backpackSize"];
        beltSize = (int)playerStats.playerStats["beltSize"];
        pocketsSize = (int)playerStats.playerStats["pocketSize"];
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
    public List<Item> HasAmmo(string name)
    {
        List<Item> items = new();
        for (int i = 0; i < ammoSlots.transform.childCount; i++)
            if (ammoSlots.transform.GetChild(i).childCount > 0)
                if (ammoSlots.transform.GetChild(i).GetChild(0).GetComponent<Item>().itemName == name)
                    items.Add(ammoSlots.transform.GetChild(i).GetChild(0).GetComponent<Item>());
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


    private void RemoveSlot(Transform parent, int index)
    {
        // Check if there are any items in it
        if (parent.GetChild(index).childCount > 0)
        {
            DropItemOnDaFloor(parent.GetChild(index).GetChild(0).GetComponent<Item>(), transform.position, null);
            Destroy(parent.GetChild(index).GetChild(0).gameObject);
        }
        parent.GetChild(index).GetComponent<Slot>().isActive = false;
    }
    public void DropItemOnDaFloor(Item item, Vector3 pos, Transform parent)
    {
        GameObject droppedItem = null;
        if (parent == null)
            droppedItem = Instantiate(itemOnDaFloorPrefab, pos, Quaternion.identity);
        else
            droppedItem = Instantiate(itemOnDaFloorPrefab, pos, Quaternion.identity, parent);
        droppedItem.GetComponent<ItemOnDaFloor>().SetUpItemOnDaFloor(item);

        float angle = GetComponent<PlayerMovement>().angleRaw + new System.Random().Next(-15, 15);
        Vector3 force = GetComponent<PlayerFight>().VectorFromAngle(angle).normalized;
        droppedItem.GetComponent<Rigidbody>().AddForce(new Vector3(force.z, 0, force.x) * 2, ForceMode.VelocityChange);
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
                    backpackInventory.transform.GetChild(i).GetComponentInChildren<Item>().itemName == item.itemName &&
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
        {
            Dungeon dungeon = FindAnyObjectByType<Dungeon>();
            if (dungeon != null && dungeon.currentRoom != null)
                DropItemOnDaFloor(item, transform.position, dungeon.currentRoom.transform);
            else
                DropItemOnDaFloor(item, transform.position, null);
        }
        ToggleInventory(false);
    }
    public void AddItemToSlot(Item item, Transform slot)
    {
        ToggleInventory(true);
        GameObject newItem = Instantiate(itemPrefab, slot);
        newItem.GetComponent<Item>().SetUpByItem(item);
        ToggleInventory(false);
    }


    public void ToggleInventory(bool toggle)
    {
        playerInventoryOpen = toggle;
        if (toggle)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        inventoryCanvas.SetActive(toggle);
    }


    public Item GetItemForLoading(string item)
    {
        string name = "";
        string amountString = "";
        string currentMagazineString = "";
        List<int> crystals = new();

        int stage = 0;
        foreach (char c in item)
        {
            if (c == '-')
                stage = 1;
            else if (c == '/')
                stage = 2;
            else if (c == '|')
                stage = 3;
            else
            {
                if (stage == 0)
                    name += c;
                else if (stage == 1)
                    amountString += c;
                else if (stage == 2)
                    currentMagazineString += c;
                else if (stage == 3 && int.TryParse(c.ToString(), out int id))
                    crystals.Add(id);
            }
        }

        // Amount
        int.TryParse(amountString, out int amount);
        Item loadedItem = itemDatabase.GetItemByNameAndAmount(name, amount);

        // Current magazine
        if(int.TryParse(currentMagazineString, out int currentMagazine))
            loadedItem.stats["currentMagazine"] = currentMagazine;

        // Magic crystals
        loadedItem.magicCrystals ??= new();
        for (int i = 0; i < crystals.Count; i++)
            loadedItem.magicCrystals.Add(i, loadedItem.GetMagicCrystalTypeByInt(crystals[i]));

        return loadedItem;
    }
    public void LoadData(GameData data)
    {
        // Clear any old items if necessary
        ClearAllInventories();

        Dictionary<int, string> inventory = data.playerInventory;

        // Containers
        LoadContainer(backpackInventory.transform, inventory);
        LoadContainer(ammoSlots.transform, inventory);
        LoadContainer(beltInventory.transform, inventory);
        LoadContainer(pocketsInventory.transform, inventory);
        LoadContainer(armorSlots.transform, inventory);
        LoadContainer(equipmentSlots.transform, inventory);

        // Single slots
        LoadSingleSlot(LeftHandSlot, inventory);
        LoadSingleSlot(RightHandSlot, inventory);
        LoadSingleSlot(backpackSlot, inventory);
        LoadSingleSlot(beltSlot, inventory);
    }
    private Item DeserializeItem(string data, Transform parent)
    {
        if (string.IsNullOrEmpty(data)) return null;

        // Split off crystals if present
        string[] parts = data.Split('|');
        string core = parts[0]; // everything before crystals
        string crystals = parts.Length > 1 ? parts[1] : null;

        // Handle magazine
        string[] magSplit = core.Split('/');
        string basePart = magSplit[0]; // itemName-amount
        int currentMag = (magSplit.Length > 1 && int.TryParse(magSplit[1], out int magVal)) ? magVal : 0;

        // Name and amount
        string[] nameSplit = basePart.Split('-');
        string itemName = nameSplit[0];
        int amount = (nameSplit.Length > 1 && int.TryParse(nameSplit[1], out int amtVal)) ? amtVal : 1;

        // Instantiate prefab under parent
        GameObject itemObj = Instantiate(itemPrefab, parent);
        Item item = itemObj.GetComponent<Item>();
        item.SetUpByItem(itemDatabase.GetItemByNameAndAmount(itemName, amount));

        // Set values
        item.itemName = itemName;
        item.amount = amount;

        item.stats ??= new();

        if (currentMag > 0)
            item.stats["currentMagazine"] = currentMag;

        if (!string.IsNullOrEmpty(crystals))
        {
            item.magicCrystals = new();
            for(int i = 0; i < crystals.Length; i++)
            {
                int crystalInt = (int)char.GetNumericValue(crystals[0]);
                item.magicCrystals.Add(i, (MagicCrystalType)crystalInt);
            }
        }

        return item;
    }
    private void LoadContainer(Transform container, Dictionary<int, string> inventory)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Slot slot = container.GetChild(i).GetComponent<Slot>();
            if (inventory.TryGetValue(slot.id, out string data))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DeserializeItem(data, container.GetChild(i));
                }
            }
        }
    }
    private void LoadSingleSlot(GameObject slotObj, Dictionary<int, string> inventory)
    {
        Slot slot = slotObj.GetComponent<Slot>();
        if (inventory.TryGetValue(slot.id, out string data))
        {
            if (!string.IsNullOrEmpty(data))
            {
                DeserializeItem(data, slotObj.transform);
            }
        }
    }
    private void ClearAllInventories()
    {
        foreach (Transform child in backpackInventory.transform)
            foreach (Transform item in child)
                Destroy(item.gameObject);

        foreach (Transform child in ammoSlots.transform)
            foreach (Transform item in child)
                Destroy(item.gameObject);

        // ...repeat for belt, pockets, armor, equipment, hands, etc.
    }

    public void SaveData(ref GameData data)
    {
        data.backpackSize = backpackSize;
        data.beltSize = beltSize;
        data.pocketSize = pocketsSize;

        Dictionary<int, string> inventory = new();

        // Containers
        SaveContainer(backpackInventory.transform, inventory);
        SaveContainer(ammoSlots.transform, inventory);
        SaveContainer(beltInventory.transform, inventory);
        SaveContainer(pocketsInventory.transform, inventory);
        SaveContainer(armorSlots.transform, inventory);
        SaveContainer(equipmentSlots.transform, inventory);

        // Single slots
        SaveSingleSlot(LeftHandSlot, inventory);
        SaveSingleSlot(RightHandSlot, inventory);
        SaveSingleSlot(backpackSlot, inventory);
        SaveSingleSlot(beltSlot, inventory);

        data.playerInventory = new();
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory.TryGetValue(i, out string itemString) && itemString != "")
                data.playerInventory[i] = itemString;
            else
                data.playerInventory[i] = "";
        }
    }
    private string SerializeItem(Item item)
    {
        if (item == null) return "";

        string result = $"{item.itemName}-{item.amount}";

        if (item.stats != null && item.stats.TryGetValue("currentMagazine", out float mag) && mag > 0)
            result += $"/{mag}";

        if (item.stats != null && item.magicCrystals != null && item.magicCrystals.Count > 0)
        {
            result += "|";
            for (int j = 0; j < item.magicCrystals.Count; j++)
                result += (int)item.magicCrystals[j];
        }

        return result;
    }
    private void SaveContainer(Transform container, Dictionary<int, string> inventory)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Slot slot = container.GetChild(i).GetComponent<Slot>();
            inventory[slot.id] = "";

            if (container.GetChild(i).childCount > 0 &&
                container.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[slot.id] = SerializeItem(item);
            }
        }
    }
    private void SaveSingleSlot(GameObject slotObj, Dictionary<int, string> inventory)
    {
        Slot slot = slotObj.GetComponent<Slot>();
        inventory[slot.id] = "";

        if (slotObj.transform.childCount > 0 &&
            slotObj.transform.GetChild(0).TryGetComponent(out Item item))
        {
            inventory[slot.id] = SerializeItem(item);
        }
    }

}