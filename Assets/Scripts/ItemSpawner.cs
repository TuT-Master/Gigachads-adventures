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


    void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public void Interact()
    {
        Item item;
        switch (Random.Range(0, 5))
        {
            case 0:
                Debug.Log("Melle weapon spawning!");
                item = itemDatabase.GetWeaponMelee(itemDatabase.weaponsMelee[Random.Range(0, itemDatabase.weaponsMelee.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                playerInventory.AddItem(item);
                break;
            case 1:
                Debug.Log("Ranged weapon spawning!");
                item = itemDatabase.GetWeaponRanged(itemDatabase.weaponsRanged[Random.Range(0, itemDatabase.weaponsRanged.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                playerInventory.AddItem(item);
                break;
            case 2:
                Debug.Log("Consumable spawning!");
                item = itemDatabase.GetConsumable(itemDatabase.consumables[Random.Range(0, itemDatabase.consumables.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                playerInventory.AddItem(item);
                break;
            case 3:
                Debug.Log("Armor spawning!");
                item = itemDatabase.GetArmor(itemDatabase.armors[Random.Range(0, itemDatabase.armors.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                playerInventory.AddItem(item);
                break;
            case 4:
                Debug.Log("Projectile spawning!");
                item = itemDatabase.GetProjectile(itemDatabase.projectiles[Random.Range(0, itemDatabase.projectiles.Count)].itemName);
                item.amount = Random.Range(1, item.stackSize);
                playerInventory.AddItem(item);
                break;
            default:
                Debug.Log("Default state!");
                break;
        }
    }

    public bool CanInteract() { return true; }
}
