using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpScreen_arrowButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField]
    private bool nextImage;

    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    [SerializeField]
    private HelpScreen helpScreen;

    void Start() { GetComponent<Image>().sprite = sprite_inactive; }
    public void OnPointerEnter(PointerEventData eventData) { GetComponent<Image>().sprite = sprite_active; }
    public void OnPointerExit(PointerEventData eventData) { GetComponent<Image>().sprite = sprite_inactive; }
    public void OnPointerUp(PointerEventData eventData) { GetComponent<Image>().sprite = sprite_inactive; }
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = sprite_clicked;
        helpScreen.SlideImage(nextImage);
    }
}
