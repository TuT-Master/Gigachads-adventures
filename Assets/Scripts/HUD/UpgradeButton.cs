using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private PlayerCrafting playerCrafting;

    [SerializeField]
    private Sprite sprite_default;
    [SerializeField]
    private Sprite sprite_active;
    [SerializeField]
    private Sprite sprite_clicked;

    [HideInInspector]
    public bool isActive = true;

    private Image buttonImage;


    void Start()
    {
        buttonImage = GetComponent<Image>();
    }
    void Update()
    {
        if (isActive)
            buttonImage.color = Color.white;
        else
            buttonImage.color = Color.gray;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
            return;
        buttonImage.sprite = sprite_active;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
            return;
        buttonImage.sprite = sprite_default;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActive)
            return;
        buttonImage.sprite = sprite_clicked;
        StartCoroutine(ResetSprite());
        playerCrafting.UpgradeItem();
    }
    private IEnumerator ResetSprite()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        buttonImage.sprite = sprite_default;
    }
}