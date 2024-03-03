using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject skillScreen;

    public bool skillScreenOpen;

    [SerializeField]
    private List<GameObject> tabs;

    private PlayerStats playerStats;
    private HUDmanager hudManager;

    [Header("Stat tab")]
    [SerializeField]
    private TextMeshProUGUI hpAmount;
    [SerializeField]
    private TextMeshProUGUI staminaAmount;
    [SerializeField]
    private TextMeshProUGUI manaAmount;

    // One handed dexterity
    Dictionary<string, float> whipStats = new()
    {
        {"bleedingChance", 0},
        {"range", 0},
        {"stunChance", 0}
    };
    Dictionary<string, float> daggerStats = new()
    {
        {"armorIngore", 0},
        {"attackSpeed", 0}
    };
    Dictionary<string, float> swordStats = new()
    {
        {"attackSpeed", 0},
        {"penetration", 0},
        {"bleedChance", 0}
    };
    Dictionary<string, float> rapierStats = new()
    {
        {"critChance", 0},
        {"range", 0}
    };
    Dictionary<string, float> oneHandedDexterityStats = new()
    {
        {"notConsumeStaminaChance", 0},
        {"staminaConsumtionReduction", 0},
        {"evade", 0},
        {"critChance", 0}
    };


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        hudManager = GetComponent<HUDmanager>();
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
        if (skillScreenOpen)
        {
            Time.timeScale = 0f;
            skillScreen.SetActive(true);
            OpenTab(0);
        }
        else
        {
            Time.timeScale = 1f;
            skillScreen.SetActive(false);
        }
    }

    public void StatButtonClicked() { OpenTab(0); }
    public void MeleeButtonClicked() { OpenTab(1); }
    public void RangedButtonClicked() { OpenTab(2); }
    public void MagicButtonClicked() { OpenTab(3); }

    private void OpenTab(int tabId)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabId)
                tabs[i].SetActive(true);
            else
                tabs[i].SetActive(false);
        }
        switch (tabId)
        {
            case 0:
                // Stat tab
                hpAmount.text = Mathf.Round(playerStats.playerStats["hp"]).ToString() + " / " + playerStats.playerStats["hpMax"].ToString();
                staminaAmount.text = Mathf.Round(playerStats.playerStats["stamina"]).ToString() + " / " + playerStats.playerStats["staminaMax"].ToString();
                manaAmount.text = Mathf.Round(playerStats.playerStats["mana"]).ToString() + " / " + playerStats.playerStats["manaMax"].ToString();
                break;
            case 1:
                // Melee tab

                break;
            case 2:
                // Ranged tab

                break;
            case 3:
                // Magic tab

                break;
        }
    }
}
