using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCrafting : MonoBehaviour
{
    public bool isOpened;

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
    [SerializeField]
    private PlayerBase playerBase;

    [SerializeField]
    private List<GameObject> craftingScreens;
    [SerializeField]
    private List<CraftingButtons> craftingButtons;

    // Upgrade screen
    [SerializeField]
    private GameObject upgradeSlot;
    private Item itemInUpgradeSlot;
    private string lastItemName = "";
    [SerializeField]
    private Transform upgradeRecipesTransform;
    [SerializeField]
    private GameObject ingredientPrefab;


    void Start()
    {
        hudmanager = GetComponent<HUDmanager>();
        playerInventory = GetComponent<PlayerInventory>();
        ToggleScreen(true);
        ToggleScreen(false);

        // TODO - load crafting leves from upgrades in player's base

    }

    void Update()
    {
        MyInput();
        if(upgradeSlot.transform.childCount > 0 && upgradeSlot.transform.GetChild(0).TryGetComponent(out itemInUpgradeSlot) && lastItemName != itemInUpgradeSlot.itemName)
        {
            lastItemName = itemInUpgradeSlot.itemName;
            if (itemInUpgradeSlot.upgradedVersionOfItem != null)
                CreateRecipesForUpgrade();
            else
            {
                Debug.Log("No possible upgrades!");

                // TODO - zatmavit upgrade button, some hláška že no possible upgrades

            }
        }
        else if (upgradeSlot.transform.childCount == 0 && upgradeRecipesTransform.childCount > 0)
            for (int i = 0; i < upgradeRecipesTransform.childCount; i++)
                Destroy(upgradeRecipesTransform.GetChild(i).gameObject);
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
            hudmanager.TogglePlayerCrafting(!isOpened);
    }

    private void CreateRecipesForUpgrade()
    {
        for (int i = 0; i < upgradeRecipesTransform.childCount; i++)
            Destroy(upgradeRecipesTransform.GetChild(i).gameObject);

        ScriptableObject upgradedVar = itemInUpgradeSlot.upgradedVersionOfItem;

        Item upgradedItem = null;
        if (upgradedVar.GetType() == typeof(WeaponMeleeSO)) upgradedItem = itemDatabase.GetWeaponMelee((upgradedVar as WeaponMeleeSO).itemName);
        else if (upgradedVar.GetType() == typeof(WeaponRangedSO)) upgradedItem = itemDatabase.GetWeaponRanged((upgradedVar as WeaponRangedSO).itemName);
        else if (upgradedVar.GetType() == typeof(WeaponMagicSO)) upgradedItem = itemDatabase.GetWeaponMagic((upgradedVar as WeaponMagicSO).itemName);
        else if (upgradedVar.GetType() == typeof(ArmorSO)) upgradedItem = itemDatabase.GetArmor((upgradedVar as ArmorSO).itemName);
        else if (upgradedVar.GetType() == typeof(ShieldSO)) upgradedItem = itemDatabase.GetShield((upgradedVar as ShieldSO).itemName);

        foreach (Item item in upgradedItem.GetMaterials())
        {
            GameObject ingredient = Instantiate(ingredientPrefab, upgradeRecipesTransform);
            ingredient.GetComponent<Image>().sprite = item.sprite_inventory;
            ingredient.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
        }
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
            if (item.recipe.Count > 0 && item.requieredCraftingLevel <= playerBase.baseUpgrades[item.craftedIn])
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
            CreateRecipes();
            craftingScreen.SetActive(toggle);
            OpenTab(0);
        }
        else if (!toggle && isOpened)
        {
            Time.timeScale = 1f;
            SaveInventory();
            isOpened = toggle;
            craftingScreen.SetActive(toggle);
        }
    }

    public void OpenTab(int tabId)
    {
        for (int i = 0; i < craftingScreens.Count; i++)
        {
            if (i == tabId)
            {
                craftingButtons[i].clicked = true;
                craftingScreens[i].SetActive(true);
            }
            else
            {
                craftingButtons[i].clicked = false;
                craftingScreens[i].SetActive(false);
            }
        }
    }
}