using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerSkill skill;
    private DungeonMap map;
    [SerializeField]
    private SkillDescription skillDescription;


    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        skill = GetComponent<PlayerSkill>();
        map = GetComponent<DungeonMap>();
    }

    public void ToggleInventoryScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        skill.ToggleSkillScreen(false);
        map.ToggleMap(false);
        inventory.ToggleInventory(toggle);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        map.ToggleMap(false);
        skill.ToggleSkillScreen(toggle);
    }

    public void ToggleMap(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        skill.ToggleSkillScreen(false);
        map.ToggleMap(toggle);
    }
}
