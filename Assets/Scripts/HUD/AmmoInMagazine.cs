using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (playerFight.itemInHand != null && playerFight.itemInHand.slotType == Slot.SlotType.WeaponRanged && playerFight.itemInHand.stats["magazineSize"] > 0)
        {
            image.color = Color.white;
            ammoCount_text.text = playerFight.itemInHand.stats["currentMagazine"] + " / " + playerFight.itemInHand.stats["magazineSize"];
        }
        else
        {
            image.color = new(1, 1, 1, 0);
            ammoCount_text.text = "";
        }
    }
}
