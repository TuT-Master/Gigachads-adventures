using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardStat : MonoBehaviour
{
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private Transform statEffects;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject bar_fill_0;
    [SerializeField] private GameObject bar_fill_1;

    [Header("Stat images")]
    [SerializeField] private Sprite damage;
    [SerializeField] private Sprite penetration;
    [SerializeField] private Sprite armorIgnore;
    [SerializeField] private Sprite critChance;
    [SerializeField] private Sprite critDamage;


    private float defaultValue;
    private float bonusValue;

    public void SetUp(string stat, float defaultValue, float bonusValue)
    {
        this.defaultValue = defaultValue;

        // Set up name of stat
        switch(stat)
        {
            case "damage":
                statName.text = "Damage";
                statImage.sprite = damage;
                break;
            case "penetration":
                statName.text = "Penetration";
                statImage.sprite = penetration;
                break;
            case "armorIgnore":
                statName.text = "Armor ignore";
                statImage.sprite = armorIgnore;
                break;
            case "critChance":
                statName.text = "Critical chance";
                statImage.sprite = critChance;
                break;
            case "critDamage":
                statName.text = "Critical damage";
                statImage.sprite = critDamage;
                break;
            default:
                Debug.Log("Stat not found! Given parameter: " + stat);
                break;
        }

        // Set up value of stat
        statValue.text = Math.Round(defaultValue + bonusValue, 2).ToString();
        
        UpdateFillBar();
    }

    private void UpdateFillBar()
    {

    }

    public void AddStatEffect()
    {

    }
}
