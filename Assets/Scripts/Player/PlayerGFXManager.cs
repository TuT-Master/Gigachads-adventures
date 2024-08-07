using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

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
    private SpriteRenderer bodyRenderer;
    [SerializeField]
    private SpriteRenderer hairRenderer;
    [SerializeField]
    private SpriteRenderer beardRenderer;
    [SerializeField]
    private SpriteRenderer headArmor;
    [SerializeField]
    private SpriteRenderer headEquipment;
    [SerializeField]
    private SpriteRenderer bodyArmor;
    [SerializeField]
    private SpriteRenderer bodyEquipment;


    [SerializeField]
    private List<Sprite> playerSpritesFront;
    [SerializeField]
    private List<Sprite> playerSpritesBack;
    private Sprite GetDefaultSpriteByName(string name, bool front)
    {
        string _name = "";
        if (name.ToLower().Contains("back"))
            _name = name.Remove(name.Length - 4);
        else if (name.ToLower().Contains("front"))
            _name = name.Remove(name.Length - 5);
        if (front)
        {
            foreach (Sprite sprite in playerSpritesFront)
                if (sprite.name.Contains(_name))
                    return sprite;
        }
        else
        {
            foreach (Sprite sprite in playerSpritesBack)
                if (sprite.name.Contains(_name))
                    return sprite;
        }
        return null;
    }
    private Sprite GetSpriteByName(string itemName, bool front, bool male)
    {
        ItemDatabase itemDatabase = GetComponent<PlayerInventory>().itemDatabase;
        Item item = itemDatabase.GetItemByNameAndAmount(itemName, 1);
        if(male)
        {
            if (front)
                return item.sprite_equipMale_Front;
            else
                return item.sprite_equipMale_Back;
        }
        else
        {
            if (front)
                return item.sprite_equipMale_Front;
            else
                return item.sprite_equipMale_Back;
        }
    }
    private bool isTurned;

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
        if(playerMovement.turn)
        {
            // Player back pics
            hairObj.transform.localPosition = new(0, 0.55f, -0.001f);
            beardObj.transform.localPosition = new(0, 0.55f, -0.001f);
            headArmorObj.transform.localPosition = new(0, 0.55f, -0.001f);
            headEquipmentObj.transform.localPosition = new(0, 0.55f, -0.002f);
            torsoArmorObj.transform.localPosition = new(0, 0.55f, -0.001f);
            torsoEquipmentObj.transform.localPosition = new(0, 0.55f, -0.002f);
            secondaryWeaponObj.transform.localPosition = new(0, 0.55f, -0.003f);
            weaponObj.transform.localPosition = new(0, 0.55f, -0.004f);
        }
        else
        {
            // Player front pics
            hairObj.transform.localPosition = new(0, 0.55f, 0.001f);
            beardObj.transform.localPosition = new(0, 0.55f, 0.001f);
            headArmorObj.transform.localPosition = new(0, 0.55f, 0.001f);
            headEquipmentObj.transform.localPosition = new(0, 0.55f, 0.002f);
            torsoArmorObj.transform.localPosition = new(0, 0.55f, 0.001f);
            torsoEquipmentObj.transform.localPosition = new(0, 0.55f, 0.002f);
            secondaryWeaponObj.transform.localPosition = new(0, 0.55f, 0.003f);
            weaponObj.transform.localPosition = new(0, 0.55f, 0.004f);
        }

        // Rotate armor, weapon, shield, etc.
        if (playerMovement.turn != isTurned && defaultBody != null)
        {
            bool male = true;
            if (defaultBody.name.Contains("Female"))
                male = false;

            GameObject armorSlots = GetComponent<PlayerInventory>().armorSlots;
            GameObject equipmentSlots = GetComponent<PlayerInventory>().equipmentSlots;
            if (armorSlots.transform.Find("Head").childCount > 0 && armorSlots.transform.Find("Head").GetChild(0).TryGetComponent(out Item item))
                headArmor.sprite = GetSpriteByName(item.itemName, !isTurned, male);
            if (equipmentSlots.transform.Find("HeadEquipment").childCount > 0 && armorSlots.transform.Find("Head").GetChild(0).TryGetComponent(out item))
                headEquipment.sprite = GetSpriteByName(headEquipment.sprite.name, !isTurned, male);
            if (armorSlots.transform.Find("Torso").childCount > 0 && armorSlots.transform.Find("Head").GetChild(0).TryGetComponent(out item))
                bodyArmor.sprite = GetSpriteByName(bodyArmor.sprite.name, !isTurned, male);
            if (equipmentSlots.transform.Find("TorsoEquipment").childCount > 0 && armorSlots.transform.Find("Head").GetChild(0).TryGetComponent(out item))
                bodyEquipment.sprite = GetSpriteByName(bodyEquipment.sprite.name, !isTurned, male);

            isTurned = playerMovement.turn;
        }

        // Check visibility
        if (hideHair)
            hairObj.SetActive(false);
        else
        {
            hairObj.SetActive(true);
            if (defaultHair != null)
                hairRenderer.sprite = GetDefaultSpriteByName(defaultHair.name, !playerMovement.turn);
        }
        if (hideBeard)
            beardObj.SetActive(false);
        else
        {
            beardObj.SetActive(true);
            if (defaultBeard != null)
                beardRenderer.sprite = GetDefaultSpriteByName(defaultBeard.name, !playerMovement.turn);
        }
        if (hideBody)
            torsoObj.SetActive(false);
        else
        {
            torsoObj.SetActive(true);
            if (defaultBody != null)
                bodyRenderer.sprite = GetDefaultSpriteByName(defaultBody.name, !playerMovement.turn);
        }
    }

    public void UpdateGFX()
    {
        hideHair = false;
        hideBeard = false;
        hideBody = false;

        // Head
        hairRenderer.sprite = GetDefaultSpriteByName(defaultHair.name, !playerMovement.turn);
        // Beard
        beardRenderer.sprite = GetDefaultSpriteByName(defaultBeard.name, !playerMovement.turn);
        // Body
        bodyRenderer.sprite = GetDefaultSpriteByName(defaultBody.name, !playerMovement.turn);


        bool male = true;
        if (defaultBody.name.Contains("Female"))
            male = false;
        // HeadArmor
        if (headSlot.transform.childCount > 0 && headSlot.transform.GetChild(0).TryGetComponent(out Item item))
        {
            headArmor.sprite = GetSpriteByName(item.itemName, !isTurned, male);
            CheckVisibility(item);
        }
        else
            headArmor.sprite = null;
        // HeadEquipment
        if (headEquipmentSlot.transform.childCount > 0 && headEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            headEquipment.sprite = GetSpriteByName(item.itemName, !isTurned, male);
            CheckVisibility(item);
        }
        else
            headEquipment.sprite = null;
        // BodyArmor
        if (torsoSlot.transform.childCount > 0 && torsoSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyArmor.sprite = GetSpriteByName(item.itemName, !isTurned, male);
            CheckVisibility(item);
        }
        else
            bodyArmor.sprite = null;
        // BodyEquipment
        if (torsoEquipmentSlot.transform.childCount > 0 && torsoEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyEquipment.sprite = GetSpriteByName(item.itemName, !isTurned, male);
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
