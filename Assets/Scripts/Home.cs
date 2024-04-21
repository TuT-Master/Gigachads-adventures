using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]
    private List<Door> doors;


    private void OnEnable()
    {
        foreach (Door door in doors)
        {
            door.canInteract = true;
            door.opened = false;
        }
    }

    private void OnDisable()
    {
        // Save game
    }
}
