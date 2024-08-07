using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Done : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public bool isActive;

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
        if (isActive)
        {
            GetComponent<Image>().color = Color.white;
            if (isPressed)
                GetComponent<Image>().sprite = sprite_clicked;
            else if (isPointerOnButton)
                GetComponent<Image>().sprite = sprite_active;
            else if (!isPointerOnButton)
                GetComponent<Image>().sprite = sprite_inactive;
        }
        else
        {
            GetComponent<Image>().sprite = sprite_inactive;
            GetComponent<Image>().color = Color.gray;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isActive)
            return;
        isPointerOnButton = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
            return;
        isPointerOnButton = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActive)
            return;
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
