using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCScreen : MonoBehaviour
{
    [HideInInspector]
    public bool escScreenToggle;

    [SerializeField]
    private GameObject escScreen;

    [SerializeField]
    private Button_Continue button_Continue;
    [SerializeField]
    private Button_Help button_Help;
    [SerializeField]
    private Button_QuitGame button_QuitGame;


    void Start()
    {
        escScreenToggle = false;
        ToggleESCScreen(false);
    }

    void Update()
    {
        if (GetComponent<HelpScreen>().helpScreenToggle || !GetComponent<HUDmanager>().canOpenESCScreen)
            return;
        if(!GetComponent<HUDmanager>().AnyScreenOpen() && !GetComponent<HelpScreen>().helpScreenToggle)
            MyInput();
    }

    void MyInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            ToggleESCScreen(!escScreenToggle);
    }

    public void ToggleESCScreen(bool toggle)
    {
        if (toggle)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        escScreenToggle = toggle;
        escScreen.SetActive(toggle);
        GetComponent<PlayerMovement>().canMove = !toggle;
        GetComponent<HUDmanager>().canOpenScreen = !toggle;
        button_Continue.ReloadSprite();
        button_Help.ReloadSprite();
        button_QuitGame.ReloadSprite();
    }
}
