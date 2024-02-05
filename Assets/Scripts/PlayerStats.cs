using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float hp;
    public float stamina;
    public float mana;
    public float hpMax;
    public float staminaMax;
    public float manaMax;
    
    public float armor;
    public float evasion;

    public float weight;
    public float speed;

    public int experience;
    public int level;

    public float accuracyBonus;
    public float penetrationBonus;
    public float armorIgnoreBonus;

    private Dictionary<string, float> basePlayerStats;

    private PlayerInventory playerInventory;
    private List<Item> armors;
    private List<Item> equipment;

    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        // Updating Lists
        for(int i = 0; i < playerInventory.armorSlots.transform.childCount; i++)
            if (playerInventory.armorSlots.transform.GetChild(i).TryGetComponent(out Item item))
                armors.Add(item);
        for (int i = 0; i < playerInventory.equipmentSlots.transform.childCount; i++)
            if (playerInventory.equipmentSlots.transform.GetChild(i).TryGetComponent(out Item item))
                equipment.Add(item);

        // Updating stats
        foreach (Item item in armors)
        {

        }
        foreach (Item item in equipment)
        {

        }
    }
}
