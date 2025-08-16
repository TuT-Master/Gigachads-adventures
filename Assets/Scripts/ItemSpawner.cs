using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ItemDatabase itemDatabase;
    private PlayerInventory playerInventory;
    [SerializeField]
    private GameObject itemPrefab;

    [HideInInspector]
    public bool dropItem;

    void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public void Interact()
    {
        if(!dropItem)
            playerInventory.AddItem(GetRandomItem());
    }

    public Item GetRandomMaterial()
    {
        Item item = itemDatabase.materials[Random.Range(0, itemDatabase.materials.Count)].ToItem();
        item.amount = Random.Range(1, item.stackSize);
        return item;
    }

    public Item GetRandomItem()
    {
        Item item = null;
        switch (Random.Range(0, 8))
        {
            case 0:
                item = itemDatabase.GetWeaponMelee(itemDatabase.weaponsMelee[Random.Range(0, itemDatabase.weaponsMelee.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 1:
                item = itemDatabase.GetWeaponRanged(itemDatabase.weaponsRanged[Random.Range(0, itemDatabase.weaponsRanged.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 2:
                item = itemDatabase.GetConsumable(itemDatabase.consumables[Random.Range(0, itemDatabase.consumables.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 3:
                item = itemDatabase.GetArmor(itemDatabase.armors[Random.Range(0, itemDatabase.armors.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 4:
                item = itemDatabase.GetProjectile(itemDatabase.projectiles[Random.Range(0, itemDatabase.projectiles.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 5:
                item = itemDatabase.GetBackpack(itemDatabase.backpacks[Random.Range(0, itemDatabase.backpacks.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 6:
                item = itemDatabase.GetBelt(itemDatabase.belts[Random.Range(0, itemDatabase.belts.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            case 7:
                item = itemDatabase.GetShield(itemDatabase.shields[Random.Range(0, itemDatabase.shields.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                break;
            default:
                break;
        }
        return item;
    }

    public Item GetMaterial(int amount)
    {
        Item item = itemDatabase.GetMaterial(itemDatabase.materials[Random.Range(0, itemDatabase.materials.Count)].itemName);
        item.amount = amount;

        return item;
    }

    public bool CanInteract() { return true; }
    public Transform GetTransform() { return transform; }
}
