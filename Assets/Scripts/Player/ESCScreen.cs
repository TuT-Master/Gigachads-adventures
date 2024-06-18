using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCScreen : MonoBehaviour
{
    [HideInInspector]
    public bool escScreenToggle;
    [HideInInspector]
    public bool helpScreenToggle;

    [SerializeField]
    private GameObject escScreen;
    [SerializeField]
    private GameObject helpScreen;

    [SerializeField]
    private GameObject button_continue;
    [SerializeField]
    private GameObject button_quitGame;
    [SerializeField]
    private GameObject button_help;

    private HUDmanager hudmanager;
    private PlayerMovement playerMovement;

    void Start()
    {
        escScreenToggle = helpScreenToggle = false;
        hudmanager = GetComponent<HUDmanager>();
        playerMovement = GetComponent<PlayerMovement>();
        ToggleHelpScreen(false);
        ToggleESCScreen(false);
    }

    void Update()
    {
        if(hudmanager.AnyScreenOpen())
            return;
        MyInput();
    }

    void MyInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            ToggleESCScreen(!escScreenToggle);
    }

    void ToggleESCScreen(bool toggle)
    {
        if (toggle)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        escScreenToggle = toggle;
        escScreen.SetActive(toggle);
        playerMovement.canMove = !toggle;
        hudmanager.canOpenScreen = !toggle;
    }

    void ToggleHelpScreen(bool toggle)
    {
        helpScreenToggle = toggle;
        helpScreen.SetActive(toggle);
        escScreen.SetActive(!toggle);
        playerMovement.canMove = !toggle;
        hudmanager.canOpenScreen = !toggle;
    }


    // Buttons
    public void Button_Continue()
    {

    }

    public void Button_QuitGame()
    {

    }

    public void Button_Help()
    {

    }
}
