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

    // Toolbar
    [SerializeField] private GameObject[] allToolbarSlots;
    private List<GameObject> toolbarSlots = new();

    private bool canScrollAgain;
    private int toolbarId;
    private int toolbarSlotsCount;

    // ItemCard
    [SerializeField]
    private GameObject itemCardPrefab;
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
            itemCard = Instantiate(itemCardPrefab, inventoryCanvas.transform);
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
            if(i < backpackSize)
                backpackInventory.transform.GetChild(i).gameObject.SetActive(true);
            else
                RemoveSlot(backpackInventory.transform, i);
        }
    }


    void UpdateBelt()
    {
        for (int i = 0; i < beltInventory.transform.childCount; i++)
        {
            if (i < beltSize)
                beltInventory.transform.GetChild(i).gameObject.SetActive(true);
            else
                RemoveSlot(beltInventory.transform, i);
        }
    }
    void UpdatePockets()
    {
        for (int i = 0; i < pocketsInventory.transform.childCount; i++)
        {
            if (i < pocketsSize)
                pocketsInventory.transform.GetChild(i).gameObject.SetActive(true);
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
        parent.GetChild(index).gameObject.SetActive(false);
    }
    void DropItemOnDaFloor(Item item)
    {
        Debug.Log("Dropping item in da floor " + item.itemName);
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
