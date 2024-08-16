using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDmanager : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerSkill skill;
    [SerializeField] private DungeonMap map;
    [SerializeField] private PlayerOtherInventoryScreen playerOtherInventory;
    [SerializeField] private PlayerCrafting playerCrafting;
    [SerializeField] private ItemCard itemCard;
    [SerializeField] private StartScreen startScreen;
    [SerializeField] private ESCScreen escScreen;


    [SerializeField]
    private SkillDescription skillDescription;

    public bool canOpenScreen = true;

    public bool canOpenESCScreen;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && AnyScreenOpen())
            CloseEverything();
        if(!AnyScreenOpen() && !canOpenESCScreen)
            StartCoroutine(CanOpenESCScreen());

        if(AnyScreenOpen() || escScreen.escScreenToggle)
        {
            canOpenESCScreen = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GetComponent<PlayerCamera>().canRotateY = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GetComponent<PlayerCamera>().canRotateY = true;
            itemCard.HideItemCard();
        }
    }

    IEnumerator CanOpenESCScreen()
    {
        yield return new WaitForSeconds(0.1f);
        canOpenESCScreen = true;
    }

    public bool AnyScreenOpen()
    {
        if(inventory.playerInventoryOpen || skill.skillScreenOpen || map.mapOpened || playerOtherInventory.isOpened || playerCrafting.isOpened || startScreen.startScreenOpen)
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
        itemCard.HideItemCard();
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
