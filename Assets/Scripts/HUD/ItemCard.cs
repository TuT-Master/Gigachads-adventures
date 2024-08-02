using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private List<ItemCardStat> stats = new();
    private PlayerStats playerStats;

    [HideInInspector] public bool pointerOnItemUI = false;
    private bool pointerOnItemCard = false;

    private Item _item;

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
                if (isOpen)
                    HideItemCard();
                isOpen = false;
            }
            else
            {
                isOpen = true;
                gameObject.SetActive(true);

                // Correcting position
                Vector3 itemPos = item.gameObject.transform.position;
                if (itemPos.x > 1300)
                    itemPos = new(itemPos.x - 500, itemPos.y);
                else
                    itemPos = new(itemPos.x, itemPos.y);
                if (itemPos.y > 650)
                    itemPos = new(itemPos.x, itemPos.y - 570);
                else
                    itemPos = new(itemPos.x, itemPos.y - 250);
                transform.position = itemPos;

                // ItemCard GFX
                transform.Find("GFX").GetComponent<Image>().sprite = itemCardGFX;

                // Item image
                transform.Find("ItemImage").GetComponent<Image>().sprite = item.sprite_inventory;

                // Item name
                transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;

                #region Melle weapon
                if (item.slotType == Slot.SlotType.WeaponMelee)
                {
                    if (item.twoHanded)
                        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "Two handed ";
                    else
                        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "One handed ";
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += GetWeaponClass(item.weaponType);

                    // Generating stats
                    List<string> _stats = new() { "damage", "penetration", "armorIgnore", "critChance", "critDamage", "defense" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        if (playerStats.GetSkillBonusStats(item.weaponClass).TryGetValue(_stats[i], out float bonus))
                            stats[i].SetUp(_stats[i], item.stats[_stats[i]], bonus);
                        else
                            stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Ranged weapon
                else if (item.slotType == Slot.SlotType.WeaponRanged)
                {
                    if (item.twoHanded)
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Two handed ";
                    else
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "One handed ";
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += GetWeaponClass(item.weaponType);

                    // Generating stats
                    List<string> _stats = new() { "damage", "penetration", "armorIgnore", "magazineSize", "attackSpeed", "reloadTime", "defense" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        if (playerStats.GetSkillBonusStats(item.weaponClass).TryGetValue(_stats[i], out float bonus))
                            stats[i].SetUp(_stats[i], item.stats[_stats[i]], bonus);
                        else
                            stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }

                    // Weight and price
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Magic weapon
                else if (item.slotType == Slot.SlotType.MagicWeapon)
                {
                    // One handed / two handed
                    if (item.twoHanded)
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Two handed ";
                    else
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "One handed ";
                    // Stuff / book / etc.
                    if (item.itemName.ToLower().Contains("book"))
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "book";
                    else if (item.itemName.ToLower().Contains("staff"))
                        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "staff";
                    // Generating stats
                    List<string> _stats = new() { "damage", "penetration", "armorIgnore", "defense" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        // Get skill bonuses depending on magic crystals type and count
                        float finalBonus = 0f;
                        foreach (Item.MagicCrystalType crystalType in item.magicSkillBonuses.Keys)
                            if (playerStats.GetSkillBonusStats(crystalType, item.magicSkillBonuses[crystalType]).TryGetValue(_stats[i], out float bonus))
                                finalBonus += bonus;
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], finalBonus);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();

                    // Used spell


                    // Magic crystals
                    if (item.magicCrystals != null)
                    {
                        for (int i = 0; i < item.magicCrystals.Count; i++)
                        {
                            GameObject crystal = crystalSlots[i];
                            crystal.SetActive(true);
                            switch (item.magicCrystals[i])
                            {
                                case Item.MagicCrystalType.Fire:
                                    crystal.GetComponent<Image>().sprite = crystalFire;
                                    break;
                                case Item.MagicCrystalType.Water:
                                    crystal.GetComponent<Image>().sprite = crystalWater;
                                    break;
                                case Item.MagicCrystalType.Air:
                                    crystal.GetComponent<Image>().sprite = crystalAir;
                                    break;
                                case Item.MagicCrystalType.Earth:
                                    crystal.GetComponent<Image>().sprite = crystalEarth;
                                    break;
                                case Item.MagicCrystalType.Light:
                                    crystal.GetComponent<Image>().sprite = crystalLight;
                                    break;
                                case Item.MagicCrystalType.Dark:
                                    crystal.GetComponent<Image>().sprite = crystalDark;
                                    break;
                            }
                        }
                    }
                }
                #endregion
                #region Armor
                else if (item.slotType == Slot.SlotType.Head | item.slotType == Slot.SlotType.Torso | item.slotType == Slot.SlotType.Legs | item.slotType == Slot.SlotType.Gloves)
                {
                    // Generating stats
                    List<string> _stats = new() { "armor", "magicResistance" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        if (GetComponentInParent<PlayerFight>().itemInHand != null && playerStats.GetSkillBonusStats(GetComponentInParent<PlayerFight>().itemInHand.weaponClass).TryGetValue("armorIncrease", out float bonus))
                            stats[i].SetUp(_stats[i], item.armorStats[_stats[i]], bonus);
                        else
                            stats[i].SetUp(_stats[i], item.armorStats[_stats[i]], 0);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }
                    weight.text = (item.armorStats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.armorStats["price"] * item.amount).ToString();
                }
                #endregion
                #region Equipable
                else if (item.slotType == Slot.SlotType.HeadEquipment | item.slotType == Slot.SlotType.TorsoEquipment | item.slotType == Slot.SlotType.LegsEquipment | item.slotType == Slot.SlotType.GlovesEquipment)
                {

                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Consumable
                else if (item.slotType == Slot.SlotType.Consumable)
                {

                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Projectile
                else if (item.slotType == Slot.SlotType.Ammo)
                {
                    // Generating stats
                    List<string> _stats = new() { "damage", "penetration", "armorIgnore" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Shield
                else if (item.slotType == Slot.SlotType.Shield)
                {
                    // Generating stats
                    List<string> _stats = new() { "defense" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                        AddStatEffects(item, stats[i], _stats[i]);
                    }
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Backpack/Belt
                else if (item.slotType == Slot.SlotType.Backpack | item.slotType == Slot.SlotType.Belt)
                {
                    // Generating stats
                    List<string> _stats = new() { "backpackSize" };
                    for (int i = 0; i < _stats.Count; i++)
                    {
                        stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                        stats[i].age = (int)playerStats.playerStats["age"];
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                    }
                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Material
                else if (item.slotType == Slot.SlotType.Material)
                {

                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion
                #region Magic crystal
                else if (item.slotType == Slot.SlotType.MagicCrystal)
                {
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "Magic crystal";

                    weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                    price.text = (item.stats["price"] * item.amount).ToString();
                }
                #endregion

                // Item description
                transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "\n" + item.description;
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
    private void AddStatEffects(Item item, ItemCardStat itemCardStat, string stat)
    {
        float value = 0f;
        switch (stat)
        {
            case "damage":
                if (item.AoE)
                    itemCardStat.AddStatEffect(StatEffect.AoE, 0);
                if (item.selfHoming)
                    itemCardStat.AddStatEffect(StatEffect.Homing, 0);
                if (item.stats.TryGetValue("poisonDamage", out value) && value > 0)
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
