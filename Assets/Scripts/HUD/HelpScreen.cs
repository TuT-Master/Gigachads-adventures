using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpScreen : MonoBehaviour
{
    [HideInInspector]
    public bool helpScreenToggle;

    [SerializeField]
    private GameObject helpScreen;

    [SerializeField]
    private TextMeshProUGUI counter;

    [SerializeField]
    private Image helpImage;
    [SerializeField]
    private List<Sprite> helpImages;
    private int currentImage = 0;


    void Start()
    {
        helpScreenToggle = false;
        helpScreen.SetActive(false);
        GetComponent<PlayerMovement>().canMove = true;
        GetComponent<HUDmanager>().canOpenScreen = true;
    }
    void Update()
    {
        counter.text = (currentImage + 1).ToString() + " / " + helpImages.Count.ToString();

        if (helpScreenToggle && Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(GoBackToESCScreen());
    }

    public void SlideImage(bool next)
    {
        if (helpImages.Count == 0)
            return;
        if(next)
        {
            if(currentImage + 1 < helpImages.Count)
                currentImage++;
            else
                currentImage = 0;
        }
        else
        {
            if (currentImage - 1 < 0)
                currentImage = helpImages.Count - 1;
            else
                currentImage--;
        }
        helpImage.sprite = helpImages[currentImage];
    }

    IEnumerator GoBackToESCScreen()
    {
        ToggleHelpScreen(false);
        yield return new WaitForEndOfFrame();
        GetComponent<ESCScreen>().ToggleESCScreen(true);
        currentImage = 0;
    }
    public void ToggleHelpScreen(bool toggle)
    {
        helpScreen.SetActive(toggle);
        GetComponent<PlayerMovement>().canMove = !toggle;
        GetComponent<HUDmanager>().canOpenScreen = !toggle;
        helpScreenToggle = toggle;
        currentImage = 0;
        if(helpImages.Count != 0)
            helpImage.sprite = helpImages[currentImage];
    }
}
