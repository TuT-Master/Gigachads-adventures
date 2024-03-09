using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonRoom : MonoBehaviour
{
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
        GenerateRoom(new(3, 5));
    }

    public void GenerateDungeon()
    {
        // Set max dungeon size depending on age and difficulty level

        int dungeonMaxSize = 10;
        DungeonTile[,] board = new DungeonTile[dungeonMaxSize, dungeonMaxSize];
        
    }

    public void GenerateRoom(Vector2 maxSize /* !Has to be non-even number! */)
    {
        GameObject newRoom = new("Room-" + rooms.Count.ToString());
        newRoom.AddComponent<DungeonRoom>();
        rooms.Add(newRoom);

        GameObject doors = new("Doors");
        doors.transform.SetParent(newRoom.transform);

        GameObject doorWalls = new("DoorWalls");
        doorWalls.transform.SetParent(newRoom.transform);

        Instantiate(objDatabase.floorMousePointer, new(0, 0, 0), Quaternion.identity, newRoom.transform);
        for (int y = 0; y < maxSize.y; y++)
        {
            for (int x = 0; x < maxSize.x; x++)
            {
                Instantiate(objDatabase.floors[0], new(x * 3, 0, y * 3), Quaternion.identity, newRoom.transform);
                // Doors
                if(x == (maxSize.x - 1) / 2)
                {
                    if(y == 0)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("DoorWalls"));
                    }
                    else if (y == maxSize.y - 1)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("DoorWalls"));
                    }
                }
                else if(y == (maxSize.y - 1) / 2)
                {
                    if (x == 0)
                    {
                        Instantiate(objDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("Doors"));
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("DoorWalls"));
                    }
                    else if (x == maxSize.x - 1)
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
                    if (x == maxSize.x - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform);
                    if (y == 0)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform);
                    if (y == maxSize.y - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform);
                }
            }
        }
        newRoom.GetComponent<DungeonRoom>().GetDoors();
    }
}
