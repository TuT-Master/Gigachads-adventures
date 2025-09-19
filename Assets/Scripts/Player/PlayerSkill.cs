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
    public Dictionary<Item.ItemType, Dictionary<SkillType, int>> playerWeaponTypeSkillLevels = new()
    {
        // One handed
        { Item.ItemType.Whip, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Dagger, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Sword, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Rapier, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.LightShield, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.ItemType.Axe, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Mace, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Hammer_oneHanded, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.HeavyShield, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Two handed
        { Item.ItemType.QuarterStaff, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Spear, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Longsword, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.ItemType.Halbert, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Hammer_twoHanded, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Zweihander, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Ranged
        { Item.ItemType.Bow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.SMG, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Pistol, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.AttackRifle, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Thrower, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },

        { Item.ItemType.Longbow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Crossbow, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Shotgun, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Revolver, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Machinegun, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.SniperRifle, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.Launcher, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        // Magic
        { Item.ItemType.MagicWeapon_fire, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.MagicWeapon_water, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.MagicWeapon_earth, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
        { Item.ItemType.MagicWeapon_air, new(){ {SkillType.Passive, 0}, {SkillType.Active, 0} } },
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

    private List<LearnableSkillConnection> skillConnections;


    void Start()
    {
        hudManager = GetComponent<HUDmanager>();
        skillConnections = FindObjectsOfType<LearnableSkillConnection>().ToList();
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
            foreach(LearnableSkillConnection connection in skillConnections)
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
        foreach (StatPrefab statPrefab in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<StatPrefab>())
            statPrefab.UpdateStat();
        foreach (StatWithFillBarPrefab statPrefab in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<StatWithFillBarPrefab>())
            statPrefab.UpdateStat();
    }
}
