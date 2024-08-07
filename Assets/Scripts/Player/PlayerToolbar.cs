using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolbar : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image toolbarSlot1;
    [SerializeField] private Image toolbarSlot2;
    [SerializeField] private Image toolbarSlot3;
    [SerializeField] private Image toolbarSlot4;
    [SerializeField] private Image toolbarSlot5;

    [SerializeField]
    private List<GameObject> toolbarSlots = new();
    private List<GameObject> activeToolbarSlots = new();

    private int toolbarId = 0;

    private bool updateToolbar = true;
    private bool animateToolbar;
    private bool start = true;
    private bool canScrollAgain = true;

    void Update()
    {
        ToolbarSlots();
        if(updateToolbar)
            UpdateToolbarGFX();
        if(canScrollAgain)
            MyInput();
        animateToolbar = false;
        if (activeToolbarSlots.Count > 1)
            animateToolbar = true;
    }

    public Item GetActiveConsumable()
    {
        Item item = null;
        if (activeToolbarSlots.Count > 0)
            if (activeToolbarSlots[toolbarId].transform.childCount > 0)
                activeToolbarSlots[toolbarId].transform.GetChild(0).TryGetComponent(out item);
        return item;
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
        if(start)
        {
            toolbarSlot1.gameObject.SetActive(false);
            toolbarSlot2.gameObject.SetActive(false);
            toolbarSlot3.gameObject.SetActive(false);
            toolbarSlot4.gameObject.SetActive(false);
            toolbarSlot5.gameObject.SetActive(false);
            start = false;
        }
        if (activeToolbarSlots.Count > 0)
        {
            if(activeToolbarSlots.Count == 1)
            {
                toolbarId = 0;
                toolbarSlot1.gameObject.SetActive(false);
                toolbarSlot2.gameObject.SetActive(false);
                toolbarSlot4.gameObject.SetActive(false);
                toolbarSlot5.gameObject.SetActive(false);

                // Active consumable slot
                if (activeToolbarSlots[toolbarId].transform.childCount > 0 && activeToolbarSlots[toolbarId].GetComponentInChildren<Item>())
                {
                    toolbarSlot3.gameObject.SetActive(true);
                    toolbarSlot3.sprite = activeToolbarSlots[toolbarId].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                {
                    toolbarSlot3.sprite = null;
                    toolbarSlot3.gameObject.SetActive(false);
                }
            }
            else
            {
                // Active consumable slot
                if (activeToolbarSlots[toolbarId].transform.childCount > 0 && activeToolbarSlots[toolbarId].GetComponentInChildren<Item>())
                {
                    toolbarSlot3.gameObject.SetActive(true);
                    toolbarSlot3.sprite = activeToolbarSlots[toolbarId].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                {
                    toolbarSlot3.sprite = null;
                    toolbarSlot3.gameObject.SetActive(false);
                }
                // Active slot - 1
                int id = toolbarId - 1;
                if (id < 0)
                    id = activeToolbarSlots.Count - 1;
                if (activeToolbarSlots[id].transform.childCount > 0 && activeToolbarSlots[id].GetComponentInChildren<Item>())
                {
                    toolbarSlot2.gameObject.SetActive(true);
                    toolbarSlot2.sprite = activeToolbarSlots[id].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                    toolbarSlot2.gameObject.SetActive(false);

                // Active slot + 1
                id = toolbarId + 1;
                if (id >= activeToolbarSlots.Count)
                    id = 0;
                if (activeToolbarSlots[id].transform.childCount > 0 && activeToolbarSlots[id].GetComponentInChildren<Item>())
                {
                    toolbarSlot4.gameObject.SetActive(true);
                    toolbarSlot4.sprite = activeToolbarSlots[id].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                    toolbarSlot2.gameObject.SetActive(false);

                // Active slot - 2
                id = toolbarId - 2;
                if (id == -1)
                    id = activeToolbarSlots.Count - 1;
                else if (id == -2)
                    id = activeToolbarSlots.Count - 2;
                if (activeToolbarSlots[id].transform.childCount > 0 && activeToolbarSlots[id].GetComponentInChildren<Item>())
                {
                    toolbarSlot1.gameObject.SetActive(true);
                    toolbarSlot1.sprite = activeToolbarSlots[id].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                    toolbarSlot2.gameObject.SetActive(false);

                // Active slot + 2
                id = toolbarId + 2;
                if (id == activeToolbarSlots.Count)
                    id = 0;
                else if (id == activeToolbarSlots.Count + 1)
                    id = 1;
                if (activeToolbarSlots[id].transform.childCount > 0 && activeToolbarSlots[id].GetComponentInChildren<Item>())
                {
                    toolbarSlot5.gameObject.SetActive(true);
                    toolbarSlot5.sprite = activeToolbarSlots[id].GetComponentInChildren<Item>().sprite_inventory;
                }
                else
                    toolbarSlot2.gameObject.SetActive(false);
            }
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
        if (!animateToolbar)
            return;
        canScrollAgain = false;
        updateToolbar = false;
        if (activeToolbarSlots.Count > 1)
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
            StartCoroutine(ToolbarAnimation(up));
        }
    }

    IEnumerator ToolbarAnimation(bool up)
    {
        if(up)
            animator.SetTrigger("ToolbarUP");
        else
            animator.SetTrigger("ToolbarDOWN");

        yield return new WaitForSeconds(0.75f);
        canScrollAgain = true;
        updateToolbar = true;
    }
}