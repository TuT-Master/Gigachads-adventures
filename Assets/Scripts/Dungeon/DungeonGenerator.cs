using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public enum Cell
    {
        None,
        Room,
        Hallway
    }

    public int age;

    public int boardSize;

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

    void AddRoom(Vector2 startPos, Vector2 roomSize, GameObject previousRoom, out GameObject newRoom)
    {
        // Get new room
        newRoom = GenerateRoom(roomSize);

        // Add room to board
        int x = (int)startPos.x;
        int y = (int)startPos.y;
        int sizeX = (int)(roomSize.x - 1) / 2;
        int sizeY = (int)(roomSize.y - 1) / 2;

        for(int i = -sizeY; i <= sizeY; i++)
            for (int j = -sizeX; j <= sizeX; j++)
                board[new(x + j, y + i)] = Cell.Room;

        // Add hallway to board
        if(previousRoom != null)
        {
            int prevX = (int)previousRoom.GetComponent<DungeonRoom>().boardPos.x;
            int prevY = (int)previousRoom.GetComponent<DungeonRoom>().boardPos.y;

            if(prevX - x != 0)
            {
                if(x > prevX)
                {
                    for (int i = prevX; i < x; i++)
                        if (board[new(i, prevY)] == Cell.None)
                            board[new(i, prevY)] = Cell.Hallway;
                }
                else
                {
                    for (int i = x; i < prevX; i++)
                        if (board[new(i, prevY)] == Cell.None)
                            board[new(i, prevY)] = Cell.Hallway;
                }
            }
            else if(prevY - y != 0)
            {
                if(y > prevY)
                {
                    for (int i = prevY; i < y; i++)
                        if (board[new(prevX, i)] == Cell.None)
                            board[new(prevX, i)] = Cell.Hallway;
                }
                else
                {
                    for (int i = y; i < prevY; i++)
                        if (board[new(prevX, i)] == Cell.None)
                            board[new(prevX, i)] = Cell.Hallway;
                }
            }
        }
    }

    public void GenerateDungeon(int maxRoomCount)
    {
        // Set max dungeon size depending on age and difficulty level
        int dungeonMaxSize = boardSize;

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
                AddRoom(startPos, roomSize, previousRoom, out newRoom);
                newRoom.GetComponent<DungeonRoom>().boardPos = startPos;

                newRoom.GetComponent<DungeonRoom>().previousRoom = previousRoom;
                previousRoom = newRoom;

                newRoom.SetActive(false);
                rooms.Add(newRoom);
                currentRoom++;
            }
            else
            {
                System.Random rnd = new();
                int rndOffset = rnd.Next(1, maxRoomOffset);
                List<Vector2> directions = new();
                if (previousRoom != null)
                {
                    startPos = previousRoom.GetComponent<DungeonRoom>().boardPos;
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
                else
                {
                    startPos = new(dungeonMaxSize / 2, (roomSize.y + 1) / 2);
                    directions = new()
                    {
                        // Right
                        new(0, rndOffset + ((rooms[0].GetComponent<DungeonRoom>().size.x + roomSize.x) / 2)),
                        // Left
                        new(0, -(rndOffset + ((rooms[0].GetComponent<DungeonRoom>().size.x + roomSize.x) / 2))),
                        // Up
                        new(rndOffset + ((rooms[0].GetComponent<DungeonRoom>().size.y + roomSize.y) / 2), 0),
                        // Down
                        new(-(rndOffset + ((rooms[0].GetComponent<DungeonRoom>().size.y + roomSize.y) / 2)), 0)
                    };
                }

                // Choose direction
                Vector2 dir = new();
                bool done = false;
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
                            AddRoom(startPos + dir, roomSize, previousRoom, out newRoom);

                            newRoom.GetComponent<DungeonRoom>().boardPos = startPos + dir;

                            newRoom.GetComponent<DungeonRoom>().previousRoom = previousRoom;
                            previousRoom = newRoom;

                            newRoom.SetActive(false);
                            rooms.Add(newRoom);
                            currentRoom++;
                            done = true;
                        }
                        else
                            directions.Remove(dir);
                    }
                    else
                    {
                        // If no direction selected -> go back and try it with previous room
                        if (startPos == new Vector2(dungeonMaxSize / 2, (roomSize.y + 1) / 2))
                            currentRoom = maxRoomCount;
                        else
                        {
                            newRoom = previousRoom;
                            previousRoom = newRoom.GetComponent<DungeonRoom>().previousRoom;
                        }
                        done = true;
                    }
                }
            }
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
