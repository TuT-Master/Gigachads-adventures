using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScreenScrollBar : MonoBehaviour
{
    void Update()
    {
        // Update scrollBar value
        float scrollBarValue = transform.parent.parent.parent.GetComponent<Scrollbar>().value;
        
        // Get height of scrollBar handle
        float height = transform.parent.GetComponent<RectTransform>().rect.height;
        
        // Get height of stone
        float heightOfStone = GetComponent<RectTransform>().rect.height;

        // Calculate Y position relative to scrollBar handle
        float relativeYpos = (height * scrollBarValue) + (heightOfStone / 2);

        // Apply relative Y position to RectTransform of this obj
        GetComponent<RectTransform>().SetLocalPositionAndRotation(new(0, relativeYpos, 0), Quaternion.identity);
    }
}
