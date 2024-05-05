using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillConnection : MonoBehaviour
{
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



    public void UpdateSkillConnection()
    {
        
    }
}
