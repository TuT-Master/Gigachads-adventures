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
        foreach (ScriptableObject so in GetComponent<Item>().recipe.Keys)
        {
            if (so.GetType() == typeof(ArmorSO)) itemsInRecipe.Add(new(so as ArmorSO));
            else if (so.GetType() == typeof(BackpackSO)) itemsInRecipe.Add(new(so as BackpackSO));
            else if (so.GetType() == typeof(BeltSO)) itemsInRecipe.Add(new(so as BeltSO));
            else if (so.GetType() == typeof(ConsumableSO)) itemsInRecipe.Add(new(so as ConsumableSO));
            else if (so.GetType() == typeof(MaterialSO)) itemsInRecipe.Add(new(so as MaterialSO));
            else if (so.GetType() == typeof(ProjectileSO)) itemsInRecipe.Add(new(so as ProjectileSO));
            else if (so.GetType() == typeof(ShieldSO)) itemsInRecipe.Add(new(so as ShieldSO));
            else if (so.GetType() == typeof(WeaponMeleeSO)) itemsInRecipe.Add(new(so as WeaponMeleeSO));
            else if (so.GetType() == typeof(WeaponRangedSO)) itemsInRecipe.Add(new(so as WeaponRangedSO));
            else if (so.GetType() == typeof(WeaponMagicSO)) itemsInRecipe.Add(new(so as WeaponMagicSO));

            var ingredient = Instantiate(ingredientPrefab, transform.Find("Ingredients"));
            ingredient.GetComponent<Image>().sprite = itemsInRecipe[itemsInRecipe.Count - 1].sprite_inventory;
            ingredient.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<Item>().recipe[so].ToString();
            ingredients.Add(ingredient);
        }
    }

    private bool CanBeCrafted()
    {
        foreach(ScriptableObject so in GetComponent<Item>().recipe.Keys)
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
            foreach (ScriptableObject so in GetComponent<Item>().recipe.Keys)
            {
                Item itemFromSO = null;
                if (so.GetType().ToString() == "ArmorSO")
                    itemFromSO = new(so as ArmorSO);
                else if (so.GetType().ToString() == "BackpackSO")
                    itemFromSO = new(so as BackpackSO);
                else if (so.GetType().ToString() == "BeltSO")
                    itemFromSO = new(so as BeltSO);
                else if (so.GetType().ToString() == "ConsumableSO")
                    itemFromSO = new(so as ConsumableSO);
                else if (so.GetType().ToString() == "MaterialSO")
                    itemFromSO = new(so as MaterialSO);
                else if (so.GetType().ToString() == "ProjectileSO")
                    itemFromSO = new(so as ProjectileSO);
                else if (so.GetType().ToString() == "ShieldSO")
                    itemFromSO = new(so as ShieldSO);
                else if (so.GetType().ToString() == "WeaponMeleeSO")
                    itemFromSO = new(so as WeaponMeleeSO);
                else if (so.GetType().ToString() == "WeaponRangedSO")
                    itemFromSO = new(so as WeaponRangedSO);


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
            // Consume materials
            for (int i = 0; i < GetComponent<Item>().nextLevel.recipeMaterials.Count; i++)
            {
                Item itemFromSO = null;
                if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "ArmorSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as ArmorSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "BackpackSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as BackpackSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "BeltSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as BeltSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "ConsumableSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as ConsumableSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "MaterialSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as MaterialSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "ProjectileSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as ProjectileSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "ShieldSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as ShieldSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "WeaponMeleeSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as WeaponMeleeSO);
                else if (GetComponent<Item>().nextLevel.recipeMaterials[i].GetType().ToString() == "WeaponRangedSO")
                    itemFromSO = new(GetComponent<Item>().nextLevel.recipeMaterials[i] as WeaponRangedSO);


                int materialNeeded = GetComponent<Item>().recipe[GetComponent<Item>().nextLevel.recipeMaterials[i]];
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
        FindAnyObjectByType<PlayerInventory>().CloseItemCard();
        isBeingClicked = true;
        time = 0f;
        craftingSpeed = 1;
    }
    public void OnPointerUp(PointerEventData eventData) { isBeingClicked = false; }
    public void OnPointerEnter(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().OpenItemCard(GetComponent<Item>()); }
    public void OnPointerExit(PointerEventData eventData) { FindAnyObjectByType<PlayerInventory>().CloseItemCard(); }

}
