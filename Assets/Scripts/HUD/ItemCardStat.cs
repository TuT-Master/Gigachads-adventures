using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Item;
using static PlayerStats;

public class ItemCardStat : MonoBehaviour
{
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private Transform statEffects;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject fillBar_main;
    [SerializeField] private GameObject fillBar_bonus;

    [HideInInspector] public int age;

    [Header("Fillbar gfx")]
    [SerializeField] private Sprite fillBarMain;
    [SerializeField] private Sprite fillBarBonus_plus;
    [SerializeField] private Sprite fillBarBonus_minus;
    [SerializeField] private Sprite fillBarBackground;
    [SerializeField] private Image fillBarMain_image;
    [SerializeField] private Image fillBarBonus_image;


    [Header("Stat images")]
    [SerializeField] private Sprite damage;
    [SerializeField] private Sprite penetration;
    [SerializeField] private Sprite armorIgnore;
    [SerializeField] private Sprite critChance;
    [SerializeField] private Sprite critDamage;
    [SerializeField] private Sprite magazineSize;
    [SerializeField] private Sprite attackSpeed;
    [SerializeField] private Sprite reloadTime;
    [SerializeField] private Sprite defense;
    [SerializeField] private Sprite additionalSlots;
    [SerializeField] private Sprite armor;
    [SerializeField] private Sprite magicResistance;

    [Header("Stat effects")]
    [SerializeField] private GameObject statEffectPrefab;
    [SerializeField] List<ItemCard.StatEffect> _statEffect;
    [SerializeField] List<Sprite> _statEffectSprites;
    private Dictionary<ItemCard.StatEffect, Sprite> statEffect_Sprite_pairs;


    // Each stat has its own fillbar max values per age
    private readonly Dictionary<StatType, List<float>> fillBarMaxValues = new()
    {
        {StatType.Damage, new List<float>(){ 20, 40, 60, 80, 100 } },
        {StatType.Penetration, new List<float>(){ 10, 20, 30, 40, 50 } },
        {StatType.ArmorIgnore, new List<float>(){ 1, 1, 1, 1, 1 } },
        {StatType.CritChance, new List<float>(){ 1, 1, 1, 1, 1 } },
        {StatType.CritDamage, new List<float>(){ 3, 3, 3, 3, 3 } },
        {StatType.MagazineSize, new List<float>(){ 15, 20, 40, 100, 500 } },
        {StatType.AttackSpeed, new List<float>(){ 5, 10, 15, 20, 40 } },
        {StatType.ReloadTime, new List<float>(){ 5, 5, 5, 5, 5 } },
        {StatType.BonusDefense, new List<float>(){ 100, 100, 100, 100, 100 } },
        {StatType.AdditionalInventorySlots, new List<float>(){ 20, 20, 20, 20, 20 } },
        {StatType.Armor, new List<float>(){ 15, 30, 45, 60, 75 } },
        {StatType.MagicResistance, new List<float>(){ 15, 30, 45, 60, 75 } },
    };
    /*
    ammo
    effect (equipables)
    */


    private Dictionary<StatType, (string displayName, Sprite sprite)> statDisplayData;
    private void Awake()
    {
        statDisplayData = new Dictionary<StatType, (string, Sprite)>
        {
            { StatType.Damage, ("Damage", damage) },
            { StatType.Penetration, ("Penetration", penetration) },
            { StatType.ArmorIgnore, ("Armor ignore", armorIgnore) },
            { StatType.CritChance, ("Critical chance", critChance) },
            { StatType.CritDamage, ("Critical damage", critDamage) },
            { StatType.MagazineSize, ("Magazine size", magazineSize) },
            { StatType.AttackSpeed, ("Attack speed", attackSpeed) },
            { StatType.ReloadTime, ("Reload time", reloadTime) },
            { StatType.BonusDefense, ("Defense", defense) },
            { StatType.AdditionalInventorySlots, ("Additional slots", additionalSlots) },
            { StatType.Armor, ("Armor", armor) },
            { StatType.MagicResistance, ("Magic resistance", magicResistance) },
        };
    }



    public void SetUp(StatType statType, float defaultValue, float bonusValue)
    {
        statEffect_Sprite_pairs = new();
        for(int i = 0; i < _statEffect.Count; i++)
            statEffect_Sprite_pairs.Add(_statEffect[i], _statEffectSprites[i]);

        // Set up name of stat
        if (statDisplayData.TryGetValue(statType, out var displayData))
        {
            statName.text = displayData.displayName;
            statImage.sprite = displayData.sprite;
        }
        else
        {
            statName.text = $"Stat '{statType}' not found!";
            statImage.sprite = null;
        }

        // Set up values of stat
        static string Format(float value, int decimals = 2) => Math.Round(value, decimals).ToString();

        if (statType == StatType.ArmorIgnore)
            statValue.text = $"{Format((defaultValue + bonusValue) * 100)}%";
        else if (statType == StatType.AttackSpeed)
            statValue.text = $"{Format(defaultValue + bonusValue)} / s";
        else if (statType == StatType.ReloadTime)
            statValue.text = $"{Format(defaultValue + bonusValue)} s";
        else if (statType == StatType.AdditionalInventorySlots)
            statValue.text = $"+ {Format(defaultValue + bonusValue)}";
        else
            statValue.text = Format(defaultValue + bonusValue);

        // Update fillBar
        float _mainValue = defaultValue / fillBarMaxValues[statType][age];
        float _bonusValue = (defaultValue + bonusValue) / fillBarMaxValues[statType][age];
        
        fillBarMain_image.sprite = fillBarMain;
        fillBarMain_image.type = Image.Type.Filled;
        fillBarMain_image.fillMethod = Image.FillMethod.Horizontal;
        fillBarMain_image.fillAmount = _mainValue;

        fillBarBonus_image.sprite = fillBarBonus_plus;
        fillBarBonus_image.type = Image.Type.Filled;
        fillBarBonus_image.fillMethod = Image.FillMethod.Horizontal;

        fillBar_bonus.SetActive(true);
        if (bonusValue == 0)
            fillBar_bonus.SetActive(false);
        else if (bonusValue > 0)
            fillBarBonus_image.fillAmount = _bonusValue;
        else if (bonusValue < 0)
        {
            fillBarBonus_image.fillAmount = _mainValue;
            fillBarBonus_image.sprite = fillBarBonus_minus;
            fillBarMain_image.fillAmount = _bonusValue;
        }
    }

    public void AddStatEffect(ItemCard.StatEffect statEffect, float value)
    {
        GameObject newStatEffect = Instantiate(statEffectPrefab, statEffects);
        newStatEffect.GetComponent<ItemCardStatEffect>().SetUp(statEffect, value, statEffect_Sprite_pairs[statEffect]);
    }
    public void AddStatEffect_FullSetBonus(ItemCard.StatEffect statEffect, Dictionary<StatType, float> values)
    {
        GameObject newStatEffect = Instantiate(statEffectPrefab, statEffects);
        newStatEffect.GetComponent<ItemCardStatEffect>().SetUp(values, statEffect_Sprite_pairs[statEffect]);
    }
    public void RemoveStatEffects()
    {
        for (int i = 0; i < statEffects.childCount; i++)
            Destroy(statEffects.GetChild(i).gameObject);
    }
}
