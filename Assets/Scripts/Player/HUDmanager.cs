using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerSkill skill;
    private DungeonMap map;
    private PlayerOtherInventoryScreen playerOtherInventory;

    [SerializeField]
    private SkillDescription skillDescription;


    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        skill = GetComponent<PlayerSkill>();
        map = GetComponent<DungeonMap>();
        playerOtherInventory = GetComponent<PlayerOtherInventoryScreen>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && AnyScreenOpen())
            CloseEverything();
    }
    private bool AnyScreenOpen()
    {
        if(inventory.playerInventoryOpen || skill.skillScreenOpen || map.mapOpened || playerOtherInventory.isOpened)
            return true;
        else
            return false;
    }
    private void CloseEverything()
    {
        skillDescription.HideSkillDetails();
        skill.ToggleSkillScreen(false);
        map.ToggleMap(false);
        playerOtherInventory.ToggleOtherInventoryScreen(false);
        inventory.ToggleInventory(false);
    }


    public void ToggleInventoryScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        skill.ToggleSkillScreen(false);
        map.ToggleMap(false);
        playerOtherInventory.ToggleOtherInventoryScreen(false);
        inventory.ToggleInventory(toggle);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        map.ToggleMap(false);
        playerOtherInventory.ToggleOtherInventoryScreen(false);
        skill.ToggleSkillScreen(toggle);
    }

    public void ToggleMap(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        skill.ToggleSkillScreen(false);
        playerOtherInventory.ToggleOtherInventoryScreen(false);
        map.ToggleMap(toggle);
    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        skillDescription.HideSkillDetails();
        inventory.ToggleInventory(false);
        skill.ToggleSkillScreen(false);
        map.ToggleMap(false);
        playerOtherInventory.ToggleOtherInventoryScreen(toggle);
    }
}
