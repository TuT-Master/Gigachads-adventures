using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillConnection : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private Skill previousSkill;
    [SerializeField]
    private Skill nextSkill;

    [SerializeField]
    private Image lineDefault;
    [SerializeField]
    private Image lineActive;
    [SerializeField]
    private Image arrowDefault;
    [SerializeField]
    private Image arrowActive;

    private string levelType;

    private void Start()
    {
        if(previousSkill == null | nextSkill == null)
        {
            Debug.Log(gameObject.name + " parameters previousSkill or nextSkill not set up!");
            return;
        }
        switch (previousSkill.weaponClass)
        {
            case PlayerStats.WeaponClass.OneHandDexterity:
                levelType = "level_oneHandDexterity";
                break;
            case PlayerStats.WeaponClass.OneHandStrenght:
                levelType = "level_oneHandStrenght";
                break;
            case PlayerStats.WeaponClass.TwoHandDexterity:
                levelType = "level_twoHandDexterity";
                break;
            case PlayerStats.WeaponClass.TwoHandStrenght:
                levelType = "level_twoHandStrenght";
                break;
            case PlayerStats.WeaponClass.RangeDexterity:
                levelType = "level_rangedDexterity";
                break;
            case PlayerStats.WeaponClass.RangeStrenght:
                levelType = "level_rangedStrenght";
                break;
            case PlayerStats.WeaponClass.Magic:
                levelType = "level_oneHandDexterity";
                break;
        }
    }

    public void UpdateSkillConnection()
    {
        if(previousSkill.levelUnlock[0] <= playerStats.playerStats[levelType] && playerStats.playerStats[levelType] <= nextSkill.levelUnlock[0])
            lineDefault.fillAmount = (playerStats.playerStats[levelType] - previousSkill.levelUnlock[0]) / (nextSkill.levelUnlock[0] - previousSkill.levelUnlock[0]);
    }
}
