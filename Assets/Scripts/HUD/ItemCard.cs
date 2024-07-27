using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public enum StatEffect
    {
        Poison,
        Bleeding,
        Burning,
        Homing,
        FullSetBonus,
        AoE,
        Piercing,
        FireMagic,
        WaterMagic,
        EarthMagic,
        AirMagic,
        LightMagic,
        DarkMagic,
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


    private void Start() { HideItemCard(); }
    public void ShowItemCard(Item item)
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
                itemPos = new(itemPos.x - 570, itemPos.y);
            else
                itemPos = new(itemPos.x + 70, itemPos.y);
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

            // Item stats
            string[] rows = new string[12]; // 12 rows

            // Melle weapon
            if (item.slotType == Slot.SlotType.WeaponMelee)
            {
                if (item.twoHanded)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "Two handed ";
                else
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "One handed ";
                if (item.weaponType == Item.WeaponType.Whip)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "whip";
                else if (item.weaponType == Item.WeaponType.Dagger)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "dagger";
                else if (item.weaponType == Item.WeaponType.Sword)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "sword";
                else if (item.weaponType == Item.WeaponType.Rapier)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "rapier";
                else if (item.weaponType == Item.WeaponType.Axe)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "axe";
                else if (item.weaponType == Item.WeaponType.Mace)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "mace";
                else if (item.weaponType == Item.WeaponType.Hammer_oneHanded)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "hammer";
                else if (item.weaponType == Item.WeaponType.QuarterStaff)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "quarter staff";
                else if (item.weaponType == Item.WeaponType.Spear)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "spear";
                else if (item.weaponType == Item.WeaponType.Longsword)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "longsword";
                else if (item.weaponType == Item.WeaponType.Halbert)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "halbert";
                else if (item.weaponType == Item.WeaponType.Hammer_twoHanded)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "hammer";
                else if (item.weaponType == Item.WeaponType.Zweihander)
                    transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "zweihander";

                // Generating stats
                List<string> _stats = new() { "damage", "penetration", "armorIgnore", "critChance", "critDamage" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    if (playerStats.GetNonMagicSkillBonusStats(item.weaponClass).TryGetValue(_stats[i], out float bonus))
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], bonus);
                    else
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                }
                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Ranged weapon
            else if (item.slotType == Slot.SlotType.WeaponRanged)
            {
                if (item.twoHanded)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Two handed ";
                else
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "One handed ";
                if (item.weaponType == Item.WeaponType.Bow)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "bow";
                else if (item.weaponType == Item.WeaponType.SMG)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "smg";
                else if (item.weaponType == Item.WeaponType.Pistol)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "pistol";
                else if (item.weaponType == Item.WeaponType.AttackRifle)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "attack rifle";
                else if (item.weaponType == Item.WeaponType.Thrower)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "thrower";
                else if (item.weaponType == Item.WeaponType.Longbow)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "longbow";
                else if (item.weaponType == Item.WeaponType.Crossbow)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "crossbow";
                else if (item.weaponType == Item.WeaponType.Shotgun)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "shotgun";
                else if (item.weaponType == Item.WeaponType.Revolver)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "revolver";
                else if (item.weaponType == Item.WeaponType.Machinegun)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "machinegun";
                else if (item.weaponType == Item.WeaponType.SniperRifle)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "sniper rifle";
                else if (item.weaponType == Item.WeaponType.Launcher)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "launcher";

                // Generating stats
                List<string> _stats = new() { "damage", "penetration", "armorIgnore", "magazineSize", "attackSpeed", "reloadTime" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    if (playerStats.GetNonMagicSkillBonusStats(item.weaponClass).TryGetValue(_stats[i], out float bonus))
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], bonus);
                    else
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                }
                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Magic weapon
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
                List<string> _stats = new() { "damage", "penetration", "armorIgnore" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    // Get skill bonuses depending on magic crystals type and count
                    float finalBonus = 0f;
                    foreach(Item.MagicCrystalType crystalType in item.magicSkillBonuses.Keys)
                        if (playerStats.GetMagicSkillBonusStats(crystalType, item.magicSkillBonuses[crystalType]).TryGetValue(_stats[i], out float bonus))
                            finalBonus += bonus;
                    stats[i].SetUp(_stats[i], item.stats[_stats[i]], finalBonus);
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
            // Armor
            else if (item.slotType == Slot.SlotType.Head | item.slotType == Slot.SlotType.Torso | item.slotType == Slot.SlotType.Legs | item.slotType == Slot.SlotType.Gloves)
            {
                // Generating stats
                List<string> _stats = new() { "armor" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    if (playerStats.GetNonMagicSkillBonusStats(item.weaponClass).TryGetValue(_stats[i], out float bonus))
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], bonus);
                    else
                        stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                }
                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Equipable
            else if (item.slotType == Slot.SlotType.HeadEquipment | item.slotType == Slot.SlotType.TorsoEquipment | item.slotType == Slot.SlotType.LegsEquipment | item.slotType == Slot.SlotType.GlovesEquipment)
            {

                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Consumable
            else if (item.slotType == Slot.SlotType.Consumable)
            {
                rows[0] = "Consumable";
                int row = 1;
                foreach (string stat in item.stats.Keys)
                    switch (stat)
                    {
                        case "hpRefill":
                            rows[row] = "HP regen: " + item.stats[stat];
                            row++;
                            break;
                        case "staminaRefill":
                            rows[row] = "Stamina regen: " + item.stats[stat];
                            row++;
                            break;
                        case "manaRefill":
                            rows[row] = "Mana regen: " + item.stats[stat];
                            row++;
                            break;
                        case "cooldown":
                            rows[row] = "Cooldown: " + item.stats[stat] + "s";
                            row++;
                            break;
                    }
                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Projectile
            else if (item.slotType == Slot.SlotType.Ammo)
            {
                // Generating stats
                List<string> _stats = new() { "damage", "penetration", "armorIgnore" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                }
                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Shield
            else if (item.slotType == Slot.SlotType.Shield)
            {
                // Generating stats
                List<string> _stats = new() { "defense" };
                for (int i = 0; i < _stats.Count; i++)
                {
                    stats.Add(Instantiate(statPrefab, transform.Find("ItemStats")).GetComponent<ItemCardStat>());
                    stats[i].age = (int)playerStats.playerStats["age"];
                    stats[i].SetUp(_stats[i], item.stats[_stats[i]], 0);
                }
                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Backpack/Belt
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
            // Material
            else if (item.slotType == Slot.SlotType.Material)
            {

                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            // Magic crystal
            else if (item.slotType == Slot.SlotType.MagicCrystal)
            {
                transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "Magic crystal";

                weight.text = (item.stats["weight"] * item.amount).ToString() + " Kg";
                price.text = (item.stats["price"] * item.amount).ToString();
            }
            else
                rows[1] = "Tak na tohle (" + item.slotType.ToString() + ") jsem zapomnìl.";

            // Item description
            transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text += "\n" + item.description;
        }
    }

    public void HideItemCard()
    {
        isOpen = false;

        // Reset magic crystals
        foreach (GameObject go in crystalSlots)
            go.SetActive(false);

        // Reset stats
        for (int i = 0; i < transform.Find("ItemStats").transform.childCount; i++)
            Destroy(transform.Find("ItemStats").transform.GetChild(i).gameObject);
        stats = new();

        gameObject.SetActive(false);
    }
}
