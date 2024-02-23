using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject skillScreen;

    public bool skillScreenOpen;

    [SerializeField]
    private List<GameObject> tabs;

    private PlayerInventory playerInventory;
    private HUDmanager hudManager;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        hudManager = GetComponent<HUDmanager>();
        skillScreenOpen = false;
        OpenTab(0);
        ToggleSkillScreen(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Toggle skillScreen"))
            hudManager.ToggleSkillScreen(!skillScreenOpen);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillScreenOpen = toggle;
        if (skillScreenOpen)
        {
            Time.timeScale = 0f;
            skillScreen.SetActive(true);
            OpenTab(0);
        }
        else
        {
            Time.timeScale = 1f;
            skillScreen.SetActive(false);
        }
    }

    public void StatButtonClicked() { OpenTab(0); }
    public void MeleeButtonClicked() { OpenTab(1); }
    public void RangedButtonClicked() { OpenTab(2); }
    public void MagicButtonClicked() { OpenTab(3); }

    private void OpenTab(int tabId)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabId)
                tabs[i].SetActive(true);
            else
                tabs[i].SetActive(false);
        }
        switch (tabId)
        {
            case 0:
                // Stat tab
                Debug.Log("Stat tab opened!");
                break;
            case 1:
                // Melee tab
                Debug.Log("Melee tab opened!");

                break;
            case 2:
                // Ranged tab
                Debug.Log("Range tab opened!");

                break;
            case 3:
                // Magic tab
                Debug.Log("Magic tab opened!");

                break;
        }
    }
}
