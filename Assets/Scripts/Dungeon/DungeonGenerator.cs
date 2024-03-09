using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class DungeonRoom : MonoBehaviour
{
    public Vector2 size;
    public bool visited = false;
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
    }

    public Vector2 GetPosition() { return new(transform.position.x * 1.5f, transform.position.y * 1.5f); }
}

public class DungeonGenerator : MonoBehaviour
{
    public enum DungeonTile
    {
        None,
        Room,
        Hallway
    }

    public int age;

    public int maxRoomOffset;

    private List<GameObject> rooms = new();

    [SerializeField]
    private DungeonDatabase objDatabase;



    private void Start()
    {
        GenerateDungeon();
    }

    int RandomOddInt(int min, int max)
    {
        int result = 0;
        bool done = false;
        while (!done)
        {
            result = Random.Range(min, max);
            if ((result + 1) % 2 == 0)
                done = true;
        }
        return result;
    }

    public void GenerateDungeon()
    {
        // Set max dungeon size depending on age and difficulty level
        int dungeonMaxSize = 50;


        DungeonTile[,] board = new DungeonTile[dungeonMaxSize, dungeonMaxSize];

        // Placing rooms
        bool done = false;
        int roomWIP = 0;
        GameObject previousRoom = null;
        while (!done)
        {
            // Generate new room
            GameObject newRoom = GenerateRoom(new(RandomOddInt(5, 11), RandomOddInt(5, 11)));
            Vector3 startPos = new(dungeonMaxSize / 2, 0, 0);

            // Try to place room
            if (roomWIP == 0)
            {
                // Starting room
                newRoom.transform.position = startPos * 3;
            }
            else
            {
                // Calculate distance between centre and walls of rooms
                Vector2 centreDist = new(newRoom.GetComponent<DungeonRoom>().size.x * 1.5f + startPos.x, newRoom.GetComponent<DungeonRoom>().size.y * 1.5f);

                // Choose direction
                Vector3 addDist = new();
                switch(new System.Random().Next(0, 3))
                {
                    case 0:
                        // Up
                        addDist = new(0, 0, (previousRoom.GetComponent<DungeonRoom>().size.y * 1.5f) + centreDist.y + Random.Range(3, maxRoomOffset * 3));
                        break;
                    case 1:
                        // Down
                        addDist = new(0, 0, -((previousRoom.GetComponent<DungeonRoom>().size.y * 1.5f) + centreDist.y + Random.Range(3, maxRoomOffset * 3)));
                        break;
                    case 2:
                        // Right
                        addDist = new((previousRoom.GetComponent<DungeonRoom>().size.x * 1.5f) + centreDist.x + Random.Range(3, maxRoomOffset * 3), 0, 0);
                        break;
                    case 3:
                        // Left
                        addDist = new(-((previousRoom.GetComponent<DungeonRoom>().size.x * 1.5f) + centreDist.x + Random.Range(3, maxRoomOffset * 3)), 0, 0);
                        break;
                };
                // Check if there is enough space for the room


                // Place the room
                newRoom.transform.position = previousRoom.transform.position + addDist;
            }


            previousRoom = newRoom;
            roomWIP++;

            rooms.Add(newRoom);
            if (roomWIP == 5)
                done = true;
        }
    }

    public GameObject GenerateRoom(Vector2 size /* !Has to be odd numbers! */)
    {
        GameObject newRoom = new("Room-" + rooms.Count.ToString());
        newRoom.AddComponent<DungeonRoom>();
        newRoom.GetComponent<DungeonRoom>().size = size;

        GameObject doors = new("Doors");
        doors.transform.SetParent(newRoom.transform);

        GameObject doorWalls = new("DoorWalls");
        doorWalls.transform.SetParent(newRoom.transform);

        // Empty room
        Instantiate(objDatabase.floorMousePointer, new(0, 0, 0), Quaternion.identity, newRoom.transform);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Instantiate(objDatabase.floors[0], new(x * 3, 0, y * 3), Quaternion.identity, newRoom.transform);
                // Doors
                if(x == (size.x - 1) / 2)
                {
                    if(y == 0)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("DoorWalls"));
                    }
                    else if (y == size.y - 1)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("DoorWalls"));
                    }
                }
                else if(y == (size.y - 1) / 2)
                {
                    if (x == 0)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("DoorWalls"));
                    }
                    else if (x == size.x - 1)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform.Find("DoorWalls"));
                    }
                }
                else
                {
                    // Walls
                    if (x == 0)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform);
                    if (x == size.x - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform);
                    if (y == 0)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform);
                    if (y == size.y - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform);
                }
            }
        }
        newRoom.GetComponent<DungeonRoom>().GetDoors();

        // Populate room


        return newRoom;
    }
}
