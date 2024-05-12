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

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO - craft the item
        Debug.Log("Recipe clicked");

    }
    public void OnPointerDown(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
    public void OnPointerEnter(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().OpenItemCard(item); }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }
}
