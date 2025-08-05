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
            case ItemCard.StatEffect.AoE:
                effectName.text = "AoE";
                effectDescription.text = "Deals damage to all enemies close to the point of collision.";
                break;
            case ItemCard.StatEffect.Piercing:
                effectName.text = "Piercing";
                effectDescription.text = "Projectile pierces through up " + value.ToString() + " enemies.";
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
                effectName.text = "Tohle nem�m";
                effectDescription.text = "A tohle samoz�ejm� taky ne";
                break;
        }
    }
    public void SetUp(Dictionary<string, float> values, Sprite gfx)
    {
        pointerOn = false;
        GetComponent<Image>().sprite = gfx;
        effectName.text = "Full-set bonus";
        effectDescription.text = "";
        foreach (string val in values.Keys)
        {
            switch (val)
            {
                case "hpMax":
                    effectDescription.text += "Max health ";
                    if (values[val] > 0)
                        effectDescription.text += "+" + values[val] + "\n";
                    else
                        effectDescription.text += values[val] + "\n";
                    break;
                case "staminaMax":
                    effectDescription.text += "Max stamina ";
                    if (values[val] > 0)
                        effectDescription.text += "+" + values[val] + "\n";
                    else
                        effectDescription.text += values[val] + "\n";
                    break;
                case "manaMax":
                    effectDescription.text += "Max mana ";
                    if (values[val] > 0)
                        effectDescription.text += "+" + values[val] + "\n";
                    else
                        effectDescription.text += values[val] + "\n";
                    break;
                default:
                    break;
            }
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
        yield return new WaitForSeconds(0.75f);
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
