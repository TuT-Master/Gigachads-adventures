using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;
using static PlayerStats;

public class AmmoInMagazine : MonoBehaviour
{
    private PlayerFight playerFight;
    private TextMeshProUGUI ammoCount_text;
    private Image image;


    void Start()
    {
        playerFight = GetComponentInParent<PlayerFight>();
        ammoCount_text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (playerFight.activeWeapon != null && playerFight.activeWeapon.slotType == Slot.SlotType.WeaponRanged && playerFight.activeWeapon.stats[StatType.MagazineSize] > 0)
        {
            image.color = Color.white;
            ammoCount_text.text = playerFight.activeWeapon.stats[StatType.CurrentMagazine] + " / " + playerFight.activeWeapon.stats[StatType.MagazineSize];
        }
        else
        {
            image.color = new(1, 1, 1, 0);
            ammoCount_text.text = "";
        }
    }
}
