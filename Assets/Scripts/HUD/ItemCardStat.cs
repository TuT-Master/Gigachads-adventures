using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    private readonly Dictionary<string, List<float>> fillBarMaxValues = new()
    {
        {"damage", new List<float>(){ 20, 40, 60, 80, 100 } },
        {"penetration", new List<float>(){ 10, 20, 30, 40, 50 } },
        {"armorIgnore", new List<float>(){ 1, 1, 1, 1, 1 } },
        {"critChance", new List<float>(){ 1, 1, 1, 1, 1 } },
        {"critDamage", new List<float>(){ 3, 3, 3, 3, 3 } },
        {"magazineSize", new List<float>(){ 15, 20, 40, 100, 500 } },
        {"attackSpeed", new List<float>(){ 5, 10, 15, 20, 40 } },
        {"reloadTime", new List<float>(){ 5, 5, 5, 5, 5 } },
        {"defense", new List<float>(){ 100, 100, 100, 100, 100 } },
        {"backpackSize", new List<float>(){ 20, 20, 20, 20, 20 } },
        {"armor", new List<float>(){ 15, 30, 45, 60, 75 } },
        {"magicResistance", new List<float>(){ 15, 30, 45, 60, 75 } },
    };
    /*
    ammo
    effect (equipables)
    */

    private Image fillBarMain_image;
    private Image fillBarBonus_image;

    private Dictionary<string, (string displayName, Sprite sprite)> statDisplayData;
    private void Awake()
    {
        statDisplayData = new Dictionary<string, (string, Sprite)>
        {
            { "damage", ("Damage", damage) },
            { "penetration", ("Penetration", penetration) },
            { "armorIgnore", ("Armor ignore", armorIgnore) },
            { "critChance", ("Critical chance", critChance) },
            { "critDamage", ("Critical damage", critDamage) },
            { "magazineSize", ("Magazine size", magazineSize) },
            { "attackSpeed", ("Attack speed", attackSpeed) },
            { "reloadTime", ("Reload time", reloadTime) },
            { "defense", ("Defense", defense) },
            { "backpackSize", ("Additional slots", additionalSlots) },
            { "armor", ("Armor", armor) },
            { "magicResistance", ("Magic resistance", magicResistance) },
        };
    }

    private void Start()
    {
        fillBarMain_image = fillBar_main.GetComponent<Image>();
        fillBarBonus_image = fillBar_bonus.GetComponent<Image>();
    }



    public void SetUp(string stat, float defaultValue, float bonusValue)
    {
        statEffect_Sprite_pairs = new();
        for(int i = 0; i < _statEffect.Count; i++)
            statEffect_Sprite_pairs.Add(_statEffect[i], _statEffectSprites[i]);

        // Set up name of stat
        if (statDisplayData.TryGetValue(stat, out var displayData))
        {
            statName.text = displayData.displayName;
            statImage.sprite = displayData.sprite;
        }
        else
        {
            statName.text = $"Stat '{stat}' not found!";
            statImage.sprite = null;
        }

        // Set up values of stat
        static string Format(float value, int decimals = 2) => Math.Round(value, decimals).ToString();

        if (stat == "armorIgnore")
            statValue.text = $"{Format((defaultValue + bonusValue) * 100)}%";
        else if (stat == "attackSpeed")
            statValue.text = $"{Format(defaultValue + bonusValue)} / s";
        else if (stat == "reloadTime")
            statValue.text = $"{Format(defaultValue + bonusValue)} s";
        else if (stat == "backpackSize")
            statValue.text = $"+ {Format(defaultValue + bonusValue)}";
        else
            statValue.text = Format(defaultValue + bonusValue);

        // Update fillBar
        float _mainValue = defaultValue / fillBarMaxValues[stat][age];
        float _bonusValue = (defaultValue + bonusValue) / fillBarMaxValues[stat][age];
        
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
    public void AddStatEffect_FullSetBonus(ItemCard.StatEffect statEffect, Dictionary<string, float> values)
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
