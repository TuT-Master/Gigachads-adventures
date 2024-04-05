using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    public OtherInventory currentInventory;

    public bool isOpened;


    [SerializeField]
    private GameObject otherInventoryScreen;
    [SerializeField]
    private List<GameObject> playerInventorySlots;
    [SerializeField]
    private List<GameObject> otherInventorySlots;

    private PlayerInventory playerInventory;

    private bool shouldSaveInventory;

    [SerializeField]
    private GameObject itemPrefab;



    void Start()
    {
        ToggleOtherInventoryScreen(false);
        shouldSaveInventory = false;
        playerInventory = GetComponent<PlayerInventory>();
    }

    public void UpdateOtherInventory(OtherInventory otherInventory)
    {
        currentInventory = otherInventory;
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
                Instantiate(itemPrefab, playerInventorySlots[i].transform);
                itemPrefab.GetComponent<Item>().SetUpByItem(itemInSlot);
            }
        }
    }

    void SaveInventory()
    {

    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        otherInventoryScreen.SetActive(toggle);
        if (toggle)
        {
            Time.timeScale = 0f;
            shouldSaveInventory = true;
            UpdatePlayerInventory();
        }
        if (!toggle && shouldSaveInventory)
        {
            SaveInventory();
            shouldSaveInventory = false;
            currentInventory.isOpened = false;
            currentInventory = null;
            Time.timeScale = 1f;
        }

        isOpened = toggle;
    }
}
