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
        textL += "Health: " + System.Math.Round(playerStats.playerStats["hp"], 2).ToString() + " / " + playerStats.playerStats["hpMax"].ToString() + "\n";
        textL += "Stamina: " + System.Math.Round(playerStats.playerStats["stamina"], 2).ToString() + " / " + playerStats.playerStats["staminaMax"].ToString() + "\n";
        textL += "Mana: " + System.Math.Round(playerStats.playerStats["mana"], 2).ToString() + " / " + playerStats.playerStats["manaMax"].ToString() + "\n";
        textL += "\nExperience: " + System.Math.Round(playerStats.playerStats["exp_player"], 2).ToString() + "\n";
        textL += "Level: " + System.Math.Round(playerStats.playerStats["level_player"], 2).ToString() + "\n";
        textL += "\nOne handed dexterity exp: " + System.Math.Round(playerStats.playerStats["exp_oneHandDexterity"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_oneHandDexterity"] * 5)) + " (lvl:" + playerStats.playerStats["level_oneHandDexterity"] + ")" + "\n";
        textL += "One handed strenght exp: " + System.Math.Round(playerStats.playerStats["exp_oneHandStrenght"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_oneHandStrenght"] * 5)) + " (lvl:" + playerStats.playerStats["level_oneHandStrenght"] + ")" + "\n";
        textL += "Two handed dexterity exp: " + System.Math.Round(playerStats.playerStats["exp_twoHandDexterity"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_twoHandDexterity"] * 5)) + " (lvl:" + playerStats.playerStats["level_twoHandDexterity"] + ")" + "\n";
        textL += "Two handed strenght exp: " + System.Math.Round(playerStats.playerStats["exp_twoHandStrenght"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_twoHandStrenght"] * 5)) + " (lvl:" + playerStats.playerStats["level_twoHandStrenght"] + ")" + "\n";
        textL += "Ranged dexterity exp: " + System.Math.Round(playerStats.playerStats["exp_rangedDexterity"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_rangedDexterity"] * 5)) + " (lvl:" + playerStats.playerStats["level_rangedDexterity"] + ")" + "\n";
        textL += "Ranged strenght exp: " + System.Math.Round(playerStats.playerStats["exp_rangedStrenght"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_rangedStrenght"] * 5)) + " (lvl:" + playerStats.playerStats["level_rangedStrenght"] + ")" + "\n";
        textL += "Magic Fire exp: " + System.Math.Round(playerStats.playerStats["exp_magicFire"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_magicFire"] * 5)) + " (lvl:" + playerStats.playerStats["level_magicFire"] + ")" + "\n";
        textL += "Magic Water exp: " + System.Math.Round(playerStats.playerStats["exp_magicWater"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_magicWater"] * 5)) + " (lvl:" + playerStats.playerStats["level_magicWater"] + ")" + "\n";
        textL += "Magic Earth exp: " + System.Math.Round(playerStats.playerStats["exp_magicEarth"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_magicEarth"] * 5)) + " (lvl:" + playerStats.playerStats["level_magicEarth"] + ")" + "\n";
        textL += "Magic Air exp: " + System.Math.Round(playerStats.playerStats["exp_magicAir"], 2).ToString() + "/" + (20 + (playerStats.playerStats["level_magicAir"] * 5)) + " (lvl:" + playerStats.playerStats["level_magicAir"] + ")" + "\n";

        // Right text field
        textR += "Weight: " + System.Math.Round(playerStats.playerStats["weight"] - 80, 2).ToString() + " Kg\n";
        textR += "Armor: " + playerStats.playerStats["armor"].ToString() + " \n";
        textR += "Defense: " + playerStats.playerStats["defense"].ToString() + "\n";

        // Apply texts
        textField_left.text = textL;
        textField_right.text = textR;
    }
}
