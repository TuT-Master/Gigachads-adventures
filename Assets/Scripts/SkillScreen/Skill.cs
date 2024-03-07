using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;

    // Levels of skill
    public int levelOfSkill = 0;
    public int MaxlevelsOfSkill = 0;
    public bool maxLevel;
    public bool clicked;

    #region Bonus stats
    public Dictionary<string, float> bonusStats;
    [Header("Bonus stats")]
    [SerializeField]
    float[] damage;
    [SerializeField]
    float[] penetration;
    [SerializeField]
    float[] armorIgnore;
    [SerializeField]
    float[] bleedingChance;
    [SerializeField]
    float[] stunChance;
    [SerializeField]
    float[] range;
    [SerializeField]
    float[] attackSpeed;
    [SerializeField]
    float[] critChance;
    [SerializeField]
    float[] notConsumeStaminaChance;
    [SerializeField]
    float[] staminaConsumtionReduction;
    [SerializeField]
    float[] evade;
    #endregion


    // Images for skill
    [SerializeField]
    private SVGImage skillLocked;
    [SerializeField]
    private SVGImage skillUnlocked;

    private SkillDescription skillDescription;


    private void Start()
    {
        skillDescription = transform.parent.parent.Find("SkillDescription").GetComponent<SkillDescription>();
        bonusStats = new()
        {
            {"damage", 0 },
            {"penetration", 0 },
            {"armorIngore", 0 },
            {"bleedingChance", 0 },
            {"stunChance", 0 },
            {"range", 0 },
            {"attackSpeed", 0 },
            {"critChance", 0 },
            {"notConsumeStaminaChance", 0 },
            {"staminaConsumtionReduction", 0 },
            {"evade", 0 },
        };
    }

    public void UpgradeSkill()
    {
        if (levelOfSkill < MaxlevelsOfSkill)
        {
            levelOfSkill++;

            bonusStats = new()
            {
                {"damage", damage[levelOfSkill - 1] },
                {"penetration", penetration[levelOfSkill - 1] },
                {"armorIngore", armorIgnore[levelOfSkill - 1] },
                {"bleedingChance", bleedingChance[levelOfSkill - 1] },
                {"stunChance", stunChance[levelOfSkill - 1] },
                {"range", range[levelOfSkill - 1] },
                {"attackSpeed", attackSpeed[levelOfSkill - 1] },
                {"critChance", critChance[levelOfSkill - 1] },
                {"notConsumeStaminaChance", notConsumeStaminaChance[levelOfSkill - 1] },
                {"staminaConsumtionReduction", staminaConsumtionReduction[levelOfSkill - 1] },
                {"evade", evade[levelOfSkill - 1] },
            };

            // TODO - change sprites

            if (levelOfSkill == MaxlevelsOfSkill)
                maxLevel = true;
        }
        else
            Debug.Log("Skill is at its max level!");
    }


    public void OnPointerDown()
    {
        clicked = true;
        // TODO - Zvýraznit vybraný skill
    }
    public void OnPointerExit()
    {
        if (!clicked)
            skillDescription.HideSkillDetails();
    }
    public void OnPointerEnter() { skillDescription.ShowSkillDetails(this); }
}
