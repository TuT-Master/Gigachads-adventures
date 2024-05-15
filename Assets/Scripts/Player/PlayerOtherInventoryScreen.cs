using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    public Dictionary<int, string> otherInventory = new();
    private GameObject otherInventoryObj;

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
        playerInventory = GetComponent<PlayerInventory>();
        isOpened = false;
        otherInventoryScreen.SetActive(false);
    }

    public void UpdateOtherInventory(GameObject inventory)
    {
        otherInventoryObj = inventory;
        otherInventory = otherInventoryObj.GetComponent<OtherInventory>().inventory;

        for (int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (otherInventorySlots[i].transform.childCount > 0)
                Destroy(otherInventorySlots[i].transform.GetChild(0).gameObject);

            if (i < otherInventory.Count)
            {
                otherInventorySlots[i].transform.GetComponent<Slot>().isActive = true;
                otherInventorySlots[i].SetActive(true);

                if (otherInventory[i] != "" && otherInventory[i] != null)
                {
                    GameObject newItem = Instantiate(itemPrefab, otherInventorySlots[i].transform);
                    newItem.GetComponent<Item>().SetUpByItem(playerInventory.GetItemForLoading(otherInventory[i]));
                }
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
        for (int i = 0; i < playerInventorySlots.Count; i++)
        {
            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0)
                Destroy(playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).gameObject);

            if (playerInventorySlots[i].transform.childCount > 0)
                playerInventorySlots[i].transform.GetChild(0).SetParent(playerInventory.backpackInventory.transform.GetChild(i));
        }

        // Other inventory
        for (int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (otherInventorySlots[i].GetComponent<Slot>().isActive)
            {
                if (otherInventorySlots[i].transform.childCount > 0 && otherInventorySlots[i].transform.GetChild(0).TryGetComponent(out Item item))
                {
                    if (otherInventory.ContainsKey(i))
                        otherInventory[i] = item.itemName + "-" + item.amount.ToString();
                    else
                        otherInventory.Add(i, item.itemName + "-" + item.amount.ToString());

                    if (item.stats != null && item.stats.ContainsKey("currentMagazine") && item.stats["currentMagazine"] > 0)
                        otherInventory[i] += "/" + item.stats["currentMagazine"].ToString();
                }
                else
                    otherInventory[i] = "";
            }
        }
        otherInventoryObj.GetComponent<OtherInventory>().SetUpInventory(otherInventory, otherInventoryObj.GetComponent<OtherInventory>().isLocked);
        otherInventory = null;
    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        if (toggle)
        {
            UpdatePlayerInventory();
            Time.timeScale = 0f;
            otherInventoryObj.GetComponent<OtherInventory>().isOpened = isOpened = toggle;
            otherInventoryScreen.SetActive(toggle);
        }
        else if (!toggle && isOpened)
        {
            SaveInventory();
            Time.timeScale = 1f;
            otherInventoryScreen.SetActive(toggle);
            otherInventoryObj.GetComponent<OtherInventory>().isOpened = isOpened = toggle;
        }
    }
}
