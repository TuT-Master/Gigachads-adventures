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
