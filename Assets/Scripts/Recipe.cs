using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private GameObject ingredientPrefab;
    [SerializeField]
    private ItemDatabase itemDatabase;

    private List<GameObject> ingredients;

    private bool isBeingClicked;
    private float time = 0f;
    private float craftingSpeed = 1f;
    private bool canCraftAgain = true;

    public void CreateRecipe()
    {
        itemImage.sprite = GetComponent<Item>().sprite_inventory;
        List<Item> itemsInRecipe = new();
        ingredients = new();
        foreach (ItemSO so in GetComponent<Item>().recipe.Keys)
        {
            itemsInRecipe.Add(so.ToItem());
            var ingredient = Instantiate(ingredientPrefab, transform.Find("Ingredients"));
            ingredient.GetComponent<Image>().sprite = itemsInRecipe[^1].sprite_inventory;
            ingredient.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<Item>().recipe[so].ToString();
            ingredients.Add(ingredient);
        }
    }

    private bool CanBeCrafted()
    {
        foreach(ItemSO so in GetComponent<Item>().recipe.Keys)
        {
            Item itemFromSO = so.ToItem();

            List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(itemFromSO.itemName);
            int totalMaterialAmount = 0;
            for (int i = 0; i < materials.Count; i++)
                totalMaterialAmount += materials[i].amount;

            if(totalMaterialAmount < GetComponent<Item>().recipe[so])
                return false;
        }
        return true;
    }

    private void Update()
    {
        if (!isBeingClicked || !CanBeCrafted())
            return;

        time += 0.015f;
        switch(time)
        {
            case > 1f and <= 2f:
                craftingSpeed = 2f;
                CraftItem();
                break;
            case > 2f and <= 2.5f:
                craftingSpeed = 4f;
                CraftItem();
                break;
            case > 2.5f and <= 3f:
                craftingSpeed = 8f;
                CraftItem();
                break;
            case > 3f and <= 3.5f:
                craftingSpeed = 16f;
                CraftItem();
                break;
            case > 3.5f and <= 4f:
                craftingSpeed = 32f;
                CraftItem();
                break;
            case > 4f:
                craftingSpeed = 64f;
                CraftItem();
                break;
        }
    }

    private void CraftItem()
    {
        if (CanBeCrafted() && canCraftAgain)
        {
            // Consume materials
            foreach (ItemSO so in GetComponent<Item>().recipe.Keys)
            {
                Item itemFromSO = so.ToItem();

                int materialNeeded = GetComponent<Item>().recipe[so];
                List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(itemFromSO.itemName);
                int totalMaterialAmount = 0;
                for (int i = 0; i < materials.Count; i++)
                    totalMaterialAmount += materials[i].amount;

                bool crafted = false;
                for (int i = 0; i < materials.Count; i++)
                {
                    if (crafted)
                        break;

                    if (materials[i].amount >= materialNeeded)
                    {
                        materials[i].amount -= materialNeeded;
                        crafted = true;
                    }
                    else
                    {
                        materialNeeded -= materials[i].amount;
                        materials[i].amount = 0;
                    }
                }
            }

            // Spawn item in inventory
            GetComponent<Item>().amount = 1;
            FindAnyObjectByType<PlayerInventory>().AddItem(GetComponent<Item>());
            StartCoroutine(FindAnyObjectByType<PlayerCrafting>().UpdatePlayerInventory());
            canCraftAgain = false;
            StartCoroutine(CanCraftAgain(1 / craftingSpeed));
        }
        else
            Debug.Log("Not enough materials for this recipe!");
    }
    private IEnumerator CanCraftAgain(float delay)
    {
        if(delay == 1)
            yield return null;
        else
            yield return new WaitForSeconds(delay);
        canCraftAgain = true;
    }

    private void CraftBaseUpgrade()
    {
        if (CanBeCrafted())
        {
            Item recipe = GetComponent<Item>();
            // Consume materials
            foreach(ItemSO so in recipe.recipe.Keys)
            {
                Item itemFromSO = so.ToItem();

                int materialNeeded = recipe.recipe[so];
                List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(itemFromSO.itemName);
                int totalMaterialAmount = 0;
                for (int j = 0; j < materials.Count; j++)
                    totalMaterialAmount += materials[j].amount;

                bool crafted = false;
                for (int j = 0; j < materials.Count; j++)
                {
                    if (crafted)
                        break;

                    if (materials[j].amount >= materialNeeded)
                    {
                        materials[j].amount -= materialNeeded;
                        crafted = true;
                    }
                    else
                    {
                        materialNeeded -= materials[j].amount;
                        materials[j].amount = 0;
                    }
                }
            }
            StartCoroutine(FindAnyObjectByType<PlayerCrafting>().UpdatePlayerInventory());
            FindAnyObjectByType<PlayerBase>().UpgradeBase(recipe.baseUpgradeType);
        }
        else
            Debug.Log("Not enough materials for this recipe!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GetComponent<Item>().baseUpgradeType == PlayerBase.BaseUpgrade.None)
            CraftItem();
        else
            CraftBaseUpgrade();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).HideItemCard();
        isBeingClicked = true;
        time = 0f;
        craftingSpeed = 1;
    }
    public void OnPointerUp(PointerEventData eventData) { isBeingClicked = false; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).ShowItemCard(GetComponent<Item>()));
        FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).pointerOnItemUI = true;
    }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<ItemCard>(FindObjectsInactive.Include).pointerOnItemUI = false; }

}
