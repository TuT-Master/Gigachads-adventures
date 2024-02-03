using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] CanvasGroup canvasGroup;
    [HideInInspector] public Transform parentAfterDrag;

    public Slot.SlotType slotType;
    [HideInInspector] public int amount;
    [HideInInspector] public bool isStackable;

    void Awake()
    {
        switch (slotType)
        {
            case Slot.SlotType.WeaponMelee:
                amount = GetComponent<WeaponMelee>().amount;
                isStackable = GetComponent<WeaponMelee>().isStackable;
                break;
            case Slot.SlotType.WeaponRanged:
                amount = GetComponent<WeaponRanged>().amount;
                isStackable = GetComponent<WeaponRanged>().isStackable;
                break;
            case Slot.SlotType.Consumable:
                amount = GetComponent<Consumable>().amount;
                isStackable = GetComponent<Consumable>().isStackable;
                break;
            case Slot.SlotType.Ammo:
                // TODO
                break;
            case Slot.SlotType.Material:
                // TODO
                break;
            case Slot.SlotType.Belt:
                // TODO
                break;
            case Slot.SlotType.Backpack:
                // TODO
                break;
            case Slot.SlotType.All:
                // TODO
                break;
            case Slot.SlotType.Toolbar:
                // TODO
                break;
            default:
                amount = GetComponent<Armor>().amount;
                isStackable = false;
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
