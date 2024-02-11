using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CanvasGroup canvasGroup;
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        FindAnyObjectByType<PlayerInventory>().CloseItemCard();
        canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        if(GetComponent<Item>().slotType == Slot.SlotType.Head |
           GetComponent<Item>().slotType == Slot.SlotType.Torso |
           GetComponent<Item>().slotType == Slot.SlotType.Legs |
           GetComponent<Item>().slotType == Slot.SlotType.Gloves |
           GetComponent<Item>().slotType == Slot.SlotType.HeadEquipment |
           GetComponent<Item>().slotType == Slot.SlotType.TorsoEquipment |
           GetComponent<Item>().slotType == Slot.SlotType.LegsEquipment |
           GetComponent<Item>().slotType == Slot.SlotType.GlovesEquipment)
        {
            FindAnyObjectByType<PlayerStats>().UpdateEquipment();
        }
    }

    public void OnDrag(PointerEventData eventData) { transform.position = Input.mousePosition; }
    public void OnPointerDown(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
    public void OnPointerEnter(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().OpenItemCard(GetComponent<Item>()); }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
}
