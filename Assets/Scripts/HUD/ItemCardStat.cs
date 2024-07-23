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

    private float defaultValue;
    private float bonusValue;

    public void SetUp(string stat, float defaultValue)
    {
        this.defaultValue = defaultValue;

    }

    public void AddBonusValue(float bonusValue)
    {
        this.bonusValue = bonusValue;
        UpdateFillBar();
    }

    private void UpdateFillBar()
    {

    }

    public void AddStatEffect()
    {

    }
}
