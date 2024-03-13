using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public int roomID;
    public GameObject previousRoom;
    public Vector2 boardPos;
    public Vector2 size;
    public bool[] entrances = new bool[4];  // Up, Right, Down, Left
    public List<GameObject> doors = new();  // Up, Right, Down, Left
    public List<GameObject> doorWalls = new();  // Up, Right, Down, Left

    public bool cleared;


    public void GetDoors()
    {
        doors.Add(transform.Find("Doors").GetChild(3).gameObject);
        doors.Add(transform.Find("Doors").GetChild(2).gameObject);
        doors.Add(transform.Find("Doors").GetChild(0).gameObject);
        doors.Add(transform.Find("Doors").GetChild(1).gameObject);

        doorWalls.Add(transform.Find("DoorWalls").GetChild(3).gameObject);
        doorWalls.Add(transform.Find("DoorWalls").GetChild(2).gameObject);
        doorWalls.Add(transform.Find("DoorWalls").GetChild(0).gameObject);
        doorWalls.Add(transform.Find("DoorWalls").GetChild(1).gameObject);

        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].SetActive(false);
            doorWalls[i].SetActive(true);
        }
    }
    public void AddDoors(int doorId, GameObject leadToRoom)
    {
        entrances[doorId] = true;
        doors[doorId].SetActive(entrances[doorId]);
        doors[doorId].GetComponent<Door>().doorId = doorId;
        doors[doorId].GetComponent<Door>().room = gameObject;
        doors[doorId].GetComponent<Door>().leadToRoom = leadToRoom;
        doorWalls[doorId].SetActive(!entrances[doorId]);
    }


    public void StartRoom()
    {
        // Short loading screen


        // Wake everything and everybody up
        gameObject.SetActive(true);

        cleared = true;

        for (int i = 0; i < 4; i++)
            doors[i].GetComponent<Door>().canInteract = cleared;
    }
}
