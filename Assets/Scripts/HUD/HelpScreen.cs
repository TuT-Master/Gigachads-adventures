using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    [HideInInspector]
    public bool helpScreenToggle;

    [SerializeField]
    private GameObject helpScreen;

    private bool closingScreen = false;


    void Start()
    {
        helpScreenToggle = false;
        helpScreen.SetActive(false);
        GetComponent<PlayerMovement>().canMove = true;
        GetComponent<HUDmanager>().canOpenScreen = true;
    }

    void Update()
    {
        if (helpScreenToggle && Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(GoBackToESCScreen());
    }

    IEnumerator GoBackToESCScreen()
    {
        ToggleHelpScreen(false);
        yield return new WaitForEndOfFrame();
        GetComponent<ESCScreen>().ToggleESCScreen(true);
    }

    public void ToggleHelpScreen(bool toggle)
    {
        helpScreen.SetActive(toggle);
        GetComponent<PlayerMovement>().canMove = !toggle;
        GetComponent<HUDmanager>().canOpenScreen = !toggle;
        helpScreenToggle = toggle;
    }
}
