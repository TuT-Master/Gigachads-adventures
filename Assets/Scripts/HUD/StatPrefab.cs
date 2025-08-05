using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPrefab : MonoBehaviour
{
    private enum Stat
    {
        Hp,
        Stamina,
        Mana,
        HpMax,
        StaminaMax,
        ManaMax,
        Armor,
        MagicResistance,
        Evade,
        Defense,
        Weight,
        MovementSpeed,
        Knockback,
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
            Stat.Hp => new float[] { playerStats.playerStats["hp"], playerStats.playerBaseStats["hp"], playerStats.playerStats["hpMax"] },
            Stat.Stamina => new float[] { playerStats.playerStats["stamina"], playerStats.playerBaseStats["stamina"], playerStats.playerStats["staminaMax"] },
            Stat.Mana => new float[] { playerStats.playerStats["mana"], playerStats.playerBaseStats["mana"], playerStats.playerStats["manaMax"] },
            _ => new float[] {0f, 0f, 0f},
        };
    }

    private string StatToString()
    {
        return stat switch
        {
            Stat.Hp => "Health",
            Stat.Stamina => "Stamina",
            Stat.Mana => "Mana",
            _ => null,
        };
    }
}
