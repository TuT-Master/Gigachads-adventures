using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeTab : MonoBehaviour
{
    [SerializeField]
    private SkillDescription skillDescription;

    #region One handed dexterity skills
    Dictionary<string, float> whipStats = new()
    {
        {"bleedingChance", 0},
        {"range", 0},
        {"stunChance", 0}
    };
    Dictionary<string, float> daggerStats = new()
    {
        {"armorIngore", 0},
        {"attackSpeed", 0}
    };
    Dictionary<string, float> swordStats = new()
    {
        {"attackSpeed", 0},
        {"penetration", 0},
        {"bleedChance", 0}
    };
    Dictionary<string, float> rapierStats = new()
    {
        {"critChance", 0},
        {"range", 0}
    };
    Dictionary<string, float> oneHandedDexterityStats = new()
    {
        {"notConsumeStaminaChance", 0},
        {"staminaConsumtionReduction", 0},
        {"evade", 0},
        {"critChance", 0}
    };
    #endregion


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
