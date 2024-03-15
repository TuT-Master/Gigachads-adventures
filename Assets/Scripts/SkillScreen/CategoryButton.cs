using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public bool clicked;
    private bool pointerHover;

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

    private SVGImage image;
    private PlayerSkill playerSkill;


    void Start()
    {
        image = GetComponent<SVGImage>();
        playerSkill = FindAnyObjectByType<PlayerSkill>();

        image.sprite = sprite_idle;
    }

    void Update()
    {
        if (clicked | pointerHover)
            image.sprite = sprite_select;
        else
            image.sprite = sprite_idle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = true;
        foreach (CategoryButton button in FindObjectsOfType<CategoryButton>())
            if (button != this)
            {
                button.clicked = false;
                button.OnPointerExit(null);
            }
        switch (buttonType)
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
        pointerHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerHover = false;
    }
}
