using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField]
    private string field;
    [SerializeField]
    private RectTransform fill;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    private void Update()
    {
        if (playerStats.playerStats == null)
            return;
        gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerStats.playerStats[field + "Max"] * 3);
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerStats.playerStats[field] * 3);
    }
}
