using System;
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
    private Transform recipeTransform;

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
    private Item upgradedVersionOfItem;
    private string lastItemName = "";
    [SerializeField]
    private Transform upgradeRecipesTransform;
    [SerializeField]
    private GameObject ingredientPrefab;
    [SerializeField]
    private TextMeshProUGUI nameField;
    [SerializeField]
    private TextMeshProUGUI statField;


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
                nameField.text = "No possible upgrades!";
                statField.text = "";

                // TODO - zatmavit upgrade button, some hláška že no possible upgrades

            }
        }
        else if (upgradeSlot.transform.childCount == 0 && nameField.text != "")
        {
            for (int i = 0; i < upgradeRecipesTransform.childCount; i++)
                Destroy(upgradeRecipesTransform.GetChild(i).gameObject);
            nameField.text = "";
            statField.text = "";
            lastItemName = "";
            itemInUpgradeSlot = null;
        }
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

        // Get upgraded version of item
        ScriptableObject upgradedVar = itemInUpgradeSlot.upgradedVersionOfItem;

        upgradedVersionOfItem = null;
        if (upgradedVar.GetType() == typeof(WeaponMeleeSO)) upgradedVersionOfItem = itemDatabase.GetWeaponMelee((upgradedVar as WeaponMeleeSO).itemName);
        else if (upgradedVar.GetType() == typeof(WeaponRangedSO)) upgradedVersionOfItem = itemDatabase.GetWeaponRanged((upgradedVar as WeaponRangedSO).itemName);
        else if (upgradedVar.GetType() == typeof(WeaponMagicSO)) upgradedVersionOfItem = itemDatabase.GetWeaponMagic((upgradedVar as WeaponMagicSO).itemName);
        else if (upgradedVar.GetType() == typeof(ArmorSO)) upgradedVersionOfItem = itemDatabase.GetArmor((upgradedVar as ArmorSO).itemName);
        else if (upgradedVar.GetType() == typeof(ShieldSO)) upgradedVersionOfItem = itemDatabase.GetShield((upgradedVar as ShieldSO).itemName);

        // Create recipe
        foreach (Item item in upgradedVersionOfItem.GetMaterials())
        {
            GameObject ingredient = Instantiate(ingredientPrefab, upgradeRecipesTransform);
            ingredient.GetComponent<Image>().sprite = item.sprite_inventory;
            ingredient.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
        }

        // Name field
        nameField.text = itemInUpgradeSlot.itemName + " -> " + upgradedVersionOfItem.itemName;

        // Upgraded stats
        statField.text = "";
        foreach (string stat in upgradedVersionOfItem.stats.Keys)
        {
            if (upgradedVersionOfItem.stats[stat] > itemInUpgradeSlot.stats[stat])
                statField.text += stat + ": +" + Math.Round(upgradedVersionOfItem.stats[stat] - itemInUpgradeSlot.stats[stat], 2).ToString() + "\n";
            else if (upgradedVersionOfItem.stats[stat] < itemInUpgradeSlot.stats[stat])
                statField.text += stat + ": " + Math.Round(upgradedVersionOfItem.stats[stat] - itemInUpgradeSlot.stats[stat], 2).ToString() + "\n";
        }
    }

    public void UpgradeItem()
    {
        if (itemInUpgradeSlot != null && CanBeCrafted())
        {
            // Destroy item
            Destroy(upgradeSlot.transform.GetChild(0).gameObject);

            // Consume materials for upgrade
            ConsumeMaterialsForUpgrade();

            // Spawn new item
            AddItem(GetComponent<PlayerInventory>().itemDatabase.GetItemByNameAndAmount(upgradedVersionOfItem.itemName, 1));

            // Reset itemInUpgradeSlot
            itemInUpgradeSlot = null;
        }
        else
            Debug.Log("Kokote");
    }

    private bool CanBeCrafted()
    {
        foreach (ScriptableObject so in upgradedVersionOfItem.recipe.Keys)
        {
            Item itemFromSO = null;
            if (so.GetType() == typeof(ArmorSO)) itemFromSO = new(so as ArmorSO);
            else if (so.GetType() == typeof(BackpackSO)) itemFromSO = new(so as BackpackSO);
            else if (so.GetType() == typeof(BeltSO)) itemFromSO = new(so as BeltSO);
            else if (so.GetType() == typeof(ConsumableSO)) itemFromSO = new(so as ConsumableSO);
            else if (so.GetType() == typeof(MaterialSO)) itemFromSO = new(so as MaterialSO);
            else if (so.GetType() == typeof(ProjectileSO)) itemFromSO = new(so as ProjectileSO);
            else if (so.GetType() == typeof(ShieldSO)) itemFromSO = new(so as ShieldSO);
            else if (so.GetType() == typeof(WeaponMeleeSO)) itemFromSO = new(so as WeaponMeleeSO);
            else if (so.GetType() == typeof(WeaponRangedSO)) itemFromSO = new(so as WeaponRangedSO);
            else if (so.GetType() == typeof(WeaponMagicSO)) itemFromSO = new(so as WeaponMagicSO);

            List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(itemFromSO.itemName);
            int totalMaterialAmount = 0;
            for (int i = 0; i < materials.Count; i++)
                totalMaterialAmount += materials[i].amount;

            if (totalMaterialAmount < upgradedVersionOfItem.recipe[so])
            {
                Debug.Log("Not enough materials for upgrading " + itemInUpgradeSlot.itemName);
                return false;
            }
        }
        return true;
    }

    private void ConsumeMaterialsForUpgrade()
    {
        foreach (ScriptableObject so in upgradedVersionOfItem.recipe.Keys)
        {
            Item itemFromSO = null;
            if (so.GetType() == typeof(ArmorSO)) itemFromSO = new(so as ArmorSO);
            else if (so.GetType() == typeof(BackpackSO)) itemFromSO = new(so as BackpackSO);
            else if (so.GetType() == typeof(BeltSO)) itemFromSO = new(so as BeltSO);
            else if (so.GetType() == typeof(ConsumableSO)) itemFromSO = new(so as ConsumableSO);
            else if (so.GetType() == typeof(MaterialSO)) itemFromSO = new(so as MaterialSO);
            else if (so.GetType() == typeof(ProjectileSO)) itemFromSO = new(so as ProjectileSO);
            else if (so.GetType() == typeof(ShieldSO)) itemFromSO = new(so as ShieldSO);
            else if (so.GetType() == typeof(WeaponMeleeSO)) itemFromSO = new(so as WeaponMeleeSO);
            else if (so.GetType() == typeof(WeaponRangedSO)) itemFromSO = new(so as WeaponRangedSO);
            else if (so.GetType() == typeof(WeaponMagicSO)) itemFromSO = new(so as WeaponMagicSO);

            int materialNeeded = upgradedVersionOfItem.recipe[so];

            List<GameObject> materials = new();
            for (int i = 0; i < playerInventorySlots.Count; i++)
                if (playerInventorySlots[i].transform.childCount > 0 && playerInventorySlots[i].transform.GetComponentInChildren<Item>().itemName == itemFromSO.itemName)
                    materials.Add(playerInventorySlots[i].transform.GetComponentInChildren<Item>().gameObject);

            int totalMaterialAmount = 0;
            for (int i = 0; i < materials.Count; i++)
                totalMaterialAmount += materials[i].GetComponent<Item>().amount;

            bool crafted = false;
            for (int i = 0; i < materials.Count; i++)
            {
                if (crafted)
                    break;

                if (materials[i].GetComponent<Item>().amount >= materialNeeded)
                {
                    materials[i].GetComponent<Item>().amount -= materialNeeded;
                    crafted = true;
                }
                else
                {
                    materialNeeded -= materials[i].GetComponent<Item>().amount;
                    materials[i].GetComponent<Item>().amount = 0;
                }
            }
        }
    }

    private void AddItem(Item item)
    {
        bool done = false;
        int freeSpaceId = -1;
        for (int i = 0; i < playerInventorySlots.Count; i++)
        {
            if (playerInventorySlots[i].activeInHierarchy && !done)
            {
                if (playerInventorySlots[i].transform.childCount == 0)
                {
                    if (freeSpaceId == -1)
                        freeSpaceId = i;
                }
                else if (
                    playerInventorySlots[i].GetComponentInChildren<Item>().itemName == item.itemName &&
                    playerInventorySlots[i].GetComponentInChildren<Item>().isStackable)
                {
                    if (playerInventorySlots[i].GetComponentInChildren<Item>().amount + item.amount <= playerInventorySlots[i].GetComponentInChildren<Item>().stackSize)
                    {
                        playerInventorySlots[i].GetComponentInChildren<Item>().amount += item.amount;
                        done = true;
                    }
                    else
                    {
                        item.amount -= (playerInventorySlots[i].GetComponentInChildren<Item>().stackSize - playerInventorySlots[i].GetComponentInChildren<Item>().amount);
                        playerInventorySlots[i].GetComponentInChildren<Item>().amount = playerInventorySlots[i].GetComponentInChildren<Item>().stackSize;
                    }
                }
            }
        }
        if (!done && freeSpaceId != -1)
        {
            GameObject newItem = Instantiate(itemPrefab, playerInventorySlots[freeSpaceId].transform);
            newItem.GetComponent<Item>().SetUpByItem(item);
            done = true;
        }
        if (!done)
        {
            Debug.Log("No space in inventory!");
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
        for (int i = 0; i < recipeTransform.childCount; i++)
            Destroy(recipeTransform.GetChild(i).gameObject);

        List<Item> allItems = itemDatabase.GetAllItems();
        int recipesAmount = 0;
        foreach (Item item in allItems)
            if (item.recipe.Count > 0 && item.requieredCraftingLevel <= playerBase.baseUpgrades[item.craftedIn])
            {
                // Show recipe
                GameObject recipe = Instantiate(recipePrefab, recipeTransform);
                recipe.GetComponent<Item>().SetUpByItem(item);
                recipe.GetComponent<Recipe>().CreateRecipe();
                recipesAmount++;
            }
        recipeTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, recipesAmount * 120);
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