using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private CategoryButton[] categoryButtons;

    [SerializeField]
    private GameObject skillScreen;

    [SerializeField]
    private SkillDescription skillDescription;

    public bool skillScreenOpen;

    [SerializeField]
    private List<GameObject> tabs;

    private HUDmanager hudManager;


    void Start()
    {
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
        skillScreen.SetActive(skillScreenOpen);
        if (skillScreenOpen)
        {
            Time.timeScale = 0f;
            OpenTab(0);
            categoryButtons[0].clicked = true;
            categoryButtons[1].clicked = categoryButtons[2].clicked = categoryButtons[3].clicked = false;
        }
        else
            Time.timeScale = 1f;
    }
    public void StatButtonClicked(){ OpenTab(0); }
    public void MeleeButtonClicked() { OpenTab(1); }
    public void RangedButtonClicked() { OpenTab(2); }
    public void MagicButtonClicked() { OpenTab(3); }
    private void OpenTab(int tabId)
    {

        skillDescription.HideSkillDetails();

        if (tabId == 0)
            skillDescription.gameObject.SetActive(false);
        else
            skillDescription.gameObject.SetActive(true);

        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabId)
            {
                categoryButtons[i].clicked = true;
                tabs[i].SetActive(true);
            }
            else
            {
                categoryButtons[i].clicked = false;
                tabs[i].SetActive(false);
            }
        }
    }
}
