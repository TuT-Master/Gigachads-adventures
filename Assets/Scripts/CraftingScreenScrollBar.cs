using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScreenScrollBar : MonoBehaviour
{
    private Scrollbar scrollBar;
    private RectTransform rectTransform;
    private RectTransform stone_rect;
    private RectTransform thisRectTrans;

    private void Start()
    {
        scrollBar = transform.parent.parent.parent.GetComponent<Scrollbar>();
        rectTransform = transform.parent.GetComponent<RectTransform>();
        stone_rect = GetComponent<RectTransform>();
        thisRectTrans = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Update scrollBar value
        float scrollBarValue = scrollBar.value;
        
        // Get height of scrollBar handle
        float height = rectTransform.rect.height;
        
        // Get height of stone
        float heightOfStone = stone_rect.rect.height;

        // Calculate Y position relative to scrollBar handle
        float relativeYpos = (height * scrollBarValue) + (heightOfStone / 2);

        // Apply relative Y position to RectTransform of this obj
        thisRectTrans.SetLocalPositionAndRotation(new(0, relativeYpos, 0), Quaternion.identity);
    }
}
