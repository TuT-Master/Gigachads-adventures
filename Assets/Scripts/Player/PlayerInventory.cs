using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    void RemoveSlot(Transform parent, int index)
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
            if(FindAnyObjectByType<VirtualSceneManager>().DungeonLoaded())
                DropItemOnDaFloor(item, transform.position, FindAnyObjectByType<Dungeon>().currentRoom.transform);
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


    IEnumerator LoadingDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (int id in data.playerInventory.Keys)
            if (data.playerInventory[id] != "" && data.playerInventory[id] != null && FindSlotById(id, out Transform trans))
                AddItemToSlot(GetItemForLoading(data.playerInventory[id]), trans);
    }
    private bool FindSlotById(int id, out Transform slotTransform)
    {
        foreach(Slot slot in FindObjectsOfType<MonoBehaviour>(true).OfType<Slot>())
            if(slot.id == id)
            {
                slotTransform = slot.transform;
                return true;
            }
        slotTransform = null;
        return false;
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
        Dictionary<int, string> inventory = new();
        // Backpack inventory
        for(int i = 0; i < backpackInventory.transform.childCount; i++)
        {
            inventory.Add(backpackInventory.transform.GetChild(i).GetComponent<Slot>().id, "");
            if(backpackInventory.transform.GetChild(i).childCount > 0 && backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
                if (item.stats != null && item.magicCrystals != null)
                {
                    inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] += "|";
                    for (int j = 0; j < item.magicCrystals.Count; j++)
                        inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] += (int)item.magicCrystals[j];
                }
            }
        }
        // Ammo slots inventory
        for (int i = 0; i < ammoSlots.transform.childCount; i++)
        {
            inventory.Add(ammoSlots.transform.GetChild(i).GetComponent<Slot>().id, "");
            if (ammoSlots.transform.GetChild(i).childCount > 0 && ammoSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[ammoSlots.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[ammoSlots.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
                if (item.stats != null && item.magicCrystals != null)
                {
                    inventory[ammoSlots.transform.GetChild(i).GetComponent<Slot>().id] += "|";
                    for (int j = 0; j < item.magicCrystals.Count; j++)
                        inventory[ammoSlots.transform.GetChild(i).GetComponent<Slot>().id] += (int)item.magicCrystals[j];
                }
            }
        }
        // Belt inventory
        for (int i = 0; i < beltInventory.transform.childCount; i++)
        {
            inventory.Add(beltInventory.transform.GetChild(i).GetComponent<Slot>().id, "");
            if (beltInventory.transform.GetChild(i).childCount > 0 && beltInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[beltInventory.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[beltInventory.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
                if (item.stats != null && item.magicCrystals != null)
                {
                    inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] += "|";
                    for (int j = 0; j < item.magicCrystals.Count; j++)
                        inventory[backpackInventory.transform.GetChild(i).GetComponent<Slot>().id] += (int)item.magicCrystals[j];
                }
            }
        }
        // Pocket inventory
        for (int i = 0; i < pocketsInventory.transform.childCount; i++)
        {
            inventory.Add(pocketsInventory.transform.GetChild(i).GetComponent<Slot>().id, "");
            if (pocketsInventory.transform.GetChild(i).childCount > 0 && pocketsInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[pocketsInventory.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[pocketsInventory.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Hand slots
        inventory.Add(LeftHandSlot.GetComponent<Slot>().id, "");
        inventory.Add(RightHandSlot.GetComponent<Slot>().id, "");
        if (LeftHandSlot.transform.childCount > 0 && LeftHandSlot.transform.GetChild(0).TryGetComponent(out Item _item))
        {
            inventory[LeftHandSlot.GetComponent<Slot>().id] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[LeftHandSlot.GetComponent<Slot>().id] += "/" + _item.stats["currentMagazine"].ToString();
            if (_item.stats != null && _item.magicCrystals != null)
            {
                inventory[LeftHandSlot.GetComponent<Slot>().id] += "|";
                for (int j = 0; j < _item.magicCrystals.Count; j++)
                    inventory[LeftHandSlot.GetComponent<Slot>().id] += (int)_item.magicCrystals[j];
            }
        }
        if (RightHandSlot.transform.childCount > 0 && RightHandSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[RightHandSlot.GetComponent<Slot>().id] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[RightHandSlot.GetComponent<Slot>().id] += "/" + _item.stats["currentMagazine"].ToString();
            if (_item.stats != null && _item.magicCrystals != null)
            {
                inventory[RightHandSlot.GetComponent<Slot>().id] += "|";
                for (int j = 0; j < _item.magicCrystals.Count; j++)
                    inventory[RightHandSlot.GetComponent<Slot>().id] += (int)_item.magicCrystals[j];
            }
        }
        // Armor slots
        for (int i = 0; i < armorSlots.transform.childCount; i++)
        {
            inventory.Add(armorSlots.transform.GetChild(i).GetComponent<Slot>().id, "");
            if (armorSlots.transform.GetChild(i).childCount > 0 && armorSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[armorSlots.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[armorSlots.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Equipment slots
        for (int i = 0; i < equipmentSlots.transform.childCount; i++)
        {
            inventory.Add(equipmentSlots.transform.GetChild(i).GetComponent<Slot>().id, "");
            if (equipmentSlots.transform.GetChild(i).childCount > 0 && equipmentSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[equipmentSlots.transform.GetChild(i).GetComponent<Slot>().id] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[equipmentSlots.transform.GetChild(i).GetComponent<Slot>().id] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Backpack slot
        inventory.Add(backpackSlot.GetComponent<Slot>().id, "");
        if (backpackSlot.transform.childCount > 0 && backpackSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[backpackSlot.GetComponent<Slot>().id] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[backpackSlot.GetComponent<Slot>().id] += "/" + _item.stats["currentMagazine"].ToString();
        }
        // Belt slot
        inventory.Add(beltSlot.GetComponent<Slot>().id, "");
        if (beltSlot.transform.childCount > 0 && beltSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[beltSlot.GetComponent<Slot>().id] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[beltSlot.GetComponent<Slot>().id] += "/" + _item.stats["currentMagazine"].ToString();
        }

        data.playerInventory.Clear();
        foreach(int id in inventory.Keys)
            data.playerInventory.Add(id, inventory[id]);
    }
}