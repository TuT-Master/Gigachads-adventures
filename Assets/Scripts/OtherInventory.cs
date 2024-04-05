using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherInventory : MonoBehaviour, IInteractable
{
    public bool isLocked = false;

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
        isOpened = !isOpened;
        HUDmanager hudManager = FindAnyObjectByType<HUDmanager>();
        hudManager.ToggleOtherInventoryScreen(isOpened);

        PlayerOtherInventoryScreen otherInventoryScreen = FindAnyObjectByType<PlayerOtherInventoryScreen>();
        otherInventoryScreen.UpdateInventory(this);
    }

    public bool CanInteract() { return !isLocked; }
}
