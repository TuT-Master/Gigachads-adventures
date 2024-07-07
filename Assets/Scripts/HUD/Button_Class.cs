using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_Class : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Class settings")]
    [SerializeField]
    private StartScreen.ClassType type;

    [Header("Button settings")]
    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    private bool isPointerOnButton;
    private bool isPressed;
    public bool isChosen;


    void Update()
    {
        if (isPressed)
            GetComponent<Image>().sprite = sprite_clicked;
        else if (isPointerOnButton || isChosen)
            GetComponent<Image>().sprite = sprite_active;
        else if (!isPointerOnButton)
            GetComponent<Image>().sprite = sprite_inactive;
    }
    public void OnPointerEnter(PointerEventData eventData) { isPointerOnButton = true; }
    public void OnPointerExit(PointerEventData eventData) { isPointerOnButton = false; }
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Button_Class button in FindObjectsByType<Button_Class>(FindObjectsSortMode.None))
            button.isChosen = false;
        isPressed = true;
        isChosen = true;
        StartCoroutine(ButtonPressedDelay());
    }
    private IEnumerator ButtonPressedDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        FindAnyObjectByType<StartScreen>().ClassButtonClicked(type);
        isPressed = false;
    }
}
