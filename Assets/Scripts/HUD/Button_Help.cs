using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Help : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprite_inactive;
    }
    public void ReloadSprite()
    {
        if (image != null)
            image.sprite = sprite_inactive;
    }
    public void OnPointerEnter(PointerEventData eventData) { image.sprite = sprite_active; }
    public void OnPointerExit(PointerEventData eventData) { image.sprite = sprite_inactive; }
    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = sprite_clicked;
        helpScreen.ToggleHelpScreen(true);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
