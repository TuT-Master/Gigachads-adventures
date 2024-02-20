using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject skillScreen;

    public bool skillScreenOpen;


    void Start()
    {
        ToggleSkillScreen(false);
    }

    void Update()
    {

    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillScreenOpen = toggle;
        if (skillScreenOpen)
        {
            Time.timeScale = 0f;
            skillScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            skillScreen.SetActive(false);
        }
    }
}
