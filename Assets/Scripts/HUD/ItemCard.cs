using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerStats;

public class ItemCard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    public enum StatEffect
    {
        Poison,
        Bleeding,
        BurningChance,
        Homing,
        FullSetBonus,
        AoE,
        Piercing,
        BleedingResistance,
        PoisonResistance,
        MagicResistance,
        Knockback,
        StunChance,
    }

    private bool isOpen;
    [SerializeField]
    private Sprite itemCardGFX;
    [SerializeField] private Sprite crystalFire;
    [SerializeField] private Sprite crystalWater;
    [SerializeField] private Sprite crystalAir;
    [SerializeField] private Sprite crystalEarth;
    [SerializeField] private Sprite crystalLight;
    [SerializeField] private Sprite crystalDark;

    [SerializeField] private List<GameObject> crystalSlots;

    [SerializeField] private GameObject statPrefab;

    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private Image gfx;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private List<ItemCardStat> stats = new();
    [SerializeField] private PlayerStats playerStats;

    [HideInInspector] public bool pointerOnItemUI = false;
    private bool pointerOnItemCard = false;

    private Item _item;

    private readonly Dictionary<Slot.SlotType, List<StatType>> statMapping = new()
    {
        { Slot.SlotType.WeaponMelee, new List<StatType>{ StatType.Damage, StatType.Penetration, StatType.ArmorIgnore, StatType.CritChance, StatType.CritDamage, StatType.BonusDefense } },
        { Slot.SlotType.WeaponRanged, new List<StatType>{ StatType.Damage, StatType.Penetration, StatType.ArmorIgnore, StatType.MagazineSize, StatType.AttackSpeed, StatType.ReloadTime, StatType.BonusDefense } },
        { Slot.SlotType.MagicWeapon, new List<StatType>{ StatType.Damage, StatType.Penetration, StatType.ArmorIgnore, StatType.BonusDefense } },
        { Slot.SlotType.Ammo, new List<StatType>{ StatType.Damage, StatType.Penetration, StatType.ArmorIgnore } },
        { Slot.SlotType.Shield, new List<StatType>{ StatType.BonusDefense } },
        { Slot.SlotType.Backpack, new List<StatType>{ StatType.AdditionalInventorySlots } },
        { Slot.SlotType.Belt, new List<StatType>{ StatType.AdditionalInventorySlots } },
        { Slot.SlotType.Helmet, new List<StatType>{ StatType.Armor, StatType.MagicResistance } },
        { Slot.SlotType.Chestplate, new List<StatType>{ StatType.Armor, StatType.MagicResistance } },
        { Slot.SlotType.Leggins, new List<StatType>{ StatType.Armor, StatType.MagicResistance } },
        { Slot.SlotType.Gauntlets, new List<StatType>{ StatType.Armor, StatType.MagicResistance } },
    };



    private void Start() { HideItemCard(); }
    private void Update()
    {
        if (!isOpen)
            return;

        if (!pointerOnItemUI && !pointerOnItemCard)
            HideItemCard();

        if (!pointerOnItemUI)
            StopAllCoroutines();
    }


    public IEnumerator ShowItemCard(Item item)
    {
        _item = item;
        yield return new WaitForSecondsRealtime(0.75f);
        if (pointerOnItemUI && _item == item)
        {
            if (isOpen)
            {
                HideItemCard();
                isOpen = false;
            }
            else
            {
                isOpen = true;
                gameObject.SetActive(true);

                // Correcting position
                Vector3 itemPos = item.gameObject.transform.position;
                itemPos.x -= itemPos.x > 1300 ? 500 : 0;
                itemPos.y -= itemPos.y > 650 ? 570 : 250;
                transform.position = itemPos;

                // ItemCard GFX
                gfx.sprite = itemCardGFX;

                // Item image
                itemImage.sprite = item.sprite_inventory;

                // Item name
                itemName.text = item.itemName;

                if (statMapping.TryGetValue(item.slotType, out List<StatType> statsList))
                    foreach (StatType stat in statsList)
                    {
                        float bonus = 0;
                        if(playerStats.bonusesFromSkills.ContainsKey(item.weaponClass))
                            bonus = playerStats.bonusesFromSkills[item.weaponClass].TryGetValue(stat, out float b) ? b : 0;
                        AddStat(stat, item.stats[stat], bonus, item);
                    }

                // Item description
                string itemType = GetItemTypeAsString(item.itemType);
                itemDescription.text = $"{(ShowTwoHanded(item.itemType) ? (item.twoHanded ? "Two handed " : "One handed ") : "")}{itemType}\n{item.description}";

                // Weight and price
                weight.text = Math.Round(item.stats[StatType.Weight] * item.amount, 1).ToString();
                price.text = Math.Round(item.stats[StatType.Price] * item.amount, 1).ToString();
            }
        }
    }
    public void HideItemCard()
    {
        _item = null;
        isOpen = false;
        pointerOnItemUI = false;
        pointerOnItemCard = false;

        // Reset magic crystals
        foreach (GameObject go in crystalSlots)
            go.SetActive(false);

        // Reset stats
        for (int i = 0; i < transform.Find("ItemStats").transform.childCount; i++)
            Destroy(transform.Find("ItemStats").transform.GetChild(i).gameObject);
        stats = new();
        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "";

        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    private bool ShowTwoHanded(Item.ItemType itemType)
    {
        return itemType != Item.ItemType.Projectile &&
               itemType != Item.ItemType.Consumable &&
               itemType != Item.ItemType.Helmet &&
               itemType != Item.ItemType.Chestplate &&
               itemType != Item.ItemType.Leggings &&
               itemType != Item.ItemType.Gauntlets &&
               itemType != Item.ItemType.HelmetAccessory &&
               itemType != Item.ItemType.ChestplateAccessory &&
               itemType != Item.ItemType.LeggingsAccessory &&
               itemType != Item.ItemType.GauntletsAccessory &&
               itemType != Item.ItemType.Backpack &&
               itemType != Item.ItemType.Belt;
    }
    private string GetItemTypeAsString(Item.ItemType itemType)
    {
        return itemType switch
        {
            Item.ItemType.Whip => "whip",
            Item.ItemType.Dagger => "dagger",
            Item.ItemType.Sword => "sword",
            Item.ItemType.Rapier => "rapier",
            Item.ItemType.LightShield => "light shield",
            Item.ItemType.Axe => "axe",
            Item.ItemType.Mace => "mace",
            Item.ItemType.Hammer_oneHanded => "hammer",
            Item.ItemType.HeavyShield => "heavy shield",
            Item.ItemType.QuarterStaff => "quarter staff",
            Item.ItemType.Spear => "spear",
            Item.ItemType.Longsword => "longsword",
            Item.ItemType.Halbert => "halbert",
            Item.ItemType.Hammer_twoHanded => "hammer",
            Item.ItemType.Zweihander => "zweihander",
            Item.ItemType.Bow => "bow",
            Item.ItemType.SMG => "smg",
            Item.ItemType.Pistol => "pistol",
            Item.ItemType.AttackRifle => "attack rifle",
            Item.ItemType.Thrower => "thrower",
            Item.ItemType.Longbow => "longbow",
            Item.ItemType.Crossbow => "crossbow",
            Item.ItemType.Shotgun => "shotgun",
            Item.ItemType.Revolver => "revolver",
            Item.ItemType.Machinegun => "machinegun",
            Item.ItemType.SniperRifle => "sniper rifle",
            Item.ItemType.Launcher => "launcher",
            Item.ItemType.Throwable => "throwable",
            Item.ItemType.Trap => "trap",
            Item.ItemType.Projectile => "Projectile",
            Item.ItemType.Consumable => "Consumable",
            Item.ItemType.Backpack => "Backpack",
            Item.ItemType.Belt => "Belt",
            Item.ItemType.Helmet => "Helmet",
            Item.ItemType.Chestplate => "Chestplate",
            Item.ItemType.Leggings => "Leggings",
            Item.ItemType.Gauntlets => "Gauntlets",
            Item.ItemType.HelmetAccessory => "Helmet accessory",
            Item.ItemType.ChestplateAccessory => "Chestplate accessory",
            Item.ItemType.LeggingsAccessory => "Leggings accessory",
            Item.ItemType.GauntletsAccessory => "Gauntlets accessory",
            _ => string.Empty,
        };
    }
    private void AddStat(StatType statType, float baseValue, float bonusValue, Item item)
    {
        ItemCardStat stat = Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>();
        stat.age = (int)playerStats.playerStats[StatType.Age];
        stat.SetUp(statType, baseValue, bonusValue);
        AddStatEffects(item, stat, statType);
        stats.Add(stat);
    }
    private void AddStatEffects(Item item, ItemCardStat itemCardStat, StatType statType)
    {
        switch (statType)
        {
            case StatType.Damage:
                if (item.AoE)
                    itemCardStat.AddStatEffect(StatEffect.AoE, 0);
                if (item.selfHoming)
                    itemCardStat.AddStatEffect(StatEffect.Homing, 0);
                if (item.stats.TryGetValue(StatType.PoisonDamage, out float value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Poison, value);
                if (item.stats.TryGetValue(StatType.BleedingDamage, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Bleeding, value);
                if (item.stats.TryGetValue(StatType.BurningChance, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.BurningChance, value);
                break;
            case StatType.Penetration:
                if (item.stats.TryGetValue(StatType.Piercing, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Piercing, value);
                break;
            case StatType.Armor:
                if (item.stats.TryGetValue(StatType.BleedingResistance, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.BleedingResistance, value);
                if (item.stats.TryGetValue(StatType.PoisonResistance, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.PoisonResistance, value);
                if (item.stats.TryGetValue(StatType.MagicResistance, out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.MagicResistance, value);
                if (item.fullSetBonus != null)
                    itemCardStat.AddStatEffect_FullSetBonus(StatEffect.FullSetBonus, item.fullSetBonus);
                break;
        }
    }


    public void OnPointerExit(PointerEventData eventData) { pointerOnItemCard = false; }
    public void OnPointerEnter(PointerEventData eventData) { pointerOnItemCard = true; }
    public void OnPointerClick(PointerEventData eventData) { HideItemCard(); }
}
