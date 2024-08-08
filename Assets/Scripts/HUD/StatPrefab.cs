using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatPrefab : MonoBehaviour
{
    private enum Stat
    {
        Health,
        Stamina,
        Mana,

    }
    [SerializeField] private Stat stat;
    [SerializeField] private bool isMainStat;

    [Header("No touchey down there")]
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private PlayerStats playerStats;
    private float valueDefault = 0f;
    private float valueCurrent = 0f;

    public void UpdateStat()
    {
        if (playerStats == null || playerStats.playerStats == null)
            return;

        // Stat name
        statName.text = StatToString();

        // Stat value
        valueDefault = (float)System.Math.Round(GetStatDefault(), 2);
        valueCurrent = (float)System.Math.Round(GetStatCurrent(), 2);
        statValue.text = valueCurrent.ToString();
        if (isMainStat)
            statValue.text += " / " + System.Math.Round(GetStatMax(), 2).ToString();
        else
        {
            if (valueDefault < valueCurrent)
                statValue.text += " + " + (valueCurrent - valueDefault).ToString();
            else if (valueDefault > valueCurrent)
                statValue.text += " - " + (valueDefault - valueCurrent).ToString();
        }
    }

    private float GetStatCurrent()
    {
        return stat switch
        {
            Stat.Health => playerStats.playerStats["hp"],
            Stat.Stamina => playerStats.playerStats["stamina"],
            Stat.Mana => playerStats.playerStats["mana"],
            _ => 0f,
        };
    }
    private float GetStatMax()
    {
        return stat switch
        {
            Stat.Health => playerStats.playerStats["hpMax"],
            Stat.Stamina => playerStats.playerStats["staminaMax"],
            Stat.Mana => playerStats.playerStats["manaMax"],
            _ => 0f,
        };
    }
    private float GetStatDefault()
    {
        return stat switch
        {
            Stat.Health => playerStats.playerStats_default["hp"],
            Stat.Stamina => playerStats.playerStats_default["stamina"],
            Stat.Mana => playerStats.playerStats_default["mana"],
            _ => 0f,
        };
    }

    private string StatToString()
    {
        return stat switch
        {
            Stat.Health => "Health",
            Stat.Stamina => "Stamina",
            Stat.Mana => "Mana",
            _ => null,
        };
    }
}
