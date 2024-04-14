using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    public OtherInventory otherInventory;

    public bool isOpened;


    [SerializeField]
    private GameObject otherInventoryScreen;
    [SerializeField]
    private List<GameObject> playerInventorySlots;
    [SerializeField]
    private List<GameObject> otherInventorySlots;

    private PlayerInventory playerInventory;

    [SerializeField]
    private ItemDatabase itemDatabase;

    [SerializeField]
    private GameObject itemPrefab;



    void Start()
    {
        ToggleOtherInventoryScreen(false);
        playerInventory = GetComponent<PlayerInventory>();
    }

    public void UpdateOtherInventory(OtherInventory inventory)
    {
        otherInventory = inventory;
        for(int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (i < otherInventory.inventory.Count)
            {
                otherInventorySlots[i].transform.GetComponent<Slot>().isActive = true;
                otherInventorySlots[i].SetActive(true);
                try
                {
                    if(otherInventorySlots[i].transform.childCount == 0 && otherInventory.inventory[i].amount > 0)
                    {
                        var newItem = Instantiate(itemPrefab, otherInventorySlots[i].transform);
                        newItem.GetComponent<Item>().SetUpByItem(otherInventory.inventory[i]);
                    }
                }
                catch { }
            }
            else
            {
                otherInventorySlots[i].transform.GetComponent<Slot>().isActive = false;
                otherInventorySlots[i].SetActive(false);
            }
        }
    }

    private void UpdatePlayerInventory()
    {
        for (int i = 0; i < playerInventory.backpackInventory.transform.childCount; i++)
        {
            playerInventorySlots[i].GetComponent<Slot>().isActive = playerInventory.backpackInventory.transform.GetChild(i).GetComponent<Slot>().isActive;
            playerInventorySlots[i].SetActive(playerInventorySlots[i].GetComponent<Slot>().isActive);

            if (playerInventorySlots[i].transform.childCount > 0)
                Destroy(playerInventorySlots[i].transform.GetChild(0).gameObject);

            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0 && playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item itemInSlot))
            {
                var newItem = Instantiate(itemPrefab, playerInventorySlots[i].transform);
                newItem.GetComponent<Item>().SetUpByItem(itemInSlot);
            }
        }
    }

    public void SaveInventory()
    {
        // Player inventory
        for(int i = 0; i < playerInventorySlots.Count; i++)
        {
            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0)
                Destroy(playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).gameObject);

            if (playerInventorySlots[i].transform.childCount > 0)
                playerInventorySlots[i].transform.GetChild(0).SetParent(playerInventory.backpackInventory.transform.GetChild(i));
        }

        // Other inventory
        for(int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (otherInventorySlots[i].GetComponent<Slot>().isActive)
            {
                if (otherInventorySlots[i].transform.childCount > 0 && otherInventorySlots[i].transform.GetChild(0).TryGetComponent(out Item item))
                    otherInventory.inventory[i] = item;
                else
                    otherInventory.inventory[i] = null;
            }
        }
    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 0f;
            UpdatePlayerInventory();
        }
        if (!toggle && otherInventory != null && otherInventory.isOpened)
        {
            SaveInventory();
            otherInventory.isOpened = false;
            otherInventory = null;
            Time.timeScale = 1f;
        }

        otherInventoryScreen.SetActive(toggle);
        isOpened = toggle;
    }
}
