using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCardStatEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionCard;
    [SerializeField] private TextMeshProUGUI effectName;
    [SerializeField] private TextMeshProUGUI effectDescription;
    private bool pointerOn;

    public void SetUp(ItemCard.StatEffect statEffect, float value, Sprite gfx)
    {
        pointerOn = false;
        GetComponent<Image>().sprite = gfx;
        switch(statEffect)
        {
            case ItemCard.StatEffect.Poison:
                effectName.text = "Poison";
                effectDescription.text = "Each hit with this weapon stacks up " + value.ToString() + " poison to enemy.";
                break;
            case ItemCard.StatEffect.Bleeding:
                effectName.text = "Bleeding";
                effectDescription.text = "Each hit with this weapon stacks up " + value.ToString() + " bleeding to enemy.";
                break;
            case ItemCard.StatEffect.BurningChance:
                effectName.text = "Burning";
                effectDescription.text = "Each hit has a chance to set enemy on fire.";
                break;
            case ItemCard.StatEffect.Homing:
                effectName.text = "Homing";
                effectDescription.text = "This projectile is self-homing.";
                break;
            case ItemCard.StatEffect.FullSetBonus:
                effectName.text = "Full set bonus";
                effectDescription.text = "";
                break;
            case ItemCard.StatEffect.AoE:
                effectName.text = "AoE";
                effectDescription.text = "Deals damage to all enemies close to the point of collision.";
                break;
            case ItemCard.StatEffect.Piercing:
                effectName.text = "Piercing";
                effectDescription.text = "Projectile pierces through up " + value.ToString() + " enemies.";
                break;
            case ItemCard.StatEffect.FireMagic:
                effectName.text = "Fire magic";
                effectDescription.text = "This weapon is using fire magic";
                break;
            case ItemCard.StatEffect.WaterMagic:
                effectName.text = "Water magic";
                effectDescription.text = "This weapon is using water magic";
                break;
            case ItemCard.StatEffect.EarthMagic:
                effectName.text = "Earth magic";
                effectDescription.text = "This weapon is using earth magic";
                break;
            case ItemCard.StatEffect.AirMagic:
                effectName.text = "Air magic";
                effectDescription.text = "This weapon is using air magic";
                break;
            case ItemCard.StatEffect.LightMagic:
                effectName.text = "Light magic";
                effectDescription.text = "This weapon is using light magic";
                break;
            case ItemCard.StatEffect.DarkMagic:
                effectName.text = "Dark magic";
                effectDescription.text = "This weapon is using dark magic";
                break;
            case ItemCard.StatEffect.BleedingResistance:
                effectName.text = "Bleeding resistance";
                effectDescription.text = "This item increases your resistance to bleeding by " + value.ToString();
                break;
            case ItemCard.StatEffect.PoisonResistance:
                effectName.text = "Poison resistance";
                effectDescription.text = "This item increases your resistance to poison by " + value.ToString();
                break;
            default:
                effectName.text = "Tohle nemám";
                effectDescription.text = "A tohle samozøejmì taky ne";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOn = true;
        StartCoroutine(ShowDescriptionCard());
    }
    private IEnumerator ShowDescriptionCard()
    {
        if(!pointerOn)
            yield return null;
        yield return new WaitForSecondsRealtime(0.75f);
        if(pointerOn)
            descriptionCard.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOn = false;
        StopAllCoroutines();
        descriptionCard.SetActive(false);
    }
}
