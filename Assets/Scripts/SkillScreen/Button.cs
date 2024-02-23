using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private enum ButtonType
    {
        Stat,
        Melee,
        Ranged,
        Magic,
    }
    [SerializeField]
    private ButtonType buttonType;

    [SerializeField]
    private Sprite sprite_idle;
    [SerializeField]
    private Sprite sprite_select;

    private Image image;
    private PlayerSkill playerSkill;


    void Start()
    {
        image = GetComponent<Image>();
        playerSkill = FindAnyObjectByType<PlayerSkill>();

        image.sprite = sprite_idle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(buttonType)
        {
            case ButtonType.Melee:
                playerSkill.MeleeButtonClicked();
                break;
            case ButtonType.Magic:
                playerSkill.MagicButtonClicked();
                break;
            case ButtonType.Ranged:
                playerSkill.RangedButtonClicked();
                break;
            case ButtonType.Stat:
                playerSkill.StatButtonClicked();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = sprite_select;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_idle;
    }
}
