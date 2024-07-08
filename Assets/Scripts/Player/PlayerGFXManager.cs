using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerGFXManager : MonoBehaviour
{
    public GameObject hairObj;
    public GameObject beardObj;
    public GameObject torsoObj;
    public GameObject headArmorObj;
    public GameObject headEquipmentObj;
    public GameObject torsoArmorObj;
    public GameObject torsoEquipmentObj;
    public GameObject weaponObj;
    public GameObject secondaryWeaponObj;

    public bool hideHair = false;
    public bool hideBeard = false;
    public bool hideBody = false;

    public Sprite defaultHair;
    public Sprite defaultBeard;
    public Sprite defaultBody;

    [SerializeField]
    private SpriteRenderer headArmor;
    [SerializeField]
    private SpriteRenderer headEquipment;
    [SerializeField]
    private SpriteRenderer bodyArmor;
    [SerializeField]
    private SpriteRenderer bodyEquipment;

    private GameObject headSlot;
    private GameObject headEquipmentSlot;
    private GameObject torsoSlot;
    private GameObject torsoEquipmentSlot;

    private PlayerMovement playerMovement;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        headSlot = GetComponent<PlayerInventory>().armorSlots.transform.Find("Head").gameObject;
        headEquipmentSlot = GetComponent<PlayerInventory>().equipmentSlots.transform.Find("HeadEquipment").gameObject;
        torsoSlot = GetComponent<PlayerInventory>().armorSlots.transform.Find("Torso").gameObject;
        torsoEquipmentSlot = GetComponent<PlayerInventory>().equipmentSlots.transform.Find("TorsoEquipment").gameObject;
    }

    private void Update()
    {
        if(!playerMovement.turn)
        {
            // Looking towards player camera
            hairObj.transform.localPosition = new(0, 0.55f, 0.001f);
            beardObj.transform.localPosition = new(0, 0.55f, 0.001f);
            headArmorObj.transform.localPosition = new(0, 0.55f, 0.001f);
            headEquipmentObj.transform.localPosition = new(0, 0.55f, 0.002f);
            torsoArmorObj.transform.localPosition = new(0, 0.55f, 0.001f);
            torsoEquipmentObj.transform.localPosition = new(0, 0.55f, 0.002f);
            secondaryWeaponObj.transform.localPosition = new(0, 0.55f, 0.003f);
            weaponObj.transform.localPosition = new(0, 0.55f, 0.004f);
        }
        else
        {
            hairObj.transform.localPosition = new(0, 0.55f, -0.001f);
            beardObj.transform.localPosition = new(0, 0.55f, -0.001f);
            headArmorObj.transform.localPosition = new(0, 0.55f, -0.001f);
            headEquipmentObj.transform.localPosition = new(0, 0.55f, -0.002f);
            torsoArmorObj.transform.localPosition = new(0, 0.55f, -0.001f);
            torsoEquipmentObj.transform.localPosition = new(0, 0.55f, -0.002f);
            secondaryWeaponObj.transform.localPosition = new(0, 0.55f, -0.003f);
            weaponObj.transform.localPosition = new(0, 0.55f, -0.004f);
        }

        // Check visibility
        if (hideHair)
            hairObj.SetActive(false);
        else
        {
            hairObj.SetActive(true);
            if(defaultHair != null)
                hairObj.GetComponent<SpriteRenderer>().sprite = defaultHair;
        }
        if (hideBeard)
            beardObj.SetActive(false);
        else
        {
            beardObj.SetActive(true);
            if (defaultBeard != null)
                beardObj.GetComponent<SpriteRenderer>().sprite = defaultBeard;
        }
        if (hideBody)
            torsoObj.SetActive(false);
        else
        {
            torsoObj.SetActive(true);
            if (defaultBody != null)
                torsoObj.GetComponent<SpriteRenderer>().sprite = defaultBody;
        }
    }

    public void UpdateGFX()
    {
        // Head
        hairObj.GetComponent<SpriteRenderer>().sprite = defaultHair;
        // Beard
        beardObj.GetComponent<SpriteRenderer>().sprite = defaultBeard;
        // Body
        torsoObj.GetComponent<SpriteRenderer>().sprite = defaultBody;

        // HeadArmor
        if (headSlot.transform.childCount > 0 && headSlot.transform.GetChild(0).TryGetComponent(out Item item))
        {
            headArmor.sprite = item.sprite_equip;
            CheckVisibility(item);
        }
        else
            headArmor.sprite = null;
        // HeadEquipment
        if (headEquipmentSlot.transform.childCount > 0 && headEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            headEquipment.sprite = item.sprite_equip;
            CheckVisibility(item);
        }
        else
            headEquipment.sprite = null;
        // BodyArmor
        if (torsoSlot.transform.childCount > 0 && torsoSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyArmor.sprite = item.sprite_equip;
            CheckVisibility(item);
        }
        else
            bodyArmor.sprite = null;
        // BodyEquipment
        if (torsoEquipmentSlot.transform.childCount > 0 && torsoEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyEquipment.sprite = item.sprite_equip;
            CheckVisibility(item);
        }
        else
            bodyEquipment.sprite = null;
    }

    private void CheckVisibility(Item item)
    {
        if (item.hideHairWhenEquiped)
            hideHair = true;
        if (item.hideBeardWhenEquiped)
            hideBeard = true;
        if (item.hideBodyWhenEquiped)
            hideBody = true;
    }
}
