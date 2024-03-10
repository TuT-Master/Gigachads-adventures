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

    bool TryAddRoom(Vector2 centrePos, Vector2 roomSize)
    {
        int x = (int)centrePos.x;
        int y = (int)centrePos.y;
        int sizeX = (int)(roomSize.x - 1) / 2;
        int sizeY = (int)(roomSize.y - 1) / 2;

        Dictionary<Vector2, Cell> tempBoard = board;
        bool canBePlaced = true;
        for(int i = -sizeY; i <= sizeY; i++)
        {
            for(int j = -sizeX; j <= sizeX; j++)
            {
                Vector2 id = new(x + j, y + i);
                if (board.ContainsKey(id) && board[id] == Cell.None)
                    tempBoard[id] = Cell.Room;
                else
                    canBePlaced = false;
            }
        }


        if (canBePlaced)
            board = tempBoard;
        else
            Debug.Log("Cannot be placed!");
        return canBePlaced;
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
        int roomWIP = 0;
        GameObject previousRoom = null;
        while (roomWIP != maxRoomCount)
        {
            // Generate new room
            GameObject newRoom = GenerateRoom(new(RandomOddInt(5, 11), RandomOddInt(5, 11)));
            

            // Try to place room
            if (roomWIP == 0)
            {
                // Starting room
                Vector2 startPos = new(dungeonMaxSize / 2, (newRoom.GetComponent<DungeonRoom>().size.y - 1) / 2);
                if(TryAddRoom(startPos, newRoom.GetComponent<DungeonRoom>().size))
                {
                    newRoom.transform.position = new(startPos.x * 3, 0, startPos.y * 3);
                    newRoom.GetComponent<DungeonRoom>().boardPos = new(newRoom.transform.position.x / 3, newRoom.transform.position.z / 3);
                }
            }
            else
            {
                // Calculate distance between centre and walls of rooms
                Vector2 centreToWallDist = new(newRoom.GetComponent<DungeonRoom>().size.x * 1.5f, newRoom.GetComponent<DungeonRoom>().size.y * 1.5f);


                // Choose direction
                System.Random rnd = new();
                int rndOffset = rnd.Next(1, maxRoomOffset);
                List<Vector2> directions = new()
                    {
                        // Right
                        new(0, rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x - 1) / 2) + ((newRoom.GetComponent<DungeonRoom>().size.x - 1) / 2)),
                        // Left
                        new(0, -(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x - 1) / 2) + ((newRoom.GetComponent<DungeonRoom>().size.x - 1) / 2))),
                        // Up
                        new(rndOffset + (previousRoom.GetComponent<DungeonRoom>().size.y - 1 / 2) + ((newRoom.GetComponent<DungeonRoom>().size.y - 1) / 2), 0),
                        // Down
                        new(-(rndOffset + (previousRoom.GetComponent<DungeonRoom>().size.y - 1 / 2) + ((newRoom.GetComponent<DungeonRoom>().size.y - 1) / 2)), 0)
                    };
                List<Vector2> directions3Dcorrection = new()
                    {
                        new((previousRoom.GetComponent<DungeonRoom>().size.x - newRoom.GetComponent<DungeonRoom>().size.x) / 2, 0),
                        new((previousRoom.GetComponent<DungeonRoom>().size.x - newRoom.GetComponent<DungeonRoom>().size.x) / 2, 0),
                        new(0, (previousRoom.GetComponent<DungeonRoom>().size.y - newRoom.GetComponent<DungeonRoom>().size.y) / 2),
                        new(0, (previousRoom.GetComponent<DungeonRoom>().size.y - newRoom.GetComponent<DungeonRoom>().size.y) / 2)
                    };
                Vector2 dir = new();
                Vector2 dirCorrection = new();
                bool dirChosen = false;
                Vector2 centrePos = new(previousRoom.GetComponent<DungeonRoom>().boardPos.x, previousRoom.GetComponent<DungeonRoom>().boardPos.y);
                
                while (!dirChosen)
                {
                    // Check if there is enough space for the room
                    if (directions.Count > 0)
                    {
                        rnd = new();
                        int dirTry = rnd.Next(0, directions.Count);
                        dir = directions[dirTry];
                        dirCorrection = directions3Dcorrection[dirTry];
                        if (TryAddRoom(centrePos + dir, newRoom.GetComponent<DungeonRoom>().size))
                            dirChosen = true;
                        else
                        {
                            Debug.Log("Trying to replace the room-" + roomWIP);
                            directions.Remove(dir);
                            directions3Dcorrection.Remove(dirCorrection);
                        }
                    }
                    else
                        break;
                }

                // If no direction selected -> go back and try it with previous room
                if(!dirChosen)
                {
                    Debug.Log("Going back to " + previousRoom.name);
                }

                // Place the room
                if(newRoom != null)
                {
                    newRoom.transform.position = previousRoom.transform.position + new Vector3(dir.x * 3, 0, dir.y * 3);
                    newRoom.GetComponent<DungeonRoom>().boardPos = new(newRoom.transform.position.x / 3, newRoom.transform.position.z / 3);
                    newRoom.transform.position += new Vector3(dirCorrection.x * 3, 0, dirCorrection.y * 3);
                }
            }


            newRoom.GetComponent<DungeonRoom>().previousRoom = previousRoom;
            previousRoom = newRoom;
            roomWIP++;
            rooms.Add(newRoom);
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
