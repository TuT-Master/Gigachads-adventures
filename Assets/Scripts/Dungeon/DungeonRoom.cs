using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public GameObject previousRoom;
    public Vector2 boardPos;
    public Vector2 size;
    public bool[] entrances = new bool[4];  // Up, Right, Down, Left
    public List<GameObject> doors = new();  // Up, Right, Down, Left
    public List<GameObject> doorWalls = new();  // Up, Right, Down, Left

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

    public void AddDoors(int doorId)
    {
        entrances[doorId] = true;
        doors[doorId].SetActive(entrances[doorId]);
        doorWalls[doorId].SetActive(!entrances[doorId]);
    }
}
