using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherInventory : MonoBehaviour, IInteractable
{
    public bool isLocked;

    public bool isOpened;

    public Dictionary<int, Item> inventory;



    public void SetUpInventory(Dictionary<int, Item> inventory, bool locked)
    {
        this.inventory = inventory;
        isLocked = locked;
        isOpened = false;
    }

    public void Interact()
    {
        HUDmanager hudManager = FindAnyObjectByType<HUDmanager>();
        hudManager.ToggleOtherInventoryScreen(true);

        PlayerOtherInventoryScreen otherInventoryScreen = FindAnyObjectByType<PlayerOtherInventoryScreen>();
        otherInventoryScreen.UpdateInventory(inventory);
    }

    public bool CanInteract() { return !isLocked; }
}
