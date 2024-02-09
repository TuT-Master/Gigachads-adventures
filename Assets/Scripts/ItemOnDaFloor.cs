using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemOnDaFloor : MonoBehaviour, IInteractable
{
    private Item item;
    private bool canInteract;

    public void SetUpItemOnDaFloor(Item droppedItem)
    {
        canInteract = true;
        item = droppedItem;
        GetComponentInChildren<TextMeshPro>().text = item.amount.ToString();
        GetComponentInChildren<SpriteRenderer>().sprite = item.sprite_inventory;
    }

    public bool CanInteract() { return canInteract; }

    public void Interact()
    {
        FindAnyObjectByType<PlayerInventory>().AddItem(item);
        canInteract = false;
        Destroy(gameObject);
    }
}
