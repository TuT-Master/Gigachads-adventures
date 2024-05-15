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
    public GameObject backpackInventory;
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
    [SerializeField]
    private SpriteRenderer secondaryWeaponSpriteRenderer;

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
        // Left hand
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

        // Right hand
        if (RightHandSlot.transform.childCount > 0 && RightHandSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            secondaryWeaponSpriteRenderer.sprite = item.sprite_equip;
            GetComponent<PlayerFight>().secondaryItemInHand = item;
        }
        else
        {
            secondaryWeaponSpriteRenderer.sprite = null;
            GetComponent<PlayerFight>().secondaryItemInHand = null;
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
        if (playerStats.playerStats == null)
            return;
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
            itemCard = Instantiate(itemCardPrefab, Vector3.zero, Quaternion.identity, inventoryCanvas.transform.parent);
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
            
            // ItemCard GFX
            //itemCard.transform.GetChild(0).GetComponent<Image>().sprite = 
            
            // Item image
            itemCard.transform.GetChild(1).GetComponent<Image>().sprite = item.sprite_inventory;
            
            // Item name
            itemCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;
            
            // Item description
            itemCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;

            // Item stats
            string[] row = new string[12]; // 12 rows
            // Melle weapon
            if (item.slotType == Slot.SlotType.WeaponMelee)
            {
                if (item.TwoHanded())
                {
                    row[0] = "Two handed ";
                    if (item.weaponType == Item.WeaponType.QuarterStaff)
                        row[0] += "quarter staff";
                    else if (item.weaponType == Item.WeaponType.Spear)
                        row[0] += "spear";
                    else if (item.weaponType == Item.WeaponType.Longsword)
                        row[0] += "longsword";
                    else if (item.weaponType == Item.WeaponType.Halbert)
                        row[0] += "halbert";
                    else if (item.weaponType == Item.WeaponType.Hammer_twoHanded)
                        row[0] += "hammer";
                    else if (item.weaponType == Item.WeaponType.Zweihander)
                        row[0] += "zweihander";
                }
                else
                {
                    row[0] = "One handed ";
                    if (item.weaponType == Item.WeaponType.Whip)
                        row[0] += "whip";
                    else if (item.weaponType == Item.WeaponType.Dagger)
                        row[0] += "dagger";
                    else if (item.weaponType == Item.WeaponType.Sword)
                        row[0] += "sword";
                    else if (item.weaponType == Item.WeaponType.Rapier)
                        row[0] += "rapier";
                    else if (item.weaponType == Item.WeaponType.Axe)
                        row[0] += "axe";
                    else if (item.weaponType == Item.WeaponType.Mace)
                        row[0] += "mace";
                    else if (item.weaponType == Item.WeaponType.Hammer_oneHanded)
                        row[0] += "hammer";
                }

                row[1] = "Damage: " + item.stats["damage"].ToString();
                row[2] = "Penetration: " + item.stats["penetration"].ToString();
                row[3] = "Armor ignore: " + (item.stats["armorIgnore"] * 100).ToString() + "%";

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Ranged weapon
            else if (item.slotType == Slot.SlotType.WeaponRanged)
            {
                row[1] = "Damage: " + item.stats["damage"].ToString();
                row[2] = "Penetration: " + item.stats["penetration"].ToString();
                row[3] = "Armor ignore: " + (item.stats["armorIgnore"] * 100).ToString() + "%";
                row[4] = "Magazine: " + item.stats["currentMagazine"].ToString() + "/" + item.stats["magazineSize"].ToString();

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Armor
            else if (item.slotType == Slot.SlotType.Head | item.slotType == Slot.SlotType.Torso | item.slotType == Slot.SlotType.Legs | item.slotType == Slot.SlotType.Gloves)
            {
                row[1] = "Armor: " + item.armorStats["armor"].ToString();

                row[11] = "Weight: " + (item.armorStats["weight"] * item.amount).ToString() + " Kg";
            }
            // Equipable
            else if (item.slotType == Slot.SlotType.HeadEquipment | item.slotType == Slot.SlotType.TorsoEquipment | item.slotType == Slot.SlotType.LegsEquipment | item.slotType == Slot.SlotType.GlovesEquipment)
            {

            }
            // Consumable
            else if (item.slotType == Slot.SlotType.Consumable)
            {
                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Projectile
            else if (item.slotType == Slot.SlotType.Ammo)
            {
                row[1] = "Damage: " + item.stats["damage"].ToString();
                row[2] = "Penetration: " + item.stats["penetration"].ToString();
                row[3] = "Armor ignore: " + item.stats["armorIgnore"].ToString();
                if (item.stats["splashDamage"] > 0)
                {
                    row[4] = "Splash damage: " + item.stats["splashDamage"].ToString();
                    row[5] = "Splash radius: " + item.stats["splashRadius"].ToString();
                }

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Shield
            else if (item.slotType == Slot.SlotType.Shield)
            {
                row[1] = "Defense: " + item.stats["defense"].ToString();

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Backpack/Belt
            else if (item.slotType == Slot.SlotType.Backpack | item.slotType == Slot.SlotType.Belt)
            {
                row[1] = "Inventory slots: +" + item.stats["backpackSize"].ToString();

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            // Material
            else if (item.slotType == Slot.SlotType.Material)
            {

                row[11] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
            }
            else
                row[1] = "Tak na tohle jsem zapomnìl.";

            string text = "";
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] != null)
                    text += row[i];
                text += "\n";
            }
            itemCard.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = text;
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
            DropItemOnDaFloor(parent.GetChild(index).GetChild(0).GetComponent<Item>(), transform.position, null);
            Destroy(parent.GetChild(index).GetChild(0).gameObject);
        }
        parent.GetChild(index).GetComponent<Slot>().isActive = false;
    }
    public void DropItemOnDaFloor(Item item, Vector3 pos, Transform parent)
    {
        if(parent == null)
        {
            GameObject droppedItem = Instantiate(itemOnDaFloorPrefab, pos, Quaternion.identity);
            droppedItem.GetComponent<ItemOnDaFloor>().SetUpItemOnDaFloor(item);
        }
        else
        {
            GameObject droppedItem = Instantiate(itemOnDaFloorPrefab, pos, Quaternion.identity, parent);
            droppedItem.GetComponent<ItemOnDaFloor>().SetUpItemOnDaFloor(item);
        }
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
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        inventoryCanvas.SetActive(toggle);
    }

    IEnumerator LoadingDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (Transform transform in data.playerInventory.Keys)
            if (data.playerInventory[transform] != "")
                AddItemToSlot(GetItemForLoading(data.playerInventory[transform]), transform);
    }
    public Item GetItemForLoading(string item)
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
            else
            {
                if (stage == 0)
                    name += c;
                else if (stage == 1)
                    amountString += c;
                else if (stage == 2)
                    currentMagazineString += c;
            }
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
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
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
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
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
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[pocketsInventory.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Hand slots
        inventory.Add(LeftHandSlot.transform, "");
        inventory.Add(RightHandSlot.transform, "");
        if (LeftHandSlot.transform.childCount > 0 && LeftHandSlot.transform.GetChild(0).TryGetComponent(out Item _item))
        {
            inventory[LeftHandSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[LeftHandSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        if (RightHandSlot.transform.childCount > 0 && RightHandSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[RightHandSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[RightHandSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        // Armor slots
        for (int i = 0; i < armorSlots.transform.childCount; i++)
        {
            inventory.Add(armorSlots.transform.GetChild(i), "");
            if (armorSlots.transform.GetChild(i).childCount > 0 && armorSlots.transform.GetChild(i).GetChild(0).TryGetComponent(out Item item))
            {
                inventory[armorSlots.transform.GetChild(i)] = item.itemName + "-" + item.amount.ToString();
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
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
                if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                    inventory[equipmentSlots.transform.GetChild(i)] += "/" + item.stats["currentMagazine"].ToString();
            }
        }
        // Backpack slot
        inventory.Add(backpackSlot.transform, "");
        if (backpackSlot.transform.childCount > 0 && backpackSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[backpackSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[backpackSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }
        // Belt slot
        inventory.Add(beltSlot.transform, "");
        if (beltSlot.transform.childCount > 0 && beltSlot.transform.GetChild(0).TryGetComponent(out _item))
        {
            inventory[beltSlot.transform] = _item.itemName + "-" + _item.amount.ToString();
            if (_item.stats != null && _item.stats.ContainsKey("currentMagazine") && _item.stats["currentMagazine"] > 0)
                inventory[beltSlot.transform] += "/" + _item.stats["currentMagazine"].ToString();
        }

        data.playerInventory.Clear();
        foreach(Transform key in inventory.Keys)
            data.playerInventory.Add(key, inventory[key]);
    }
}