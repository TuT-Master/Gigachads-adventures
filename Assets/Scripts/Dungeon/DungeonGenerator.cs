using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

        for(int i = 0; i < doors.Count; i++)
        {
            doors[i].SetActive(false);
            doorWalls[i].SetActive(true);
        }
    }
}

public class DungeonGenerator : MonoBehaviour
{
    public enum Cell
    {
        None,
        Room,
        Hallway
    }

    public int age;

    public int maxRoomCount;

    public int maxRoomOffset;

    private List<GameObject> rooms = new();

    Dictionary<Vector2, Cell> board = new();

    [SerializeField]
    private DungeonDatabase objDatabase;



    private void Start()
    {
        GenerateDungeon(maxRoomCount);
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

    bool CanPlaceRoom(Vector2 centrePos, Vector2 roomSize)
    {
        int x = (int)centrePos.x;
        int y = (int)centrePos.y;
        int sizeX = (int)(roomSize.x + 1) / 2;
        int sizeY = (int)(roomSize.y + 1) / 2;

        Dictionary<Vector2, Cell> updatedBoard = board;
        for (int i = -sizeY; i <= sizeY; i++)
            for (int j = -sizeX; j <= sizeX; j++)
                if (!updatedBoard.ContainsKey(new(x + j, y + i)) || updatedBoard[new(x + j, y + i)] != Cell.None)
                    return false;
        return true;
    }

    void AddRoom(Vector2 startPos, Vector2 roomSize, out GameObject newRoom)
    {
        newRoom = GenerateRoom(roomSize);
        int x = (int)startPos.x;
        int y = (int)startPos.y;
        int sizeX = (int)(roomSize.x - 1) / 2;
        int sizeY = (int)(roomSize.y - 1) / 2;

        for(int i = -sizeY; i <= sizeY; i++)
            for (int j = -sizeX; j <= sizeX; j++)
                board[new(x + j, y + i)] = Cell.Room;
    }

    public void GenerateDungeon(int maxRoomCount)
    {
        // Set max dungeon size depending on age and difficulty level
        int dungeonMaxSize = 50;


        // Creating dungeon board
        for (int i = 0; i < dungeonMaxSize; i++)
            for (int j = 0; j < dungeonMaxSize; j++)
                board.Add(new Vector2(j, dungeonMaxSize - 1 - i), Cell.None);


        // Placing rooms
        GameObject previousRoom = null;
        Vector2 startPos = new();
        int currentRoom = 0;
        while (currentRoom < maxRoomCount)
        {
            // Generate new room
            Vector2 roomSize = new(RandomOddInt(5, 11), RandomOddInt(5, 11));
            GameObject newRoom = null;
            

            // Try to place room
            if (currentRoom == 0)
            {
                // Starting room
                startPos = new(dungeonMaxSize / 2, (roomSize.y + 1) / 2);
                AddRoom(startPos, roomSize, out newRoom);
                newRoom.GetComponent<DungeonRoom>().boardPos = startPos;

                newRoom.GetComponent<DungeonRoom>().previousRoom = previousRoom;
                previousRoom = newRoom;

                newRoom.SetActive(false);
                rooms.Add(newRoom);
            }
            else
            {
                startPos = previousRoom.GetComponent<DungeonRoom>().boardPos;

                // Choose direction
                System.Random rnd = new();
                int rndOffset = rnd.Next(1, maxRoomOffset);
                List<Vector2> directions = new()
                    {
                        // Right
                        new(0, rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2)),
                        // Left
                        new(0, -(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2))),
                        // Up
                        new(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2), 0),
                        // Down
                        new(-(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2)), 0)
                    };
                Vector2 dir = new();
                bool done = false;
                Debug.Log("Trying place room-" + currentRoom);
                while (!done)
                {
                    // Check if there is enough space for the room
                    if (directions.Count > 0)
                    {
                        rnd = new();
                        int dirTry = rnd.Next(0, directions.Count);
                        dir = directions[dirTry];
                        if (CanPlaceRoom(startPos + dir, roomSize))
                        {
                            done = true;
                            AddRoom(startPos + dir, roomSize, out newRoom);

                            Debug.Log("Placing " + newRoom.name);

                            newRoom.GetComponent<DungeonRoom>().boardPos = startPos + dir;

                            newRoom.GetComponent<DungeonRoom>().previousRoom = previousRoom;
                            previousRoom = newRoom;

                            newRoom.SetActive(false);
                            rooms.Add(newRoom);
                        }
                        else
                            directions.Remove(dir);
                    }
                    else
                    {
                        // If no direction selected -> go back and try it with previous room
                        Debug.Log("Going back to " + previousRoom.name);

                        newRoom = previousRoom;
                        previousRoom = newRoom.GetComponent<DungeonRoom>().previousRoom;

                        break;

                        if (previousRoom == newRoom)
                        {
                            Debug.Log("previousRoom == newRoom");
                            done = true;
                        }

                        if (previousRoom == null)
                            previousRoom = newRoom;

                        directions = new()
                        {
                            // Right
                            new(0, rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2)),
                            // Left
                            new(0, -(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2))),
                            // Up
                            new(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2), 0),
                            // Down
                            new(-(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2)), 0)
                        };
                    }
                }
            }
            currentRoom++;
        }
        FindObjectOfType<DungeonMap>().DrawMap(board);
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
