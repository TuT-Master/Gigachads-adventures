using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPrefab : MonoBehaviour
{
    [SerializeField] private PlayerStats.StatType stat;
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
        float[] stats = GetStats();
        valueDefault = (float)System.Math.Round(stats[1], 2);
        valueCurrent = (float)System.Math.Round(stats[0], 2);
        statValue.text = valueCurrent.ToString();
        if (isMainStat)
            statValue.text += " / " + System.Math.Round(stats[2], 2).ToString();
        else
        {
            if (valueDefault < valueCurrent)
                statValue.text += " + " + (valueCurrent - valueDefault).ToString();
            else if (valueDefault > valueCurrent)
                statValue.text += " - " + (valueDefault - valueCurrent).ToString();
        }
    }

    private float[] GetStats()
    {
        return stat switch
        {
            PlayerStats.StatType.Hp => new float[] { playerStats.playerStats[PlayerStats.StatType.Hp], playerStats.playerBaseStats[PlayerStats.StatType.Hp], playerStats.playerStats[PlayerStats.StatType.HpMax] },
            PlayerStats.StatType.Stamina => new float[] { playerStats.playerStats[PlayerStats.StatType.Stamina], playerStats.playerBaseStats[PlayerStats.StatType.Stamina], playerStats.playerStats[PlayerStats.StatType.StaminaMax] },
            PlayerStats.StatType.Mana => new float[] { playerStats.playerStats[PlayerStats.StatType.Mana], playerStats.playerBaseStats[PlayerStats.StatType.Mana], playerStats.playerStats[PlayerStats.StatType.ManaMax] },
            _ => new float[] {0f, 0f, 0f},
        };
    }

    private string StatToString()
    {
        return stat switch
        {
            PlayerStats.StatType.Hp => "Health",
            PlayerStats.StatType.Stamina => "Stamina",
            PlayerStats.StatType.Mana => "Mana",
            _ => null,
        };
    }
}
