using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;

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

    [HideInInspector] public List<GameObject> rooms = new();

    Dictionary<Vector2, Cell> board = new();

    [SerializeField]
    private DungeonDatabase objDatabase;


    private void Start()
    {
        GenerateDungeon(maxRoomCount);
        rooms[0].SetActive(true);
        FindAnyObjectByType<PlayerMovement>().transform.position = rooms[0].GetComponent<DungeonRoom>().doors[2].transform.position;
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
        // Apply doors to rooms (Up, Right, Down, Left)
        bool[] entrances = new bool[4] {false, false, false, false};
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
                    newRoom.GetComponent<DungeonRoom>().AddDoors(3, previousRoom);
                    previousRoom.GetComponent<DungeonRoom>().AddDoors(1, newRoom);
                }
                else
                {
                    for (int i = x; i < prevX; i++)
                        if (board[new(i, prevY)] == Cell.None)
                            board[new(i, prevY)] = Cell.Hallway;
                    newRoom.GetComponent<DungeonRoom>().AddDoors(1, previousRoom);
                    previousRoom.GetComponent<DungeonRoom>().AddDoors(3, newRoom);
                }
            }
            else if(prevY - y != 0)
            {
                if(y > prevY)
                {
                    for (int i = prevY; i < y; i++)
                        if (board[new(prevX, i)] == Cell.None)
                            board[new(prevX, i)] = Cell.Hallway;
                    newRoom.GetComponent<DungeonRoom>().AddDoors(2, previousRoom);
                    previousRoom.GetComponent<DungeonRoom>().AddDoors(0, newRoom);
                }
                else
                {
                    for (int i = y; i < prevY; i++)
                        if (board[new(prevX, i)] == Cell.None)
                            board[new(prevX, i)] = Cell.Hallway;
                    newRoom.GetComponent<DungeonRoom>().AddDoors(0, previousRoom);
                    previousRoom.GetComponent<DungeonRoom>().AddDoors(2, newRoom);
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
                startPos = new(dungeonMaxSize / 2, ((roomSize.y + 1) / 2) + 2);
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
                    startPos = new(dungeonMaxSize / 2, ((roomSize.y + 1) / 2) + 2);
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
                        if (startPos == new Vector2(dungeonMaxSize / 2, ((roomSize.y + 1) / 2) + 2))
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

        // Draw map
        FindObjectOfType<DungeonMap>().BuildMap(board, rooms);

        // Populate rooms
    }

    public GameObject GenerateRoom(Vector2 size /* !Has to be odd numbers! */)
    {
        GameObject newRoom = new("Room-" + rooms.Count.ToString());
        // Add DungeonRoom component
        newRoom.AddComponent<DungeonRoom>();
        newRoom.GetComponent<DungeonRoom>().size = size;
        newRoom.GetComponent<DungeonRoom>().roomID = rooms.Count;

        GameObject floor = new("Floor");
        floor.transform.SetParent(newRoom.transform);

        GameObject doors = new("Doors");
        doors.transform.SetParent(newRoom.transform);

        GameObject doorWalls = new("DoorWalls");
        doorWalls.transform.SetParent(newRoom.transform);

        GameObject walls = new("Walls");
        walls.transform.SetParent(newRoom.transform);

        // Empty room
        Instantiate(objDatabase.floorMousePointer, new(0, 0, 0), Quaternion.identity, floor.transform);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Instantiate(objDatabase.floors[0], new(x * 3, 0, y * 3), Quaternion.identity, floor.transform);
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
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), walls.transform);
                    if (x == size.x - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), walls.transform);
                    if (y == 0)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), walls.transform);
                    if (y == size.y - 1)
                        Instantiate(objDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), walls.transform);
                }
            }
        }
        newRoom.GetComponent<DungeonRoom>().GetDoors();

        // Populate
        PopulateRoom(newRoom);


        return newRoom;
    }

    void PopulateRoom(GameObject room)
    {
        Dictionary<Vector2, GameObject> pop = new();


        // Add obstacles
        System.Random random;
        for (int y = 0; y < room.GetComponent<DungeonRoom>().size.y * 3; y++)
        {
            for (int x = 0; x < room.GetComponent<DungeonRoom>().size.x * 3; x++)
            {
                random = new();
                switch (random.Next(0, 100))
                {
                    case 0 | 1:
                        pop.Add(new(x, y), objDatabase.obstacles[0]);
                        break;
                    case 2 | 3:
                        pop.Add(new(x, y), objDatabase.resources[0]);
                        break;
                    case 4 | 5:
                        pop.Add(new(x, y), objDatabase.lootBoxes[0]);
                        break;
                    case 99:
                        if(rooms.Count > 0)
                            pop.Add(new(x, y), objDatabase.meleeEnemies[0]);
                        break;
                    default:
                        pop.Add(new(x, y), null);
                        break;
                }
            }
        }

        // Add collectable resources


        // Add loot boxes


        // Populate room with enemies





        // Spawn population
        GameObject population = new("Population");
        population.transform.parent = room.transform;
        room.GetComponent<DungeonRoom>().enemiesCount = 0;
        room.GetComponent<DungeonRoom>().population = pop;
        foreach (Vector2 id in room.GetComponent<DungeonRoom>().population.Keys)
        {
            if (pop[id] != null)
            {
                Instantiate(pop[id], new Vector3(id.x, 0, id.y), Quaternion.identity, population.transform);
                if (pop[id].TryGetComponent(out EnemyStats enemy))
                    room.GetComponent<DungeonRoom>().enemiesCount++;
            }
        }

        // Build NavMeshSurface
        for (int i = 1; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshSurface surface = room.AddComponent<NavMeshSurface>();
            surface.agentTypeID = NavMesh.GetSettingsByIndex(i).agentTypeID;
            surface.BuildNavMesh();
        }
    }
}
