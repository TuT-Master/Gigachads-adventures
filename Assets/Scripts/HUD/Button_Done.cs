using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Done : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private int screenIndex;
    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    private bool isPointerOnButton;
    private bool isPressed;


    void Update()
    {
        if (isPressed)
            GetComponent<Image>().sprite = sprite_clicked;
        else if (isPointerOnButton)
            GetComponent<Image>().sprite = sprite_active;
        else if (!isPointerOnButton)
            GetComponent<Image>().sprite = sprite_inactive;
    }
    public void OnPointerEnter(PointerEventData eventData) { isPointerOnButton = true; }
    public void OnPointerExit(PointerEventData eventData) { isPointerOnButton = false; }
    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = true;
        StartCoroutine(ButtonPressedDelay());
    }
    private IEnumerator ButtonPressedDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        FindAnyObjectByType<StartScreen>().ButtonDoneClicked(screenIndex);
        isPressed = false;
    }
}
