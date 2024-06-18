using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Help : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    private Image image;

    [SerializeField]
    private HelpScreen helpScreen;

    void Start() { image = GetComponent<Image>(); }
    public void OnPointerEnter(PointerEventData eventData) { image.sprite = sprite_active; }
    public void OnPointerExit(PointerEventData eventData) { image.sprite = sprite_inactive; }
    public void OnPointerClick(PointerEventData eventData)
    {
        image.sprite = sprite_clicked;
        helpScreen.ToggleHelpScreen(true);
    }
}
