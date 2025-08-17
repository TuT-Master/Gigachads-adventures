using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Door door;


    public void EnterShop()
    {
        // Teleport player to the shop entrance
        player.transform.position = door.transform.position;

        door.canInteract = true;
        door.opened = false;
    }
}
