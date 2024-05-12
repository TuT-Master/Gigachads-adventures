using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private GameObject ingredientPrefab;
    [SerializeField]
    private ItemDatabase itemDatabase;

    private List<GameObject> ingredients;

    private Item item;

    public void CreateRecipe()
    {
        item = GetComponent<Item>();
        ingredients = new();
        itemImage.sprite = item.sprite_inventory;
        foreach (MaterialSO material in item.recipe.Keys)
        {
            var ingredient = Instantiate(ingredientPrefab, transform.Find("Ingredients"));
            ingredient.GetComponent<Image>().sprite = material.sprite_inventory;
            ingredient.GetComponentInChildren<TextMeshProUGUI>().text = item.recipe[material].ToString();
            ingredients.Add(ingredient);
        }
    }

    private bool CanBeCrafted()
    {
        foreach(MaterialSO material in item.recipe.Keys)
        {
            List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(material.itemName);
            int totalMaterialAmount = 0;
            for (int i = 0; i < materials.Count; i++)
                totalMaterialAmount += materials[i].amount;

            if(totalMaterialAmount < item.recipe[material])
                return false;
        }
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO - craft the item
        if(CanBeCrafted())
        {
            foreach (MaterialSO material in item.recipe.Keys)
            {
                int materialNeeded = item.recipe[material];
                List<Item> materials = FindAnyObjectByType<PlayerInventory>().HasItem(material.itemName);
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
            item.amount = 1;
            FindAnyObjectByType<PlayerInventory>().AddItem(item);
            StartCoroutine(FindAnyObjectByType<PlayerCrafting>().UpdatePlayerInventory());
        }
        else
            Debug.Log("Not enough materials for this recipe!");
    }
    public void OnPointerDown(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
    public void OnPointerEnter(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().OpenItemCard(item); }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
}
