using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_QuitGame : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    private Image image;


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
    public void OnPointerClick(PointerEventData eventData)
    {
        image.sprite = sprite_clicked;
        StartCoroutine(ActionDelay());
    }

    IEnumerator ActionDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Application.Quit();
    }
}
