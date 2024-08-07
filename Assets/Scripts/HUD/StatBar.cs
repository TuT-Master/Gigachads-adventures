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
    [SerializeField]
    private Image fillEnd;

    private PlayerStats playerStats;

    private void Start() { playerStats = FindAnyObjectByType<PlayerStats>(); }
    private void Update()
    {
        if (playerStats.playerStats == null)
            return;
        gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerStats.playerStats[field + "Max"] * 3);
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerStats.playerStats[field] * 3);
        float statRatio = playerStats.playerStats[field] / playerStats.playerStats[field + "Max"];
        if (statRatio <= 0.2f)
        {
            fill.GetComponent<Image>().color = new Color(1, 1, 1, statRatio / 0.2f);
            fillEnd.color = new Color(1, 1, 1, statRatio / 0.2f);
        }
        else
        {
            fill.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            fillEnd.color = new Color(1, 1, 1, 1);
        }
    }
}
