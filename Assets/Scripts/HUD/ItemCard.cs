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

            // Item description
            transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;

            // Item stats
            string[] rows = new string[12]; // 12 rows

            // Melle weapon
            if (item.slotType == Slot.SlotType.WeaponMelee)
            {
                if (item.twoHanded)
                    rows[0] = "Two handed ";
                else
                    rows[0] = "One handed ";
                if (item.weaponType == Item.WeaponType.Whip)
                    rows[0] += "whip";
                else if (item.weaponType == Item.WeaponType.Dagger)
                    rows[0] += "dagger";
                else if (item.weaponType == Item.WeaponType.Sword)
                    rows[0] += "sword";
                else if (item.weaponType == Item.WeaponType.Rapier)
                    rows[0] += "rapier";
                else if (item.weaponType == Item.WeaponType.Axe)
                    rows[0] += "axe";
                else if (item.weaponType == Item.WeaponType.Mace)
                    rows[0] += "mace";
                else if (item.weaponType == Item.WeaponType.Hammer_oneHanded)
                    rows[0] += "hammer";
                else if (item.weaponType == Item.WeaponType.QuarterStaff)
                    rows[0] += "quarter staff";
                else if (item.weaponType == Item.WeaponType.Spear)
                    rows[0] += "spear";
                else if (item.weaponType == Item.WeaponType.Longsword)
                    rows[0] += "longsword";
                else if (item.weaponType == Item.WeaponType.Halbert)
                    rows[0] += "halbert";
                else if (item.weaponType == Item.WeaponType.Hammer_twoHanded)
                    rows[0] += "hammer";
                else if (item.weaponType == Item.WeaponType.Zweihander)
                    rows[0] += "zweihander";

                rows[1] = "Damage: " + item.stats["damage"].ToString();
                rows[2] = "Penetration: " + item.stats["penetration"].ToString();
                rows[3] = "Ignores " + (item.stats["armorIgnore"] * 100).ToString() + "% of armor";
                rows[4] = "Crit chance: doplnit stat";
                rows[5] = "Crit damage: doplnit stat";

                rows[10] = "Weight: " + (item.stats["weight"] * item.amount).ToString() + " Kg";
                rows[11] = "Cost: " + item.stats["price"].ToString();
            }
            // Ranged weapon
            else if (item.slotType == Slot.SlotType.WeaponRanged)
            {
                if (item.twoHanded)
                    rows[0] = "Two handed ";
                else
                    rows[0] = "One handed ";
                if (item.weaponType == Item.WeaponType.Bow)
                    rows[0] += "bow";
                else if (item.weaponType == Item.WeaponType.SMG)
                    rows[0] += "smg";
                else if (item.weaponType == Item.WeaponType.Pistol)
                    rows[0] += "pistol";
                else if (item.weaponType == Item.WeaponType.AttackRifle)
                    rows[0] += "attack rifle";
                else if (item.weaponType == Item.WeaponType.Thrower)
                    rows[0] += "thrower";
                else if (item.weaponType == Item.WeaponType.Longbow)
                    rows[0] += "longbow";
                else if (item.weaponType == Item.WeaponType.Crossbow)
                    rows[0] += "crossbow";
                else if (item.weaponType == Item.WeaponType.Shotgun)
                    rows[0] += "shotgun";
                else if (item.weaponType == Item.WeaponType.Revolver)
                    rows[0] += "revolver";
                else if (item.weaponType == Item.WeaponType.Machinegun)
                    rows[0] += "machinegun";
                else if (item.weaponType == Item.WeaponType.SniperRifle)
                    rows[0] += "sniper rifle";
                else if (item.weaponType == Item.WeaponType.Launcher)
                    rows[0] += "launcher";

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
                    rows[0] = "Two handed ";
                else
                    rows[0] = "One handed ";
                // Stuff / book / etc.
                if (item.itemName.ToLower().Contains("book"))
                    rows[0] += "book";
                else if (item.itemName.ToLower().Contains("staff"))
                    rows[0] += "staff";

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
                        GameObject crystal = transform.Find("CrystalSlot" + i).gameObject;
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
        }
    }

    public void HideItemCard()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }
}
