using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour
{
    public bool isOpened;

    public int craftingLevel = 0;


    [SerializeField]
    private GameObject craftingScreen;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private List<GameObject> playerInventorySlots;
    [SerializeField]
    private GameObject recipePrefab;
    [SerializeField]
    private Transform recipesTransform;

    [SerializeField]
    private ItemDatabase itemDatabase;

    private HUDmanager hudmanager;
    private PlayerInventory playerInventory;



    void Start()
    {
        hudmanager = GetComponent<HUDmanager>();
        playerInventory = GetComponent<PlayerInventory>();
        ToggleScreen(true);
        ToggleScreen(false);
    }

    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
            hudmanager.TogglePlayerCrafting(!isOpened);
    }

    public IEnumerator UpdatePlayerInventory()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < playerInventory.backpackInventory.transform.childCount; i++)
        {
            playerInventorySlots[i].GetComponent<Slot>().isActive = playerInventory.backpackInventory.transform.GetChild(i).GetComponent<Slot>().isActive;
            playerInventorySlots[i].SetActive(playerInventorySlots[i].GetComponent<Slot>().isActive);

            for(int j = 0; j < playerInventorySlots[i].transform.childCount; j++)
                Destroy(playerInventorySlots[i].transform.GetChild(j).gameObject);

            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0 && playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item itemInSlot))
            {
                var newItem = Instantiate(itemPrefab, playerInventorySlots[i].transform);
                newItem.GetComponent<Item>().SetUpByItem(itemInSlot);
            }
        }
    }

    private void SaveInventory()
    {
        for (int i = 0; i < playerInventorySlots.Count; i++)
        {
            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0)
                Destroy(playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).gameObject);

            if (playerInventorySlots[i].transform.childCount > 0)
                playerInventorySlots[i].transform.GetChild(0).SetParent(playerInventory.backpackInventory.transform.GetChild(i));
        }
    }

    private void CreateRecipes()
    {
        for (int i = 0; i < recipesTransform.childCount; i++)
            Destroy(recipesTransform.GetChild(i).gameObject);

        List<Item> allItems = itemDatabase.GetAllItems();
        int recipesAmount = 0;
        foreach (Item item in allItems)
            if (item.recipe.Count > 0 && item.requieredCraftingLevel <= craftingLevel)
            {
                // Show recipe
                GameObject recipe = Instantiate(recipePrefab, recipesTransform);
                recipe.GetComponent<Item>().SetUpByItem(item);
                recipe.GetComponent<Recipe>().CreateRecipe();
                recipesAmount++;
            }
        recipesTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, recipesAmount * 120);
    }

    public void ToggleScreen(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 0f;
            StartCoroutine(UpdatePlayerInventory());
            isOpened = toggle;
            craftingScreen.SetActive(toggle);
            CreateRecipes();
        }
        else if (!toggle && isOpened)
        {
            Time.timeScale = 1f;
            SaveInventory();
            isOpened = toggle;
            craftingScreen.SetActive(toggle);
        }
    }
}
