using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolbar : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Image toolbarSlot1;
    [SerializeField]
    Image toolbarSlot2;
    [SerializeField]
    Image toolbarSlot3;
    [SerializeField]
    Image toolbarSlot4;
    [SerializeField]
    Image toolbarSlot5;
    [SerializeField]
    private List<GameObject> toolbarSlots = new();
    private List<GameObject> activeToolbarSlots = new();
    private int toolbarId = 0;

    private bool updateToolbar = true;

    void Update()
    {
        ToolbarSlots();
        if(updateToolbar)
            UpdateToolbarGFX();
        MyInput();
    }

    private void ToolbarSlots()
    {
        activeToolbarSlots.Clear();
        foreach (var slot in toolbarSlots)
            if (slot.GetComponent<Slot>().isActive)
                activeToolbarSlots.Add(slot);
    }

    void UpdateToolbarGFX()
    {
        Item item;
        switch(activeToolbarSlots.Count)
        {
            case 2:
                for(int i = 0; i < 5; i++)
                {
                    switch(i)
                    {
                        case 0:
                            toolbarSlot1.sprite = null;
                            if (activeToolbarSlots[toolbarId].transform.childCount > 0 &&
                                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item))
                                toolbarSlot1.sprite = item.sprite_inventory;
                            break;
                        case 1:
                            toolbarSlot2.sprite = null;
                            if (activeToolbarSlots[toolbarId].transform.childCount > 0 &&
                                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item))
                                toolbarSlot2.sprite = item.sprite_inventory;
                            break;
                        case 2:
                            toolbarSlot3.sprite = null;
                            if (activeToolbarSlots[toolbarId].transform.childCount > 0 &&
                                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item))
                                toolbarSlot3.sprite = item.sprite_inventory;
                            break;
                        case 3:
                            toolbarSlot4.sprite = null;
                            if (activeToolbarSlots[toolbarId].transform.childCount > 0 &&
                                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item))
                                toolbarSlot4.sprite = item.sprite_inventory;
                            break;
                        case 4:
                            toolbarSlot5.sprite = null;
                            if (activeToolbarSlots[toolbarId].transform.childCount > 0 &&
                                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item))
                                toolbarSlot5.sprite = item.sprite_inventory;
                            break;
                    }
                }
                break;
            case 3:

                break;
            case 4:

                break;
            case >= 5:

                break;
            default:
                Debug.Log("Tak co ti je??");
                break;
        }
    }

    void MyInput()
    {
        if (Input.mouseScrollDelta.y < 0)
            ScrollToolbar(false);
        else if (Input.mouseScrollDelta.y > 0)
            ScrollToolbar(true);
    }

    void ScrollToolbar(bool up)
    {
        updateToolbar = false;
        if (up)
        {
            if (toolbarId + 1 >= activeToolbarSlots.Count)
                toolbarId = 0;
            else
                toolbarId++;
        }
        else
        {
            if (toolbarId - 1 < 0)
                toolbarId = activeToolbarSlots.Count - 1;
            else
                toolbarId--;
        }
        StartCoroutine(ToolbarAnimation(up));
    }

    IEnumerator ToolbarAnimation(bool up)
    {
        if(up)
        {
            animator.SetTrigger("ToolbarUP");
            yield return new WaitForSeconds(0.25f);

        }
        else
        {
            animator.SetTrigger("ToolbarDOWN");
            yield return new WaitForSeconds(0.25f);

        }
        updateToolbar = true;
    }
}