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

    public List<int> levelUnlock;

    public bool activeSkill;

    public PlayerStats.WeaponClass weaponClass;

    public Item.WeaponType weaponType;

    // Levels of skill
    [HideInInspector] public float levelOfSkill = 0f;
    [HideInInspector] public float MaxlevelsOfSkill = 1f;
    [HideInInspector] public bool maxLevel;

    public Dictionary<string, float> bonusStats = new();
    #region Bonus Passive Stats
    [Header("Bonus passive stats")]
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
    float[] knockback;
    [SerializeField]
    float[] increaseArmorByPercentage;
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
    private GameObject highlight;

    // Other variables for images
    private float fillAmount = 0f;

    private SkillDescription skillDescription;



    private void Start()
    {
        highlight = transform.GetChild(0).gameObject;
        highlight.SetActive(false);
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
            knockback,
            increaseArmorByPercentage,
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
            {"knockback", 0 },
            {"increaseArmorByPercentage", 0 },
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
            if (levelOfSkill == MaxlevelsOfSkill)
                maxLevel = true;

            // Upgrade skill bonuses
            Dictionary<string, float[]> skillBonus = new()
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
                {"knockback", knockback },
                {"increaseArmorByPercentage", increaseArmorByPercentage },
                {"notConsumeStaminaChance", notConsumeStaminaChance },
                {"staminaConsumtionReduction", staminaConsumtionReduction },
                {"evade", evade },
            };
            foreach (string key in  skillBonus.Keys)
            {
                if (skillBonus[key].Length > 0)
                {
                    if (bonusStats.ContainsKey(key))
                        bonusStats[key] = skillBonus[key][(int)levelOfSkill - 1];
                    else
                        bonusStats.Add(key, skillBonus[key][(int)levelOfSkill - 1]);
                }
            }

            // Update skill level
            if (activeSkill)
                FindAnyObjectByType<PlayerSkill>().playerWeaponTypeSkillLevels[weaponType][PlayerSkill.SkillType.Active]++;
            else
                FindAnyObjectByType<PlayerSkill>().playerWeaponTypeSkillLevels[weaponType][PlayerSkill.SkillType.Passive]++;

            // Fill
            fillAmount = levelOfSkill / MaxlevelsOfSkill;
            skillUnlockedAmountImage.fillAmount = fillAmount;
        }
    }


    public void OnPointerDown()
    {
        skillDescription.skill = this;
    }
    public void OnPointerExit()
    {
        skillDescription.HideSkillDetails();
        highlight.SetActive(false);
    }
    public void OnPointerEnter()
    {
        skillDescription.ShowSkillDetails(this);
        highlight.SetActive(true);
    }
}
