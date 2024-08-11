using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_CustomDoor : MonoBehaviour, IPointerClickHandler
{
    private Image door;
    [SerializeField] private Image checkmark;

    [SerializeField] private Sprite state0;
    [SerializeField] private Sprite state1;
    [SerializeField] private Sprite state2;


    // If doors has to be there (0 - doesn't matter, 1 - has to be there, 2 - can't be there)
    public int doorState;
    public int id;


    void Start()
    {
        door = GetComponent<Image>();
        doorState = 0;
    }
    void Update()
    {
        checkmark.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        doorState++;
        if (doorState > 2)
            doorState = 0;
        else if (doorState < 0)
            doorState = 0;
        
        switch(doorState)
        {
            case 0:
                door.color = Color.green;
                checkmark.sprite = state0;
                break;
            case 1:
                door.color = Color.white;
                checkmark.sprite = state1;
                break;
            case 2:
                door.color = Color.black;
                checkmark.sprite = state2;
                break;
        }
    }
}
