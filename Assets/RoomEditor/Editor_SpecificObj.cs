using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_SpecificObj : MonoBehaviour, IPointerClickHandler
{
    private Editor editor;
    private Image image;
    public string specificObjName;

    public void SetUp(string specificObjName, Editor editor)
    {
        this.specificObjName = specificObjName;
        this.editor = editor;
        image = GetComponent<Image>();
        GetComponentInChildren<TextMeshProUGUI>().text = specificObjName;
    }

    public void Unselect() { image.color = Color.white; }
    public void Select() { image.color = Color.gray; }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach(Editor_SpecificObj specificObj in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<Editor_SpecificObj>())
        {
            if(specificObj != this)
                specificObj.Unselect();
            else
            {
                Select();
                editor.specificObjName = specificObjName;
            }
        }
    }
}
