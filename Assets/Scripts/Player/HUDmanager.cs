using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerSkill skill;
    [SerializeField]
    private SkillDescription skillDescription;


    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        skill = GetComponent<PlayerSkill>();
    }

    public void ToggleInventoryScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        skill.ToggleSkillScreen(false);
        inventory.ToggleInventory(toggle);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        skill.ToggleSkillScreen(toggle);
    }
}
