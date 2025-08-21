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
        Item item = Random.Range(0, 9) switch
        {
            0 => itemDatabase.GetWeaponMelee(itemDatabase.weaponsMelee[Random.Range(0, itemDatabase.weaponsMelee.Count)].itemName),
            1 => itemDatabase.GetWeaponRanged(itemDatabase.weaponsRanged[Random.Range(0, itemDatabase.weaponsRanged.Count)].itemName),
            2 => itemDatabase.GetConsumable(itemDatabase.consumables[Random.Range(0, itemDatabase.consumables.Count)].itemName),
            3 => itemDatabase.GetArmor(itemDatabase.armors[Random.Range(0, itemDatabase.armors.Count)].itemName),
            4 => itemDatabase.GetProjectile(itemDatabase.projectiles[Random.Range(0, itemDatabase.projectiles.Count)].itemName),
            5 => itemDatabase.GetBackpack(itemDatabase.backpacks[Random.Range(0, itemDatabase.backpacks.Count)].itemName),
            6 => itemDatabase.GetBelt(itemDatabase.belts[Random.Range(0, itemDatabase.belts.Count)].itemName),
            7 => itemDatabase.GetShield(itemDatabase.shields[Random.Range(0, itemDatabase.shields.Count)].itemName),
            8 => itemDatabase.GetMaterial(itemDatabase.materials[Random.Range(0, itemDatabase.materials.Count)].itemName),
        };
        item.amount = Random.Range(1, item.stackSize);
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
