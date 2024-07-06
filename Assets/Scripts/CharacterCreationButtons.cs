using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCreationButtons : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private StartScreen.PicType picType;

    [SerializeField]
    private bool next;

    private int index = 0;
    private int indexChange = 0;

    [SerializeField]
    private Sprite sprite_inactive;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;


    void Start()
    {
        if (next)
            indexChange = 1;
        else
            indexChange = -1;
    }

    public void OnPointerEnter(PointerEventData eventData) { GetComponent<Image>().sprite = sprite_active; }
    public void OnPointerExit(PointerEventData eventData) { GetComponent<Image>().sprite = sprite_inactive; }
    public void OnPointerClick(PointerEventData eventData)
    {
        index += indexChange;
        FindAnyObjectByType<StartScreen>().ChangePic(picType, index, out index);
        GetComponent<Image>().sprite = sprite_clicked;
        StartCoroutine(ResetSprite());
    }
    private IEnumerator ResetSprite()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Image>().sprite = sprite_inactive;
    }
}
