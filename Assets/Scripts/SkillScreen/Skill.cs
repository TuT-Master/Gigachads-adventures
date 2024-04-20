using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;

    public string description;

    public bool activeSkill;

    // Levels of skill
    public int levelOfSkill = 0;
    [HideInInspector] public int MaxlevelsOfSkill = 1;
    [HideInInspector] public bool maxLevel;

    public Dictionary<string, float> bonusStats = new();
    #region Bonus stats
    [Header("Bonus stats")]
    [SerializeField]
    float[] damage;
    [SerializeField]
    float[] accuracyBonus;
    [SerializeField]
    float[] penetration;
    [SerializeField]
    float[] armorIgnore;
    [SerializeField]
    float[] bleedingChance;
    [SerializeField]
    float[] bleedingDamage;
    [SerializeField]
    float[] poisonChance;
    [SerializeField]
    float[] poisonDamage;
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
    private Image skillUnlockedAmountImage;
    [SerializeField]
    private Sprite skillLocked;
    [SerializeField]
    private Sprite skillUnlocked;

    // Other variables for images
    private float fillAmount = 0f;

    private SkillDescription skillDescription;



    private void Start()
    {
        transform.Find("ImageLocked").GetComponent<Image>().sprite = skillLocked;
        skillUnlockedAmountImage.sprite = skillUnlocked;
        skillUnlockedAmountImage.type = Image.Type.Filled;
        skillUnlockedAmountImage.fillMethod = Image.FillMethod.Vertical;
        skillUnlockedAmountImage.fillAmount = 0f;
        skillDescription = FindAnyObjectByType<SkillDescription>();
        List<float[]> floats = new()
        {
            damage,
            penetration,
            armorIgnore,
            bleedingChance,
            bleedingDamage,
            poisonChance,
            poisonDamage,
            stunChance,
            range,
            attackSpeed,
            critChance,
            notConsumeStaminaChance,
            staminaConsumtionReduction,
            evade,
        };
        foreach (float[] f in floats)
            if (f.Length > MaxlevelsOfSkill)
                MaxlevelsOfSkill = f.Length;
        bonusStats = new()
        {
            {"damage", 0 },
            {"accuracyBonus", 0 },
            {"penetration", 0 },
            {"armorIngore", 0 },
            {"bleedingChance", 0 },
            {"bleedingDamage", 0 },
            {"poisonChance", 0 },
            {"poisonDamage", 0 },
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


            Dictionary<string, float[]> newBonusStats = new()
            {
                {"damage", damage },
                {"accuracyBonus", accuracyBonus },
                {"penetration", penetration },
                {"armorIngore", armorIgnore },
                {"bleedingChance", bleedingChance },
                {"bleedingDamage", bleedingDamage },
                {"poisonChance", poisonChance },
                {"poisonDamage", poisonDamage },
                {"stunChance", stunChance },
                {"range", range },
                {"attackSpeed", attackSpeed },
                {"critChance", critChance },
                {"notConsumeStaminaChance", notConsumeStaminaChance },
                {"staminaConsumtionReduction", staminaConsumtionReduction },
                {"evade", evade },
            };

            foreach (string key in  newBonusStats.Keys)
            {
                if (newBonusStats[key].Length > 0)
                {
                    if (bonusStats.ContainsKey(key))
                        bonusStats[key] = newBonusStats[key][levelOfSkill - 1];
                    else
                        bonusStats.Add(key, newBonusStats[key][levelOfSkill - 1]);
                }
            }

            // Fill
            fillAmount = levelOfSkill / MaxlevelsOfSkill;
            Debug.Log(fillAmount);
            skillUnlockedAmountImage.fillAmount = fillAmount;

            if (levelOfSkill == MaxlevelsOfSkill)
                maxLevel = true;
        }
        else
            Debug.Log("Skill is at max level!");
    }


    public void OnPointerDown()
    {
        skillDescription.skill = this;
        // TODO - Zvýraznit vybraný skill
    }
    public void OnPointerExit()
    {
        skillDescription.HideSkillDetails();
    }
    public void OnPointerEnter() { skillDescription.ShowSkillDetails(this); }
}
