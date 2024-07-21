using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public Transform parentBeforeDrag;
    [HideInInspector]
    public Transform parentAfterDrag;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;
        transform.parent.GetComponent<Slot>().SetDefaultImage();
        FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).HideItemCard();
        canvasGroup.blocksRaycasts = false;
        parentBeforeDrag = transform.parent;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root.Find("UI"));
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;
        transform.position = Input.mousePosition;
    }
    public void OnPointerDown(PointerEventData eventData) { FindAnyObjectByType<ItemCard>().HideItemCard(); }
    public void OnPointerEnter(PointerEventData eventData) { FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).ShowItemCard(GetComponent<Item>()); }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).HideItemCard(); }
}
