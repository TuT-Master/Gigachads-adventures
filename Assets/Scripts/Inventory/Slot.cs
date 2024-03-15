using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
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
        Backpack,
        SecondHand,
        Shield
    }
    public SlotType slotType;

    public SVGImage image;
    public List<Sprite> slotImages;

    public bool isActive;

    void Start() { SetDefaultImage(); }
    void Update()
    {
        if (isActive)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void SetDefaultImage()
    {
        switch(slotType)
        {
            case SlotType.All:
                image.sprite = slotImages[0];
                break;
            case SlotType.Head:
                image.sprite = slotImages[3];
                break;
            case SlotType.HeadEquipment:
                image.sprite = slotImages[7];
                break;
            case SlotType.Torso:
                image.sprite = slotImages[4];
                break;
            case SlotType.TorsoEquipment:
                image.sprite = slotImages[8];
                break;
            case SlotType.Legs:
                image.sprite = slotImages[5];
                break;
            case SlotType.LegsEquipment:
                image.sprite = slotImages[9];
                break;
            case SlotType.Gloves:
                image.sprite = slotImages[6];
                break;
            case SlotType.GlovesEquipment:
                image.sprite = slotImages[10];
                break;
            case SlotType.Belt:
                image.sprite = slotImages[12];
                break;
            case SlotType.Backpack:
                image.sprite = slotImages[11];
                break;
            case SlotType.Toolbar:
                if (gameObject.name == "LeftHandSlot")
                    image.sprite = slotImages[1];
                else
                    image.sprite = slotImages[0];
                break;
            case SlotType.SecondHand:
                image.sprite = slotImages[2];
                break;
        }
    }

    public bool HasItem(out Item item)
    {
        if(transform.childCount > 0)
        {
            item = transform.GetComponentInChildren<Item>();
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Item droppedItem = eventData.pointerDrag.GetComponent<Item>();
        if (transform.childCount == 0 && CanBePlaced(droppedItem))
        {
            // Placenutí do prázdného slotu
            GameObject dropped = eventData.pointerDrag;
            ItemUI item_DragHandler = dropped.GetComponent<ItemUI>();
            item_DragHandler.parentAfterDrag = transform;
            image.sprite = slotImages[0];
        }
        else if (transform.childCount == 1 && CanBePlaced(droppedItem))
        {
            if (gameObject.transform.childCount == 1 &&
            droppedItem.isStackable &&
            transform.GetChild(0).gameObject.GetComponent<Item>().isStackable &&
            droppedItem.itemName == transform.GetChild(0).gameObject.GetComponent<Item>().itemName)
            {
                // Spojení stackable itemù
                if (droppedItem.amount + gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount <= gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize)
                {
                    gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount += droppedItem.amount;
                    Destroy(eventData.pointerDrag);
                }
                else
                {
                    droppedItem.amount -= gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize - gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount;
                    gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().amount = gameObject.transform.GetChild(0).gameObject.GetComponent<Item>().stackSize;
                }
            }
            else
            {
                // Prohození itemù
                transform.GetChild(0).SetParent(eventData.pointerDrag.GetComponent<ItemUI>().parentBeforeDrag);
                eventData.pointerDrag.GetComponent<ItemUI>().parentAfterDrag = transform;
            }
            image.sprite = slotImages[0];
        }
    }

    private bool CanBePlaced(Item dropped)
    {
        if(slotType == SlotType.All)
            return true;
        else if(slotType == SlotType.Toolbar && dropped.slotType == SlotType.WeaponMelee)
            return true;
        else if (slotType == SlotType.Toolbar && dropped.slotType == SlotType.WeaponRanged)
            return true;
        else if(slotType == SlotType.Toolbar && dropped.slotType == SlotType.Consumable)
            return true;
        else if(slotType == dropped.slotType)
            return true;
        else if(slotType == SlotType.SecondHand && dropped.slotType == SlotType.Shield && !FindAnyObjectByType<PlayerInventory>().TwoHandedWeaponInFirstSlot())
            return true;
        else if (slotType == SlotType.SecondHand && dropped.slotType == SlotType.WeaponMelee && dropped.stats["twoHanded"] == 0)
            return true;
        else if (slotType == SlotType.SecondHand && dropped.slotType == SlotType.Consumable && !FindAnyObjectByType<PlayerInventory>().TwoHandedWeaponInFirstSlot())
            return true;
        else
            return false;
    }
}
