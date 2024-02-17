using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public enum SlotType
    {
        All,
        WeaponMelee,
        WeaponRanged,
        Consumable,
        Ammo,
        Material,
        Toolbar,
        Head,
        HeadEquipment,
        Torso,
        TorsoEquipment,
        Legs,
        LegsEquipment,
        Gloves,
        GlovesEquipment,
        Belt,
        Backpack
    }
    public SlotType slotType;

    public Image image;

    public bool isActive;


    void Update()
    {
        if (isActive)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Item droppedItem = eventData.pointerDrag.GetComponent<Item>();
        if (transform.childCount == 0 && CanBePlaced(droppedItem.slotType))
        {
            // Placenutí do prázdného slotu
            GameObject dropped = eventData.pointerDrag;
            ItemUI item_DragHandler = dropped.GetComponent<ItemUI>();
            item_DragHandler.parentAfterDrag = transform;
        }
        else if (gameObject.transform.childCount == 1 &&
            droppedItem.isStackable &&
            transform.GetChild(0).gameObject.GetComponent<Item>().isStackable &&
            droppedItem.slotType == slotType)
        {
            // Spojení stackable itemù
            if(droppedItem.amount + gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount <= gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount += droppedItem.amount;
                Destroy(eventData.pointerDrag);
            }
            else
            {
                droppedItem.amount -= (gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize - gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount);
                gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount = gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize;
            }
        }
        else if (transform.childCount > 0)
        {
            // Prohození itemù
            transform.GetChild(0).SetParent(eventData.pointerDrag.GetComponent<ItemUI>().parentBeforeDrag);
            eventData.pointerDrag.GetComponent<ItemUI>().parentAfterDrag = transform;
        }
    }

    private bool CanBePlaced(SlotType itemSlotType)
    {
        if(slotType == SlotType.All)
            return true;
        else if(slotType == SlotType.Toolbar && itemSlotType == SlotType.WeaponMelee)
            return true;
        else if (slotType == SlotType.Toolbar && itemSlotType == SlotType.WeaponRanged)
            return true;
        else if(slotType == SlotType.Toolbar && itemSlotType == SlotType.Consumable)
            return true;
        else if(slotType == itemSlotType)
            return true;
        else
            return false;
    }
}
