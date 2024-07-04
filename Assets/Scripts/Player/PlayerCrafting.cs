using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerCrafting : MonoBehaviour
{
    [Header("Global stuff")]
    public bool isOpened;
    [SerializeField]
    private ItemDatabase itemDatabase;
    [SerializeField]
    private List<GameObject> playerInventorySlots;
    [SerializeField]
    private PlayerBase playerBase;
    private PlayerInventory playerInventory;
    private HUDmanager hudmanager;

    [Header("Screens & buttons")]
    [SerializeField]
    private List<GameObject> craftingScreens;
    [SerializeField]
    private List<CraftingButtons> craftingButtons;

    [Header("Item crafting")]
    [SerializeField]
    private GameObject craftingScreen;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform recipeTransform;
    [SerializeField]
    private GameObject recipePrefab;

    [Header("Item upgrading")]
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

    [Header("Magic weapon management")]
    [SerializeField]
    private GameObject magicWeaponSlot;
    [SerializeField]
    private Transform magicCrystalTransform;
    [SerializeField]
    private GameObject crystalSlotPrefab;
    private Item magicWeaponInSlot;

    [Header("Base upgrading")]
    [SerializeField]
    private Transform baseUpgradesTransform;


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

        // Upgrade screen
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

        // Magic weapon management screen
        if (magicWeaponSlot.transform.childCount > 0 && lastItemName == "")
        {
            magicWeaponInSlot = magicWeaponSlot.GetComponentInChildren<Item>();
            CreateMagicCrystalSlots(magicWeaponInSlot);
        }
        else if (magicWeaponSlot.transform.childCount == 0 && lastItemName != "")
        {
            DeleteMagicCrystalSlots(magicWeaponInSlot);
            magicWeaponInSlot = null;
        }

        if (magicCrystalTransform.childCount > 0)
            UpdateMagicCrystals(magicWeaponInSlot);
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
            hudmanager.TogglePlayerCrafting(!isOpened);
    }

    public void CreateMagicCrystalSlots(Item item)
    {
        lastItemName = item.itemName;
        if (item == null || item.slotType != Slot.SlotType.MagicWeapon)
            return;
        for (int i = 0; i < item.magicCrystals.Count; i++)
        {
            // Create new empty slot
            GameObject crystalSlot = Instantiate(crystalSlotPrefab, magicCrystalTransform);
            crystalSlot.GetComponent<Slot>().slotType = Slot.SlotType.MagicCrystal;
            crystalSlot.GetComponent<Slot>().isActive = true;

            // Fill slot
            if (item.magicCrystals[i] != Item.MagicCrystalType.None)
            {
                GameObject crystal = Instantiate(itemPrefab, crystalSlot.transform);
                crystal.GetComponent<Item>().SetUpByItem(itemDatabase.GetCrystalByType(item.magicCrystals[i]));
                crystal.GetComponent<Item>().amount = 1;
            }
        }
    }

    public void DeleteMagicCrystalSlots(Item item)
    {
        UpdateMagicCrystals(item);
        for (int i = 0; i < magicCrystalTransform.childCount; i++)
            Destroy(magicCrystalTransform.GetChild(i).gameObject);
        lastItemName = "";
    }

    private void UpdateMagicCrystals(Item item)
    {
        if(item == null)
            return;
        item.magicCrystals = new();
        for (int i = 0; i < magicCrystalTransform.childCount; i++)
        {
            if (magicCrystalTransform.GetChild(i).childCount > 0 && magicCrystalTransform.GetChild(i).GetChild(0).TryGetComponent(out Item crystal))
                item.magicCrystals.Add(i, crystal.crystalType);
            else
                item.magicCrystals.Add(i, Item.MagicCrystalType.None);
        }
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

    public void CreateBaseUpgrades()
    {
        for (int i = 0; i < baseUpgradesTransform.childCount; i++)
            Destroy(baseUpgradesTransform.GetChild(i).gameObject);

        int recipesAmount = 0;
        foreach (PlayerBase.BaseUpgrade baseUpgrade in playerBase.baseUpgrades.Keys)
            if (baseUpgrade != PlayerBase.BaseUpgrade.Upgrade && baseUpgrade != PlayerBase.BaseUpgrade.None)
            {
                GameObject upgrade = Instantiate(recipePrefab, baseUpgradesTransform);
                // Set up an upgrade recipe
                if (playerBase.baseUpgrades[baseUpgrade] == null)
                {
                    // Not upgraded yet
                    upgrade.GetComponent<Item>().SetUpByItem(itemDatabase.GetBaseUpgradeAsItem(baseUpgrade, 1));
                    upgrade.GetComponent<Recipe>().CreateRecipe();
                }
                else if (playerBase.baseUpgrades[baseUpgrade].nextLevel != null &&
                        playerBase.baseUpgrades[baseUpgrade].nextLevel.requieredAge <= GetComponent<PlayerStats>().playerStats["age"])
                {
                    // Upgraded already

                }
                recipesAmount++;
            }
        baseUpgradesTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, recipesAmount * 120);
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
            if (item.recipe.Count > 0 &&
                    (item.requieredCraftingLevel == 0 ||
                    (playerBase.baseUpgrades[item.craftedIn] != null && item.requieredCraftingLevel <= playerBase.baseUpgrades[item.craftedIn].levelOfUpgrade)))
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
            CreateBaseUpgrades();
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