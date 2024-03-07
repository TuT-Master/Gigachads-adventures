using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSkill : MonoBehaviour
{
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
        skillDescription.HideSkillDetails();
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabId)
                tabs[i].SetActive(true);
            else
                tabs[i].SetActive(false);
        }
    }
}
