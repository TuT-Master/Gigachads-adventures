using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

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
    private PlayerStats playerStats;

    [HideInInspector] public bool pointerOnItemUI = false;
    private bool pointerOnItemCard = false;

    private Item _item;


    private static readonly Dictionary<Slot.SlotType, List<string>> statMap = new()
    {
        { Slot.SlotType.WeaponMelee, new List<string>{ "damage", "penetration", "armorIgnore", "critChance", "critDamage", "defense" } },
        { Slot.SlotType.WeaponRanged, new List<string>{ "damage", "penetration", "armorIgnore", "magazineSize", "attackSpeed", "reloadTime", "defense" } },
        { Slot.SlotType.MagicWeapon, new List<string>{ "damage", "penetration", "armorIgnore", "defense" } },
        { Slot.SlotType.Ammo, new List<string>{ "damage", "penetration", "armorIgnore" } },
        { Slot.SlotType.Shield, new List<string>{ "defense" } },
        { Slot.SlotType.Backpack, new List<string>{ "backpackSize" } },
        { Slot.SlotType.Belt, new List<string>{ "backpackSize" } },
        { Slot.SlotType.Head, new List<string>{ "armor", "magicResistance" } },
        { Slot.SlotType.Torso, new List<string>{ "armor", "magicResistance" } },
        { Slot.SlotType.Legs, new List<string>{ "armor", "magicResistance" } },
        { Slot.SlotType.Gloves, new List<string>{ "armor", "magicResistance" } },
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
            playerStats = GetComponentInParent<PlayerStats>();
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

                if (statMap.TryGetValue(item.slotType, out List<string> statsList))
                    foreach (string stat in statsList)
                    {
                        float bonus = playerStats.GetSkillBonusStats(item.weaponClass).TryGetValue(stat, out float b) ? b : 0;
                        AddStat(stat, item.stats[stat], bonus, item);
                    }

                // Item description
                itemDescription.text = $"{(item.twoHanded ? "Two handed" : "One handed")} {GetWeaponClass(item.weaponType)}\n{item.description}";
            }
        }
    }

    private string GetWeaponClass(Item.WeaponType weaponType)
    {
        return weaponType switch
        {
            Item.WeaponType.Whip => "whip",
            Item.WeaponType.Dagger => "dagger",
            Item.WeaponType.Sword => "sword",
            Item.WeaponType.Rapier => "rapier",
            Item.WeaponType.LightShield => "light shield",
            Item.WeaponType.Axe => "axe",
            Item.WeaponType.Mace => "mace",
            Item.WeaponType.Hammer_oneHanded => "hammer",
            Item.WeaponType.HeavyShield => "heavy shield",
            Item.WeaponType.QuarterStaff => "quarter staff",
            Item.WeaponType.Spear => "spear",
            Item.WeaponType.Longsword => "longsword",
            Item.WeaponType.Halbert => "halbert",
            Item.WeaponType.Hammer_twoHanded => "hammer",
            Item.WeaponType.Zweihander => "zweihander",
            Item.WeaponType.Bow => "bow",
            Item.WeaponType.SMG => "smg",
            Item.WeaponType.Pistol => "pistol",
            Item.WeaponType.AttackRifle => "attack rifle",
            Item.WeaponType.Thrower => "thrower",
            Item.WeaponType.Longbow => "longbow",
            Item.WeaponType.Crossbow => "crossbow",
            Item.WeaponType.Shotgun => "shotgun",
            Item.WeaponType.Revolver => "revolver",
            Item.WeaponType.Machinegun => "machinegun",
            Item.WeaponType.SniperRifle => "sniper rifle",
            Item.WeaponType.Launcher => "launcher",
            Item.WeaponType.Throwable => "throwable",
            Item.WeaponType.Trap => "trap",
            _ => null
        };
    }

    private void AddStat(string statName, float baseValue, float bonusValue, Item item)
    {
        ItemCardStat stat = Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>();
        stat.age = (int)playerStats.playerStats["age"];
        stat.SetUp(statName, baseValue, bonusValue);
        AddStatEffects(item, stat, statName);
        stats.Add(stat);
    }
    private void AddStatEffects(Item item, ItemCardStat itemCardStat, string stat)
    {
        switch (stat)
        {
            case "damage":
                if (item.AoE)
                    itemCardStat.AddStatEffect(StatEffect.AoE, 0);
                if (item.selfHoming)
                    itemCardStat.AddStatEffect(StatEffect.Homing, 0);
                if (item.stats.TryGetValue("poisonDamage", out float value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Poison, value);
                if (item.stats.TryGetValue("bleedingDamage", out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Bleeding, value);
                if (item.stats.TryGetValue("burningChance", out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.BurningChance, value);
                break;
            case "penetration":
                if (item.stats.TryGetValue("piercing", out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.Piercing, value);
                break;
            case "armor":
                if (item.armorStats.TryGetValue("bleedingResistance", out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.BleedingResistance, value);
                if (item.armorStats.TryGetValue("poisonResistance", out value) && value > 0)
                    itemCardStat.AddStatEffect(StatEffect.PoisonResistance, value);
                if (item.fullSetBonus != null)
                    itemCardStat.AddStatEffect_FullSetBonus(StatEffect.FullSetBonus, item.fullSetBonus);
                break;
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



    public void OnPointerExit(PointerEventData eventData) { pointerOnItemCard = false; }
    public void OnPointerEnter(PointerEventData eventData) { pointerOnItemCard = true; }
    public void OnPointerClick(PointerEventData eventData) { HideItemCard(); }
}
