using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;

public class PlayerSkill : MonoBehaviour
{
    public enum SkillType
    {
        Passive,
        Active
    }
    public Dictionary<Item.WeaponType, Dictionary<SkillType, int>> playerWeaponTypeSkillLevels = new()
    {
        // One handed
        { Item.WeaponType.Whip, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Dagger, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Sword, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Rapier, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.LightShield, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.WeaponType.Axe, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Mace, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Hammer_oneHanded, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.HeavyShield, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Two handed
        { Item.WeaponType.QuarterStaff, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Spear, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Longsword, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.WeaponType.Halbert, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Hammer_twoHanded, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Zweihander, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Ranged
        { Item.WeaponType.Bow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.SMG, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Pistol, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.AttackRifle, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Thrower, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.WeaponType.Longbow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Crossbow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Shotgun, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Revolver, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Machinegun, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.SniperRifle, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.Launcher, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Magic
        { Item.WeaponType.MagicWeapon_fire, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.MagicWeapon_water, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.MagicWeapon_earth, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.WeaponType.MagicWeapon_air, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
    };

    public bool skillScreenOpen;

    [SerializeField]
    private CategoryButton[] categoryButtons;

    [SerializeField]
    private GameObject skillScreen;

    [SerializeField]
    private SkillDescription skillDescription;

    [SerializeField]
    private List<GameObject> tabs;

    private HUDmanager hudManager;

    private List<SkillConnection> skillConnections;


    void Start()
    {
        hudManager = GetComponent<HUDmanager>();
        skillConnections = FindObjectsOfType<SkillConnection>().ToList();
        skillScreenOpen = false;
        OpenTab(0);
        ToggleSkillScreen(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Toggle skillScreen"))
            hudManager.ToggleSkillScreen(!skillScreenOpen);
    }

    public void ToggleSkillScreen(bool toggle)
    {
        skillScreenOpen = toggle;
        skillScreen.SetActive(skillScreenOpen);
        if (skillScreenOpen)
        {
            Time.timeScale = 0f;
            OpenTab(0);
            foreach(SkillConnection connection in skillConnections)
                connection.UpdateSkillConnection();
            categoryButtons[0].clicked = true;
            categoryButtons[1].clicked = categoryButtons[2].clicked = categoryButtons[3].clicked = false;
        }
        else
            Time.timeScale = 1f;
    }
    public void StatButtonClicked(){ OpenTab(0); }
    public void MeleeButtonClicked() { OpenTab(1); }
    public void RangedButtonClicked() { OpenTab(2); }
    public void MagicButtonClicked() { OpenTab(3); }
    private void OpenTab(int tabId)
    {

        skillDescription.HideSkillDetails();

        if (tabId == 0)
            skillDescription.gameObject.SetActive(false);
        else
            skillDescription.gameObject.SetActive(true);

        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabId)
            {
                categoryButtons[i].clicked = true;
                tabs[i].SetActive(true);
            }
            else
            {
                categoryButtons[i].clicked = false;
                tabs[i].SetActive(false);
            }
        }
    }
}
