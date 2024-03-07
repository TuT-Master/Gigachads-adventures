using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillDescription : MonoBehaviour
{
    private Skill skill;

    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillDescription;
    private TextMeshProUGUI skillStats;


    void Start()
    {
        skillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillDescription = transform.Find("SkillDescription").GetComponent<TextMeshProUGUI>();
        skillStats = transform.Find("SkillStats").GetComponent<TextMeshProUGUI>();
    }

    public void ShowSkillDetails(Skill skill)
    {
        this.skill = skill;
        Debug.Log("Show details");
    }

    public void HideSkillDetails()
    {
        Debug.Log("Hide details");
    }

    public void UpgradeSkill()
    {
        Debug.Log("Upgrading skill /" + skill.skillName + "/ to level " + (skill.levelOfSkill + 1).ToString());
    }
}
