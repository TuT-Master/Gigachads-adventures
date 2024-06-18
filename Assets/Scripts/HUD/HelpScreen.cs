using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    [HideInInspector]
    public bool helpScreenToggle;

    [SerializeField]
    private GameObject helpScreen;


    void Start()
    {
        helpScreenToggle = false;
        helpScreen.SetActive(false);
        GetComponent<PlayerMovement>().canMove = true;
        GetComponent<HUDmanager>().canOpenScreen = true;
    }

    void Update()
    {
        if(helpScreenToggle)
            MyInput();
    }

    void MyInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<ESCScreen>().ToggleESCScreen(true);
            ToggleHelpScreen(false);
        }
    }

    public void ToggleHelpScreen(bool toggle)
    {
        helpScreenToggle = toggle;
        helpScreen.SetActive(toggle);
        GetComponent<PlayerMovement>().canMove = !toggle;
        GetComponent<HUDmanager>().canOpenScreen = !toggle;
    }
}
