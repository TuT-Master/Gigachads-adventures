using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableObject> loot;
    
    [SerializeField]
    private List<float> lootDropChanceInPercentage;

    private PlayerInventory playerInventory;


    private void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public void DropLoot()
    {
        Item item = null;
        for (int i = 0; i < loot.Count; i++)
        {
            if (new System.Random().Next(0, 10000) / 100 <= lootDropChanceInPercentage[i])
            {
                // Spawn loot item
                if (loot[i].GetType().ToString() == "ArmorSO")
                    item = new(loot[i] as ArmorSO);
                else if (loot[i].GetType().ToString() == "BackpackSO")
                    item = new(loot[i] as BackpackSO);
                else if (loot[i].GetType().ToString() == "BeltSO")
                    item = new(loot[i] as BeltSO);
                else if (loot[i].GetType().ToString() == "ConsumableSO")
                    item = new(loot[i] as ConsumableSO);
                else if (loot[i].GetType().ToString() == "MaterialSO")
                    item = new(loot[i] as MaterialSO);
                else if (loot[i].GetType().ToString() == "ProjectileSO")
                    item = new(loot[i] as ProjectileSO);
                else if (loot[i].GetType().ToString() == "ShieldSO")
                    item = new(loot[i] as ShieldSO);
                else if (loot[i].GetType().ToString() == "WeaponMeleeSO")
                    item = new(loot[i] as WeaponMeleeSO);
                else if (loot[i].GetType().ToString() == "WeaponRangedSO")
                    item = new(loot[i] as WeaponRangedSO);

                item.amount = GetRandomItemAmount();
                playerInventory.DropItemOnDaFloor(item, transform.position, FindAnyObjectByType<Dungeon>().currentRoom.transform);
            }
        }
    }

    private int GetRandomItemAmount()
    {
        return new System.Random().Next(0, 100) switch
        {
            <= 20 => 1,
            > 20 and <= 65 => 2,
            > 65 and <= 90 => 3,
            > 90 => 4
        };
    }
}
