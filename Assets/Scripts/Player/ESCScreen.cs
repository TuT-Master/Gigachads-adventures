using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCScreen : MonoBehaviour
{
    [HideInInspector]
    public bool escScreenToggle;

    [SerializeField]
    private GameObject escScreen;

    void Start()
    {
        escScreenToggle = false;
        ToggleESCScreen(false);
    }

    void Update()
    {
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
    }
}
