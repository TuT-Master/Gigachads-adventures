using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hpAmount;
    [SerializeField]
    private TextMeshProUGUI staminaAmount;
    [SerializeField]
    private TextMeshProUGUI manaAmount;

    [SerializeField]
    private PlayerStats playerStats;


    void Update()
    {
        hpAmount.text = Mathf.Round(playerStats.playerStats["hp"]).ToString() + " / " + playerStats.playerStats["hpMax"].ToString();
        staminaAmount.text = Mathf.Round(playerStats.playerStats["stamina"]).ToString() + " / " + playerStats.playerStats["staminaMax"].ToString();
        manaAmount.text = Mathf.Round(playerStats.playerStats["mana"]).ToString() + " / " + playerStats.playerStats["manaMax"].ToString();
    }
}
