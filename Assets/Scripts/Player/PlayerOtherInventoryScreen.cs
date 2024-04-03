using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    HUDmanager hudManager;

    [SerializeField]
    private GameObject otherInventoryScreen;

    private bool shouldSaveInventory;

    void Start()
    {
        hudManager = GetComponent<HUDmanager>();
        ToggleOtherInventoryScreen(false);
        shouldSaveInventory = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Interact"))
            hudManager.ToggleOtherInventoryScreen(false);
    }

    public void UpdateInventory(Dictionary<int, Item> otherInventory)
    {

    }

    void SaveInventory()
    {

    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        otherInventoryScreen.SetActive(toggle);
        if (toggle)
            shouldSaveInventory = true;
        if(shouldSaveInventory)
        {
            SaveInventory();
            shouldSaveInventory = false;
        }
    }
}
