using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public bool clicked;
    private bool pointerHover;

    [SerializeField]
    private Sprite sprite_idle;
    [SerializeField]
    private Sprite sprite_select;

    [SerializeField]
    private PlayerCrafting playerCrafting;
    [SerializeField]
    private int screenId;

    private Image image;


    void Start()
    {
        image = GetComponent<Image>();

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
        playerCrafting.OpenTab(screenId);
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
