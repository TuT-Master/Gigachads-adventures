using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerSkill skill;


    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        skill = GetComponent<PlayerSkill>();
    }

    public void ToggleInventoryScreen(bool toggle)
    {
        skill.ToggleSkillScreen(false);
        inventory.ToggleInventory(toggle);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        inventory.ToggleInventory(false);
        skill.ToggleSkillScreen(toggle);
    }
}
