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
        textL += "\nExperience: " + Mathf.Round(playerStats.playerStats["exp_player"]).ToString() + "\n";
        textL += "\nLevel: " + Mathf.Round(playerStats.playerStats["level_player"]).ToString() + "\n";
        textL += "\nOne handed dexterity exp: " + Mathf.Round(playerStats.playerStats["exp_oneHandDexterity"]).ToString() + "\n";
        textL += "One handed strenght exp: " + Mathf.Round(playerStats.playerStats["exp_oneHandStrenght"]).ToString() + "\n";
        textL += "Two handed dexterity exp: " + Mathf.Round(playerStats.playerStats["exp_twoHandDexterity"]).ToString() + "\n";
        textL += "Two handed strenght exp: " + Mathf.Round(playerStats.playerStats["exp_twoHandStrenght"]).ToString() + "\n";
        textL += "Ranged dexterity exp: " + Mathf.Round(playerStats.playerStats["exp_rangedDexterity"]).ToString() + "\n";
        textL += "Ranged strenght exp: " + Mathf.Round(playerStats.playerStats["exp_rangedStrenght"]).ToString() + "\n";
        textL += "Magic Fire exp: " + Mathf.Round(playerStats.playerStats["exp_magicFire"]).ToString() + "\n";
        textL += "Magic Water exp: " + Mathf.Round(playerStats.playerStats["exp_magicWater"]).ToString() + "\n";
        textL += "Magic Earth exp: " + Mathf.Round(playerStats.playerStats["exp_magicEarth"]).ToString() + "\n";
        textL += "Magic Air exp: " + Mathf.Round(playerStats.playerStats["exp_magicAir"]).ToString() + "\n";

        // Right text field
        textR += "Weight: " + System.Math.Round(playerStats.playerStats["weight"] - 80, 2).ToString() + " Kg\n";
        textR += "Armor: " + playerStats.playerStats["armor"].ToString() + " \n";
        textR += "Defense: " + playerStats.playerStats["defense"].ToString() + "\n";

        // Apply texts
        textField_left.text = textL;
        textField_right.text = textR;
    }
}
