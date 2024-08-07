using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemOnDaFloor : MonoBehaviour, IInteractable
{
    public Item item;
    private bool canInteract;

    public void SetUpItemOnDaFloor(Item droppedItem)
    {
        item = droppedItem;
        GetComponentInChildren<TextMeshPro>().text = item.amount.ToString();
        GetComponentInChildren<SpriteRenderer>().sprite = item.sprite_inventory;
        StartCoroutine(BecomeKinematic());
    }
    private IEnumerator BecomeKinematic()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody>().isKinematic = true;
        canInteract = true;
    }

    public bool CanInteract() { return canInteract; }

    public void Interact()
    {
        canInteract = false;
        FindAnyObjectByType<PlayerInventory>().AddItem(item);
        FindAnyObjectByType<PlayerInteract>().RemoveInteractable(this);
        Destroy(gameObject);
    }

    public Transform GetTransform() { return transform; }
}
