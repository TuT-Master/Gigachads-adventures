using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerSkill skill;
    private DungeonMap map;
    private PlayerOtherInventoryScreen playerOtherInventory;
    private PlayerCrafting playerCrafting;

    [SerializeField]
    private SkillDescription skillDescription;

    public bool canOpenScreen = true;


    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        skill = GetComponent<PlayerSkill>();
        map = GetComponent<DungeonMap>();
        playerOtherInventory = GetComponent<PlayerOtherInventoryScreen>();
        playerCrafting = GetComponent<PlayerCrafting>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && AnyScreenOpen())
            CloseEverything();
    }
    public bool AnyScreenOpen()
    {
        if(inventory.playerInventoryOpen || skill.skillScreenOpen || map.mapOpened || playerOtherInventory.isOpened || playerCrafting.isOpened)
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
        inventory.CloseItemCard();
        playerCrafting.ToggleScreen(false);
    }


    public void TogglePlayerCrafting(bool toggle)
    {
        CloseEverything();
        playerCrafting.ToggleScreen(toggle);
    }

    public void ToggleInventoryScreen(bool toggle)
    {
        CloseEverything();
        inventory.ToggleInventory(toggle);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        CloseEverything();
        skill.ToggleSkillScreen(toggle);
    }

    public void ToggleMap(bool toggle)
    {
        CloseEverything();
        map.ToggleMap(toggle);
    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        CloseEverything();
        playerOtherInventory.ToggleOtherInventoryScreen(toggle);
    }
}
