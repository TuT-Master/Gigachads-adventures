using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillDescription : MonoBehaviour
{
    private Skill skill;

    [SerializeField]
    private TextMeshProUGUI skillName;
    [SerializeField]
    private TextMeshProUGUI skillDescription;
    [SerializeField]
    private TextMeshProUGUI skillStats;


    public void ShowSkillDetails(Skill skill)
    {
        this.skill = skill;
        skillName.text = skill.skillName;
        skillDescription.text = skill.description;
        string fokinText = "";
        foreach (string key in skill.bonusStats.Keys)
        {
            if (skill.bonusStats[key] > 0)
                fokinText += key + ": + " + skill.bonusStats[key].ToString() + "%\n";
            else if (skill.bonusStats[key] < 0)
                fokinText += key + ": - " + Mathf.Abs(skill.bonusStats[key]).ToString() + "%\n";
        }
        skillStats.text = fokinText;
    }

    public void HideSkillDetails()
    {
        skillName.text = "";
        skillDescription.text = "";
        skillStats.text = "";
    }

    public void UpgradeSkill()
    {
        skill.UpgradeSkill();
        ShowSkillDetails(skill);
        Debug.Log("Upgrading skill /" + skill.skillName + "/ to level " + skill.levelOfSkill.ToString());
    }
}
