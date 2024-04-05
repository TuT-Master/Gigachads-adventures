using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    public OtherInventory currentInventory;

    public bool isOpened;


    [SerializeField]
    private GameObject otherInventoryScreen;

    private bool shouldSaveInventory;


    void Start()
    {
        ToggleOtherInventoryScreen(false);
        shouldSaveInventory = false;
    }

    public void UpdateInventory(OtherInventory otherInventory)
    {
        currentInventory = otherInventory;
    }

    void SaveInventory()
    {

    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        otherInventoryScreen.SetActive(toggle);
        if (toggle)
            shouldSaveInventory = true;
        if (!toggle && shouldSaveInventory)
        {
            SaveInventory();
            shouldSaveInventory = false;
            currentInventory.isOpened = false;
            currentInventory = null;
        }

        isOpened = toggle;
    }
}
