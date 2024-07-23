using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
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


    private void Start() { HideItemCard(); }
    public void ShowItemCard(Item item)
    {

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
            transform.GetChild(0).GetComponent<Image>().sprite = itemCardGFX;

            // Item image
            transform.GetChild(1).GetComponent<Image>().sprite = item.sprite_inventory;

            // Item name
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;

            // Item stats
            string[] rows = new string[12]; // 12 rows

            // Melle weapon
            if (item.slotType == Slot.SlotType.WeaponMelee)
            {
                if (item.twoHanded)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Two handed ";
                else
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "One handed ";
                if (item.weaponType == Item.WeaponType.Whip)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "whip";
                else if (item.weaponType == Item.WeaponType.Dagger)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "dagger";
                else if (item.weaponType == Item.WeaponType.Sword)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "sword";
                else if (item.weaponType == Item.WeaponType.Rapier)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "rapier";
                else if (item.weaponType == Item.WeaponType.Axe)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "axe";
                else if (item.weaponType == Item.WeaponType.Mace)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "mace";
                else if (item.weaponType == Item.WeaponType.Hammer_oneHanded)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "hammer";
                else if (item.weaponType == Item.WeaponType.QuarterStaff)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "quarter staff";
                else if (item.weaponType == Item.WeaponType.Spear)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "spear";
                else if (item.weaponType == Item.WeaponType.Longsword)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "longsword";
                else if (item.weaponType == Item.WeaponType.Halbert)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "halbert";
                else if (item.weaponType == Item.WeaponType.Hammer_twoHanded)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "hammer";
                else if (item.weaponType == Item.WeaponType.Zweihander)
                    transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "zweihander";

                rows[1] = "Damage: " + item.stats["damage"].ToString();
                rows[2] = "Penetration: " + item.stats["penetration"].ToString();
                rows[3] = "Ignores " + (item.stats["armorIgnore"] * 100).ToString() + "% of armor";
                rows[4] = "Has " + (item.stats["critChance"] / 100).ToString() + "% chance for dealing critical damage";
                rows[5] = "Crit damage: " + item.stats["critDamage"].ToString();

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
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

                rows[1] = "Damage: " + item.stats["damage"].ToString();
                rows[2] = "Penetration: " + item.stats["penetration"].ToString();
                rows[3] = "Ignores " + (item.stats["armorIgnore"] * 100).ToString() + "% of armor";
                rows[4] = "Magazine: " + item.stats["currentMagazine"].ToString() + "/" + item.stats["magazineSize"].ToString();
                rows[5] = "Attack speed: " + item.stats["attackSpeed"].ToString() + " /sec";
                rows[6] = "Reload time: " + item.stats["reloadTime"].ToString() + " sec";

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
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

                rows[1] = "Damage: " + item.stats["damage"].ToString();
                rows[2] = "Penetration: " + item.stats["penetration"].ToString();
                rows[3] = "Ignores " + (item.stats["armorIgnore"] * 100).ToString() + "% of armor";

                // Used spell
                rows[5] = "Using " + item.spell.ToString();


                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();

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
                rows[1] = "Armor: " + item.armorStats["armor"].ToString();
                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Equipable
            else if (item.slotType == Slot.SlotType.HeadEquipment | item.slotType == Slot.SlotType.TorsoEquipment | item.slotType == Slot.SlotType.LegsEquipment | item.slotType == Slot.SlotType.GlovesEquipment)
            {

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
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
                rows[1] = "Damage: " + item.stats["damage"].ToString();
                rows[2] = "Penetration: " + item.stats["penetration"].ToString();
                rows[3] = "Ignores " + (item.stats["armorIgnore"] * 100).ToString() + "% of armor";
                if (item.stats["splashDamage"] > 0)
                {
                    rows[4] = "Splash damage: " + item.stats["splashDamage"].ToString();
                    rows[5] = "Splash radius: " + item.stats["splashRadius"].ToString();
                }

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Shield
            else if (item.slotType == Slot.SlotType.Shield)
            {
                rows[1] = "Defense: " + item.stats["defense"].ToString();

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Backpack/Belt
            else if (item.slotType == Slot.SlotType.Backpack | item.slotType == Slot.SlotType.Belt)
            {
                rows[1] = "Inventory slots: +" + item.stats["backpackSize"].ToString();

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Material
            else if (item.slotType == Slot.SlotType.Material)
            {

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Magic crystal
            else if (item.slotType == Slot.SlotType.MagicCrystal)
            {
                rows[1] = item.crystalType.ToString() + " crystal";

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            else
                rows[1] = "Tak na tohle (" + item.slotType.ToString() + ") jsem zapomnìl.";

            // Write text
            string text = "";
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] != null)
                    text += rows[i];
                text += "\n";
            }
            transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = text;

            // Item description
            transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += "\n" + item.description;
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

        gameObject.SetActive(false);
    }
}
