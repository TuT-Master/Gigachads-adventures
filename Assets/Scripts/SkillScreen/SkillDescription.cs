using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PlayerStats;

public class SkillDescription : MonoBehaviour
{
    [HideInInspector]
    public LearnableSkill skill;
    private LearnableSkill tempSkill;

    [SerializeField]
    private TextMeshProUGUI skillName;
    [SerializeField]
    private TextMeshProUGUI skillDescription;
    [SerializeField]
    private TextMeshProUGUI skillStats;

    [SerializeField]
    private GameObject upgradeButton;
    [SerializeField]
    private GameObject cannotUpgradeSkill;

    private bool upgradeButtonVisible;

    [SerializeField]
    private PlayerStats playerStats;


    private void Update()
    {
        if (skill == null)
            return;

        if(skill.maxLevel || playerStats.playerStats[StatType.SkillPoints] <= 0)
        {
            upgradeButton.SetActive(false);
            cannotUpgradeSkill.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(true);
            cannotUpgradeSkill.SetActive(false);
        }
    }

    public void ShowSkillDetails(LearnableSkill skill)
    {
        tempSkill = skill;
        skillName.text = skill.skillName;
        skillDescription.text = skill.description;
        string fokinText = "";
        foreach (StatType key in skill.bonusStats.Keys)
        {
            if (skill.bonusStats[key] > 0)
                fokinText += key + ": + " + skill.bonusStats[key].ToString() + "%\n";
            else if (skill.bonusStats[key] < 0)
                fokinText += key + ": - " + Mathf.Abs(skill.bonusStats[key]).ToString() + "%\n";
        }
        skillStats.text = fokinText;
    }

    public void ShowChosenSkillDetails()
    {
        string fokinText = "";
        foreach (StatType key in skill.bonusStats.Keys)
        {
            if (skill.bonusStats[key] > 0)
                fokinText += key + ": + " + skill.bonusStats[key].ToString() + "%\n";
            else if (skill.bonusStats[key] < 0)
                fokinText += key + ": - " + Mathf.Abs(skill.bonusStats[key]).ToString() + "%\n";
        }
        skillStats.text = fokinText;
        skillName.text = skill.skillName;
        skillDescription.text = skill.description;
    }

    public void HideSkillDetails()
    {
        if (skill != null)
            ShowChosenSkillDetails();
        else
        {
            skillName.text = "";
            skillDescription.text = "";
            skillStats.text = "";
        }
    }

    public void UpgradeSkill()
    {
        skill.UpgradeSkill();
        ShowSkillDetails(skill);

        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
        playerStats.playerStats[StatType.SkillPoints]--;
        playerStats.UpdateSkillBonusStats();

        Debug.Log("Upgrading skill /" + skill.skillName + "/ to level " + skill.levelOfSkill.ToString());
    }
}
