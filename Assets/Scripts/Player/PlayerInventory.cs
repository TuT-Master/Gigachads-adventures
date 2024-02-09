using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
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


    // Prefabs
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject itemOnDaFloorPrefab;
    [SerializeField]
    private GameObject itemCardPrefab;


    // Toolbar
    [SerializeField] private GameObject[] allToolbarSlots;
    private List<GameObject> toolbarSlots = new();
    private bool canScrollAgain;
    private int toolbarId;
    private int toolbarSlotsCount;


    // ItemCard
    private GameObject itemCard;
    private bool isItemCardOpen;


    private void Start()
    {
        isItemCardOpen = false;
        canScrollAgain = true;
        toolbarId = 0;
        toolbarSlotsCount = 2;
        ToggleInventory(false);
    }

    private void Update()
    {
        toolbarSlots.Clear();
        for (int i = 0; i < toolbarSlotsCount; i++)
            toolbarSlots.Add(allToolbarSlots[i]);

        MyInput();
        ActiveToolbarSlot();
        UpdateBackpack();
        UpdateBelt();
        UpdatePockets();
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
        ToggleInventory(!playerInventoryOpen);

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

        ToggleInventory(!playerInventoryOpen);
    }


    void MyInput()
    {
        if (Input.GetButtonDown("Toggle inventory"))
            ToggleInventory(!playerInventoryOpen);

        if (Input.mouseScrollDelta.y < 0)
        {
            // Mouse wheel scroll down
            ScrollToolbar(false);
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            // Mouse wheel scroll up
            ScrollToolbar(true);
        }
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

    void ScrollToolbar(bool up)
    {
        if (up)
        {
            if (toolbarId + 1 >= toolbarSlotsCount)
                toolbarId = 0;
            else
                toolbarId++;
        }
        else
        {
            if (toolbarId - 1 < 0)
                toolbarId = toolbarSlotsCount - 1;
            else
                toolbarId--;
        }
    }

    void ActiveToolbarSlot()
    {
        if (toolbarSlots[toolbarId].transform.childCount != 0)
        {
            if (toolbarSlots[toolbarId].transform.GetChild(0).gameObject.TryGetComponent(out Item item))
                GetComponent<PlayerFight>().itemInHand = item;
        }
    }
}