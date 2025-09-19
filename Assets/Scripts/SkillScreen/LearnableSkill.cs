using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PlayerStats;

public class LearnableSkill : MonoBehaviour
{
    public string skillName;

    public string description;

    public List<int> levelUnlock;

    public bool activeSkill;

    public PlayerStats.WeaponClass weaponClass;

    public Item.ItemType weaponType;

    // Levels of skill
    [HideInInspector] public float levelOfSkill = 0f;
    [HideInInspector] public float MaxlevelsOfSkill = 1f;
    [HideInInspector] public bool maxLevel;

    public Dictionary<StatType, float> bonusStats = new();
    #region Bonus Passive Stats
    [Header("Bonus passive stats")]
    [SerializeField] float[] damage;
    [SerializeField] float[] accuracyBonus;
    [SerializeField] float[] penetration;
    [SerializeField] float[] armorIgnore;
    [SerializeField] float[] bleedingChance;
    [SerializeField] float[] bleedingDamage;
    [SerializeField] float[] poisonChance;
    [SerializeField] float[] poisonDamage;
    [SerializeField] float[] stunChance;
    [SerializeField] float[] range;
    [SerializeField] float[] attackSpeed;
    [SerializeField] float[] critChance;
    [SerializeField] float[] knockback;
    [SerializeField] float[] increaseArmorByPercentage;
    [SerializeField] float[] notConsumeStaminaChance;
    [SerializeField] float[] staminaConsumtionReduction;
    [SerializeField] float[] evade;
    #endregion


    [Header("GFX")]
    [SerializeField] private Image skillUnlockedAmountImage;
    [SerializeField] private Sprite skillLocked;
    [SerializeField] private Sprite skillUnlocked;
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
            {StatType.Damage, 0 },
            {StatType.AccuracyBonus, 0 },
            {StatType.Penetration, 0 },
            {StatType.ArmorIgnore, 0 },
            {StatType.BleedingChance, 0 },
            {StatType.BleedingDamage, 0 },
            {StatType.PoisonChance, 0 },
            {StatType.PoisonDamage, 0 },
            {StatType.StunChance, 0 },
            {StatType.Range, 0 },
            {StatType.AttackSpeed, 0 },
            {StatType.CritChance, 0 },
            {StatType.Knockback, 0 },
            {StatType.IncreaseArmorByPercentage, 0 },
            {StatType.NotConsumeStaminaChance, 0 },
            {StatType.StaminaConsumptionReduction, 0 },
            {StatType.Evade, 0 },
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
            Dictionary<StatType, float[]> skillBonus = new()
            {
                {StatType.Damage, damage },
                {StatType.AccuracyBonus, accuracyBonus },
                {StatType.Penetration, penetration },
                {StatType.ArmorIgnore, armorIgnore },
                {StatType.BleedingChance, bleedingChance },
                {StatType.BleedingDamage, bleedingDamage },
                {StatType.PoisonChance, poisonChance },
                {StatType.PoisonDamage, poisonDamage },
                {StatType.StunChance, stunChance },
                {StatType.Range, range },
                {StatType.AttackSpeed, attackSpeed },
                {StatType.CritChance, critChance },
                {StatType.Knockback, knockback },
                {StatType.IncreaseArmorByPercentage, increaseArmorByPercentage },
                {StatType.NotConsumeStaminaChance, notConsumeStaminaChance },
                {StatType.StaminaConsumptionReduction, staminaConsumtionReduction },
                {StatType.Evade, evade },
            };
            foreach (StatType statType in  skillBonus.Keys)
            {
                if (skillBonus[statType].Length > 0)
                {
                    if (bonusStats.ContainsKey(statType))
                        bonusStats[statType] = skillBonus[statType][(int)levelOfSkill - 1];
                    else
                        bonusStats.Add(statType, skillBonus[statType][(int)levelOfSkill - 1]);
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
