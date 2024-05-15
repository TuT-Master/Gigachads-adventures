using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OtherInventoryItem : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftShift))
            FindAnyObjectByType<PlayerOtherInventoryScreen>().ShiftClickOnItem(GetComponent<Item>(), transform.parent.parent.name);
    }
}
