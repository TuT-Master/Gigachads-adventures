using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public bool clicked;

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

        clicked = false;
    }

    void Update()
    {
        if (clicked)
            image.sprite = sprite_select;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = true;
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
        foreach (CategoryButton button in FindObjectsOfType<CategoryButton>())
            if (button != this)
            {
                button.clicked = false;
                button.OnPointerExit(null);
            }
        image.sprite = sprite_select;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_idle;
    }
}
