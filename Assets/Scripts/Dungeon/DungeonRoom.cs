using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public List<GameObject> population = new();
    public List<IInteractableEnemy> enemies = new();
    //public List<MineableResource> resources = new();

    public TextMeshProUGUI messageObj;
    public bool cleared;

    private List<OtherInventory> lootBoxes = new();
    public List<Door> doors = new();

    private void Update()
    {
        if (enemies.Count == 0 && !cleared)
        {
            // Show message 'Room cleared!' which hides after 1.5 seconds
            FindAnyObjectByType<PlayerStats>().ShowMessage("Room cleared!", 1.5f);

            cleared = true;
            foreach (OtherInventory lootbox in lootBoxes)
                lootbox.isLocked = false;
            for (int i = 0; i < doors.Count; i++)
                doors[i].canInteract = true;
        }
    }

    public void StartRoom()
    {
        // Short loading screen

        // Wake everything and everybody up
        gameObject.SetActive(true);
        // Add loot
        for(int i = 0; i < transform.Find("Objs").childCount; i++)
            if (transform.Find("Objs").GetChild(i).TryGetComponent(out OtherInventory otherInventory))
            {
                otherInventory.isLocked = true;
                lootBoxes.Add(otherInventory);
                Item material = FindAnyObjectByType<ItemSpawner>().GetRandomMaterial();
                Dictionary<int, string> loot = new()
                {
                    {0, material.itemName + "-" + material.amount},
                };
                otherInventory.SetUpInventory(loot, false);
            }
    }
}
