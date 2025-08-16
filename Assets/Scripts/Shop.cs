using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform shopEntrance;
    [SerializeField] private Door door;


    public void EnterShop()
    {
        Debug.Log("Entering the shop...");

        gameObject.SetActive(true);

        // Teleport player to the shop entrance
        player.transform.position = shopEntrance.position;

        door.canInteract = true;
        door.opened = false;
    }
}
