using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSkillButton : MonoBehaviour
{
    [SerializeField]
    private SkillDescription skillDescription;


    public void ButtonClicked() { skillDescription.UpgradeSkill(); }
}
