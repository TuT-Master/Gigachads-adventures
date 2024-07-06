using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScreenScrollBar : MonoBehaviour
{
    private Transform parent;
    private float scrollBarValue;

    void Start() { parent = transform.parent; }
    void Update()
    {
        scrollBarValue = transform.parent.parent.GetComponentInParent<Scrollbar>().value;
        float height = parent.GetComponent<RectTransform>().sizeDelta.y * scrollBarValue;
    }
}
