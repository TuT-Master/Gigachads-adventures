using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField_left;
    [SerializeField]
    private TextMeshProUGUI textField_right;

    [SerializeField]
    private PlayerStats playerStats;


    void Update()
    {
        string textL = "";
        string textR = "";

        // Left text field
        textL += "Health: " + Mathf.Round(playerStats.playerStats["hp"]).ToString() + " / " + playerStats.playerStats["hpMax"].ToString() + "\n";
        textL += "Stamina: " + Mathf.Round(playerStats.playerStats["stamina"]).ToString() + " / " + playerStats.playerStats["staminaMax"].ToString() + "\n";
        textL += "Mana: " + Mathf.Round(playerStats.playerStats["mana"]).ToString() + " / " + playerStats.playerStats["manaMax"].ToString() + "\n";

        // Right text field
        textR += "Weight: " + playerStats.playerStats["weight"].ToString() + " Kg\n";

        // Apply texts
        textField_left.text = textL;
        textField_right.text = textR;
    }
}
