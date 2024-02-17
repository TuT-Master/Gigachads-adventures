using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerToolbar : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject toolbarSlot1;
    [SerializeField]
    GameObject toolbarSlot2;
    [SerializeField]
    GameObject toolbarSlot3;
    [SerializeField]
    GameObject toolbarSlot4;
    [SerializeField]
    private List<GameObject> toolbarSlots = new();
    private List<GameObject> activeToolbarSlots = new();
    private int toolbarId = 0;



    void Update()
    {
        ToolbarSlots();
        MyInput();
    }

    private void ToolbarSlots()
    {
        foreach (var slot in toolbarSlots)
            if (slot.GetComponent<Slot>().isActive)
                activeToolbarSlots.Add(slot);
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
        ToolbarAnimation(up);
    }

    IEnumerator ToolbarAnimation(bool up)
    {
        if(up)
        {
            animator.SetTrigger("ToolbarUP");
            yield return new WaitForSeconds(0.25f);
            Debug.Log("Gatóv up");
        }
        else
        {
            animator.SetTrigger("ToolbarDOWN");
            yield return new WaitForSeconds(0.25f);
            Debug.Log("Gatóv down");
        }
    }
}