using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowChosenSkill : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private SkillDescription skillDescription;

    public void OnPointerDown(PointerEventData eventData)
    {
        skillDescription.skill = null;
        skillDescription.HideSkillDetails();
    }
}
