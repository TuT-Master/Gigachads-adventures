using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public bool playerInventoryOpen;


    [SerializeField]
    private GameObject inventory;


    // Toolbar
    [SerializeField] private GameObject[] allToolbarSlots;
    private List<GameObject> toolbarSlots = new();

    private bool canScrollAgain;
    private int toolbarId;
    private int toolbarSlotsCount;



    private void Start()
    {
        canScrollAgain = true;
        toolbarId = 0;
        toolbarSlotsCount = 2;
        ToggleInventory(false);
    }

    private void Update()
    {
        toolbarSlots.Clear();
        for (int i = 0; i < toolbarSlotsCount; i++)
            toolbarSlots.Add(allToolbarSlots[i]);

        MyInput();
        ActiveToolbarSlot();
    }

    void MyInput()
    {
        if (Input.GetButtonDown("Toggle inventory"))
            ToggleInventory(!playerInventoryOpen);

        if (Input.mouseScrollDelta.y < 0)
        {
            // Mouse wheel scroll down
            ScrollToolbar(false);
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            // Mouse wheel scroll up
            ScrollToolbar(true);
        }
    }

    public void ToggleInventory(bool open)
    {
        if(open)
        {
            inventory.SetActive(true);
            Time.timeScale = 0f;
            playerInventoryOpen = true;
        }
        else
        {
            inventory.SetActive(false);
            Time.timeScale = 1f;
            playerInventoryOpen = false;
        }
    }

    void ScrollToolbar(bool up)
    {
        if (up)
        {
            if (toolbarId + 1 >= toolbarSlotsCount)
                toolbarId = 0;
            else
                toolbarId++;
        }
        else
        {
            if (toolbarId - 1 < 0)
                toolbarId = toolbarSlotsCount - 1;
            else
                toolbarId--;
        }
    }

    void ActiveToolbarSlot()
    {
        if (toolbarSlots[toolbarId].transform.childCount != 0)
        {
            if (toolbarSlots[toolbarId].transform.GetChild(0).gameObject.TryGetComponent(out Item item))
                GetComponent<PlayerFight>().itemInHand = item;
        }
    }
}
