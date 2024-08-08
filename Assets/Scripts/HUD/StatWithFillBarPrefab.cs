using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatWithFillBarPrefab : MonoBehaviour
{
    private enum SkillClass
    {
        OneHandedDexterity,
        OneHandedStrenght,
        TwoHandedDexterity,
        TwoHandedStrenght,
        RangedDexterity,
        RangedStrenght,
        Magic
    }
    [SerializeField] private SkillClass skillClass;
    [SerializeField] private Sprite fillBar_background;
    [SerializeField] private Sprite fillBar_fill;

    [Header("No touchey down there")]
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private PlayerStats playerStats;
    private float valueCurrent = 0f;
    private float valueMax = 0f;

    public void UpdateStat()
    {
        if (playerStats == null || playerStats.playerStats == null)
            return;

        // Skill class name
        statName.text = StatToString();

        // Skill value


        // Update fillbar

    }
    private string StatToString()
    {
        return skillClass switch
        {
            SkillClass.OneHandedDexterity => "One handed dexterity",
            SkillClass.OneHandedStrenght => "One handed strenght",
            SkillClass.TwoHandedDexterity => "Two handed dexterity",
            SkillClass.TwoHandedStrenght => "Two handed strenght",
            SkillClass.RangedDexterity => "Ranged dexterity",
            SkillClass.RangedStrenght=> "Ranged strenght",
            SkillClass.Magic => "Magic",
            _ => null,
        };
    }
}
