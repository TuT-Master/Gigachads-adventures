using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    // Global stuff
    [Header("Global")]
    public int clearedDungeonCounter;
    public int maxRoomCount;
    public int age;
    public List<GameObject> rooms = new();
    [SerializeField] private Transform dungeonScene;
    [SerializeField] private DungeonDatabase dungeonDatabase;
    [SerializeField] private ItemDatabase itemDatabase;


    private void Start()
    {
        dungeonDatabase.LoadRooms();
        if (testRoom != null)
            BuildTestRoom();

        // Testing only
        BuildDungeon();
    }
    public void CreateDungeon()
    {
        if (clearedDungeonCounter % 3 == 2)
            BuildCave();
        else
            BuildDungeon();
    }

    private GameObject GetNewRoom(Editor_Room room, int id)
    {
        return null;
    }
    private GameObject GetTestRoom(UnityEngine.Object roomFile)
    {
        Editor_Room room = null;

        #region Load from file
        string fullPath = Path.GetFullPath(AssetDatabase.GetAssetPath(roomFile));
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using StreamReader read = new(stream);
                    dataToLoad = read.ReadToEnd();
                }
                room = JsonUtility.FromJson<Editor_Room>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file " + fullPath + "\n" + e);
            }
        }
        if(room == null)
            return null;
        #endregion

        GameObject roomResult = new("TestRoom");
        // Spawn floor and ceiling
        Instantiate(dungeonDatabase.GetFloorBySize(room.roomSize), Vector3.zero, Quaternion.identity, roomResult.transform).name = "Floor";
        Instantiate(dungeonDatabase.GetCeilingBySize(room.roomSize), new(0, 3, 0), Quaternion.identity, roomResult.transform).name = "Ceiling";

        // Spawn walls        
        // Bottom
        bool[] doors = GetDoorsInWall(room, DoorSide.Bottom);
        GameObject wall = Instantiate(
            dungeonDatabase.GetWallBySizeAndDoors((int)room.roomSize.x, doors),
            new(0, 0, (room.roomSize.y - 1) * -4.5f),
            Quaternion.Euler(0, 180, 0),
            roomResult.transform);
        wall.name = "Wall_Bottom";
        // Right
        doors = GetDoorsInWall(room, DoorSide.Right);
        wall = Instantiate(
            dungeonDatabase.GetWallBySizeAndDoors((int)room.roomSize.x, doors),
            new((room.roomSize.x - 1) * 4.5f, 0, 0),
            Quaternion.Euler(0, 90, 0),
            roomResult.transform);
        wall.name = "Wall_Right";
        // Upper
        doors = GetDoorsInWall(room, DoorSide.Top);
        wall = Instantiate(
            dungeonDatabase.GetWallBySizeAndDoors((int)room.roomSize.x, doors),
            new(0, 0, (room.roomSize.y - 1) * 4.5f),
            Quaternion.Euler(0, 0, 0),
            roomResult.transform);
        wall.name = "Wall_Upper";
        // Left
        doors = GetDoorsInWall(room, DoorSide.Left);
        wall = Instantiate(
            dungeonDatabase.GetWallBySizeAndDoors((int)room.roomSize.x, doors),
            new((room.roomSize.x - 1) * -4.5f, 0, 0),
            Quaternion.Euler(0, -90, 0),
            roomResult.transform);
        wall.name = "Wall_Left";

        // Populate room
        PopulateRoom(room, roomResult.transform);

        return roomResult;
    }
    private void PopulateRoom(Editor_Room room, Transform roomParent)
    {
        for (int y = 0;  y < room.roomSize.y * 6; y++)
        {
            for (int x = 0; x < room.roomSize.x * 6; x++)
            {
                int tileID = (int)(y * room.roomSize.x * 6) + x;
                if (room.tiles[tileID] == null || room.tiles[tileID] == "")
                    continue;

                GameObject newGo = null;

                if (int.TryParse(room.tiles[tileID], out int tileType) && tileType != 0)
                    newGo = Instantiate(dungeonDatabase.GetRandomPop(tileType), roomParent);
                else if (!int.TryParse(room.tiles[tileID], out tileType))
                    newGo = Instantiate(dungeonDatabase.GetPopByName(room.tiles[tileID]), roomParent);
                else
                    continue;

                newGo.transform.SetLocalPositionAndRotation(new((1.5f * x) - (room.roomSize.x * 4.5f) + 0.75f, 0, (1.5f * y) - (room.roomSize.y * 4.5f) + 0.75f), Quaternion.Euler(0, new System.Random().Next(360), 0));
            }
        }
    }

    public enum DoorSide
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }
    private bool[] GetDoorsInWall(Editor_Room room, DoorSide side)
    {
        bool[] doors;
        if(side == DoorSide.Top || side == DoorSide.Bottom)
            doors = new bool[(int)room.roomSize.x];
        else
            doors = new bool[(int)room.roomSize.y];
        List<int> doorsIds = GetDoorsIDs(room, side);
        for (int i = 0; i < doorsIds.Count; i++)
            if (room.doorsSave[doorsIds[i]] != 2)
                doors[i] = true;
        return doors;
    }
    private List<int> GetDoorsIDs(Editor_Room room, DoorSide side)
    {
        int wallX = (int)room.roomSize.x;
        int wallY = (int)room.roomSize.y;
        List<int> doors = new();
        switch (side)
        {
            case DoorSide.Bottom:
                for(int i = 0; i < wallX; i++)
                    doors.Add(i);
                break;
            case DoorSide.Right:
                for (int i = 0; i < wallY; i++)
                    doors.Add(i + wallX);
                break;
            case DoorSide.Top:
                for (int i = 0; i < wallX; i++)
                    doors.Add(i + wallY + wallX);
                break;
            case DoorSide.Left:
                for (int i = 0; i < wallY; i++)
                    doors.Add(i + (wallX * 2) + wallY);
                break;
        }
        return doors;
    }



    // Test room
    [SerializeField] private UnityEngine.Object testRoom;

    private void BuildTestRoom()
    {
        GameObject _testRoom = Instantiate(GetTestRoom(testRoom), dungeonScene);
        _testRoom.SetActive(true);
    }

    // Minimap
    [Header("Minimap")]
    [SerializeField] private Transform minimapTransform;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject doorPrefab;


    // New dungeon generator
    [Header("Dungeon")]
    // Maximální velikost roomky
    public Vector2 maxRoomSize;

    // Maximální distance (poèet cyklù / vzdálenost v roomkách od start roomky)
    public int maxDistance;

    // Minimální poèet roomek v jednom cyklu (distance)
    public int minRoomCountPerDistance;

    // Maximální poèet roomek v jednom cyklu (distance)
    public int maxRoomCountPerDistance;

    // Maximální poèet roomek, co vedou z jedné roomky
    public int maxNewRoomsPerRoom;

    // Šance na spawnutí roomky v požadovaném smìru (v procentech)
    public float roomDirectionBias_spawnChancePercentage;

    // Šance na spawnutí roomky mimo požadovaný smìr (v procentech)
    public float roomNoDirectionBias_spawnChancePercentage;

    // Šance na branchování (v procentech)
    public float roomBranching_ChancePercentage;

    // Maximální poèet roomek bez vytvoøení nové vìtve
    public float maxRoomsWithoutNewBranchesCount;

    // Šance na spawnutí resourcefull roomky
    public float resourceRoom_spawnChancePercentage;


    private List<VirtualDungeonRoom> virtualRooms = new();
    private Dictionary<Vector2, VirtualDungeonRoom> virtualBoard = new();
    private VirtualDungeonRoom startRoom;

    public void BuildDungeon()
    {
        // Generate dungeon
        GenerateDungeon();
        // Activate starting room
        //rooms[0].SetActive(true);
        // Teleport player to starting room

    }

    private void GenerateDungeon()
    {
        Debug.Log("Starting generating dungeon...");
        // Generate virtual board
        virtualRooms.Clear();
        virtualBoard.Clear();
        for (int y = 0; y < ((maxDistance + 2) * maxRoomSize.y); y++)
            for (int x = 0; x < ((maxDistance + 2) * maxRoomSize.x * 2); x++)
                virtualBoard.Add(new(x, y), null);

        int roomCount = 0;
        int currentDistance = 0;
        bool maxRoomCountReached = false;
        bool maxDistanceReached = false;

        while (!maxRoomCountReached)
        {
            if(!maxDistanceReached)
            {
                if (roomCount == 0)
                {
                    // Starting room
                    Editor_Room _startRoom = dungeonDatabase.GetRoomByType(Editor_Room.RoomType.Start);
                    List<Vector2> tiles = new();
                    for (int y = 0; y < _startRoom.roomSize.y; y++)
                        for (int x = (int)(maxDistance * maxRoomSize.x); x < (int)(maxDistance * maxRoomSize.x) + _startRoom.roomSize.x; x++)
                            tiles.Add(new Vector2(x, y));
                    startRoom = new(currentDistance, tiles, _startRoom, Vector2.zero, roomCount);
                    startRoom.startDoor = startRoom.doors[new System.Random().Next((int)startRoom.room.roomSize.x)];
                    startRoom.canCreateNewBranches = true;

                    // Set random direction bias
                    switch(new System.Random().Next(3))
                    {
                        case 0:
                            startRoom.dirBias = Vector2.up;
                            break;
                        case 1:
                            startRoom.dirBias = Vector2.right;
                            break;
                        case 2:
                            startRoom.dirBias = Vector2.left;
                            break;
                    }

                    virtualRooms.Add(startRoom);
                    // Place it in the board
                    AddRoomToBoard(startRoom);
                    UpdateDoors();
                    roomCount++;
                }
                else
                {
                    // Generating rooms in one cycle
                    int roomCountPerCycle = 0;
                    bool cycleDone = false;
                    int _currentDistance = currentDistance;
                    while (!cycleDone)
                    {
                        List<VirtualDungeonRoom> currentRooms = new();
                        for (int i = 0; i < virtualRooms.Count; i++)
                            if (virtualRooms[i].distance == _currentDistance - 1 && virtualRooms[i].neighbourRooms.Count < maxNewRoomsPerRoom)
                                currentRooms.Add(virtualRooms[i]);

                        int freeDoorsTotal = 0;
                        if(currentRooms.Count != 0)
                            foreach (VirtualDungeonRoom room in currentRooms)
                                foreach (VirtualDoor door in room.doors)
                                    if(door.leadToDoor == null)
                                        freeDoorsTotal++;

                        if (currentRooms.Count == 0 || freeDoorsTotal == 0)
                            _currentDistance = GetLoweredDistance(_currentDistance);

                        if (_currentDistance < 0)
                            cycleDone = true;

                        for (int i = 0; i < currentRooms.Count; i++)
                        {
                            VirtualDungeonRoom room = currentRooms[new System.Random().Next(currentRooms.Count)];
                            List<VirtualDoor> freeDoorsRef = room.doors;

                            // Creating new branches
                            if ((new System.Random().Next(10001) / 100f) + (100 * room.noBranchCount / maxRoomsWithoutNewBranchesCount) <= roomBranching_ChancePercentage)
                            {
                                room.canCreateNewBranches = true;
                                room.noBranchCount = 0;
                            }
                            else
                                room.noBranchCount++;

                            for (int j = 0; j < room.doors.Count; j++)
                            {
                                int randomId = new System.Random().Next(freeDoorsRef.Count);
                                VirtualDoor door = freeDoorsRef[randomId];

                                // Roll for room to spawn
                                float randomRoll = new System.Random().Next(10001) / 100f;
                                if ((door.side == room.dirBias && randomRoll <= roomDirectionBias_spawnChancePercentage) ||
                                    (door.side != room.dirBias && room.canCreateNewBranches && randomRoll <= roomNoDirectionBias_spawnChancePercentage))
                                {
                                    // Choose type of room
                                    Editor_Room.RoomType roomType = Editor_Room.RoomType.Basic;
                                    if (new System.Random().Next(10001) / 100f < resourceRoom_spawnChancePercentage)
                                        roomType = Editor_Room.RoomType.Resources;

                                    // Try place the room
                                    Editor_Room newRoom = dungeonDatabase.GetRoomByType(roomType);
                                    List<Vector2> roomTiles = new();
                                    for (int y = 0; y < newRoom.roomSize.y; y++)
                                        for (int x = 0; x < newRoom.roomSize.x; x++)
                                            roomTiles.Add(new Vector2(door.pos.x + x, door.pos.y + y));

                                    if (CanPlaceRoom(roomTiles))
                                    {
                                        // Placing new room
                                        VirtualDungeonRoom roomResult = new(currentDistance, roomTiles, newRoom, door.side, roomCount);
                                        room.doors[randomId].isOccupied = true;
                                        room.neighbourRooms.Add(roomResult);
                                        AddRoomToBoard(roomResult);
                                        UpdateDoors();

                                        virtualRooms.Add(roomResult);
                                        roomCount++;
                                        roomCountPerCycle++;

                                        UpdateDistances();

                                        if (roomCountPerCycle >= maxRoomCountPerDistance)
                                        {
                                            cycleDone = true;
                                            break;
                                        }
                                    }
                                }
                                freeDoorsRef.Remove(door);
                            }

                            currentRooms.Remove(room);
                        }
                    }
                }
                currentDistance++;
                if (currentDistance >= maxDistance)
                    maxDistanceReached = true;
            }
            else
            {
                currentDistance = 0;
                roomCount = 0;
                maxDistanceReached = false;
                virtualRooms = new();
                virtualBoard.Clear();
                for (int y = 0; y < ((maxDistance + 2) * maxRoomSize.y); y++)
                    for (int x = 0; x < ((maxDistance + 2) * maxRoomSize.x * 2); x++)
                        virtualBoard.Add(new(x, y), null);
                Debug.Log("Another try...");
            }

            if (roomCount >= maxRoomCount)
                maxRoomCountReached = true;
        }
        FinishDoors();
        Debug.Log("Dungeon generation done!");

        // Create minimap
        DrawMinimap();

        // Physicaly spawn rooms

    }

    // F-ing recursive function YEEAAH!! xD
    private int GetLoweredDistance(int _currentDistance)
    {
        _currentDistance--;
        List<VirtualDungeonRoom> currentRooms = new();
        for (int i = 0; i < virtualRooms.Count; i++)
            if (virtualRooms[i].distance == _currentDistance && virtualRooms[i].neighbourRooms.Count < maxNewRoomsPerRoom)
                currentRooms.Add(virtualRooms[i]);

        int freeDoorsTotal = 0;
        if (currentRooms.Count != 0)
            foreach (VirtualDungeonRoom room in currentRooms)
                foreach (VirtualDoor door in room.doors)
                    if (door.leadToDoor == null)
                        freeDoorsTotal++;
        if (currentRooms.Count > 0 && freeDoorsTotal > 0)
            return _currentDistance;
        else if (_currentDistance < 0)
            return -1;
        else
            return GetLoweredDistance(_currentDistance);
    }

    private void UpdateDistances()
    {
        /*foreach(VirtualDungeonRoom room in virtualRooms)
        {
            int lowestDistance = room.distance;
            foreach(VirtualDungeonRoom neighbor in room.neighbourRooms)
                if(neighbor.distance + 1 < lowestDistance)
                    lowestDistance = neighbor.distance;
            room.distance = lowestDistance;
        }*/
    }
    private void DrawMinimap()
    {
        foreach (VirtualDungeonRoom room in virtualBoard.Values)
        {
            if(room == null)
                continue;
            GameObject newRoom = Instantiate(cellPrefab, new(), Quaternion.identity, minimapTransform);
            newRoom.name = "DungeonRoom_" + room.id + "_distance" + room.distance;
            newRoom.transform.localPosition = new((room.room.roomSize.x + room.position[0].x) * 30, (room.room.roomSize.y + room.position[0].y) * 30, 0);
            // Doors
            foreach(VirtualDoor door in room.doors)
            {
                GameObject newDoor = Instantiate(doorPrefab, newRoom.transform);
                // Set rotation
                if (door.side == Vector2.down)
                    newDoor.transform.localRotation = Quaternion.Euler(0, 0, 180);
                else if (door.side == Vector2.left)
                    newDoor.transform.localRotation = Quaternion.Euler(0, 0, 90);
                else if (door.side == Vector2.right)
                    newDoor.transform.localRotation = Quaternion.Euler(0, 0, -90);
                else if (door.side == Vector2.up)
                    newDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    private void FinishDoors()
    {
        /*foreach (VirtualDungeonRoom room in virtualRooms)
        {
            List<VirtualDoor> newDoors = new();
            foreach (VirtualDoor door in room.doors)
                if (door.isOccupied)
                    newDoors.Add(door);
            room.doors = newDoors;
        }*/
    }
    private void UpdateDoors()
    {
        /*foreach (VirtualDungeonRoom room in virtualRooms)
            foreach (VirtualDoor door in room.doors)
                if (door.isOccupied && door.leadToDoor == null)
                    door.leadToDoor = GetOtherDoor(door);*/
    }
    private VirtualDoor GetOtherDoor(VirtualDoor door)
    {
        if(virtualBoard.ContainsKey(door.side + door.pos) && virtualBoard[door.side + door.pos] != null)
            foreach (VirtualDungeonRoom vRoom in virtualRooms)
                foreach (VirtualDoor vDoor in vRoom.doors)
                    if (vDoor.pos == door.side + door.pos)
                        return vDoor;
        return null;
    }
    private void AddRoomToBoard(VirtualDungeonRoom room)
    {
        foreach(Vector2 tile in room.position)
            virtualBoard[tile] = room;
    }
    private bool CanPlaceRoom(List<Vector2> roomTiles)
    {
        bool canBePlaced = true;
        foreach (var tile in roomTiles)
            if (!virtualBoard.ContainsKey(tile) || virtualBoard[tile] != null)
                canBePlaced = false;
        return canBePlaced;
    }



    // Old dungeon generator
    public enum Cell
    {
        None,
        Room,
        Hallway
    }
    [Header("Cave")]
    public int boardSize;
    public int maxRoomOffset;
    private Vector2 preferedDirection;
    private Dictionary<Vector2, Cell> board = new();


    public void BuildCave()
    {
        GenerateCave(maxRoomCount);
        rooms[0].SetActive(true);
        FindAnyObjectByType<PlayerMovement>().transform.position = rooms[0].GetComponent<DungeonRoom>().doors[2].transform.position;
    }
    int RandomOddInt(int min, int max)
    {
        int result = 0;
        bool done = false;
        while (!done)
        {
            result = UnityEngine.Random.Range(min, max);
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
    public void GenerateCave(int maxRoomCount)
    {
        rooms.Clear();
        board.Clear();
        FindObjectOfType<DungeonMap>().ClearMap();

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

                // Doors back to player's base
                newRoom.GetComponent<DungeonRoom>().AddDoors(2, null);
                newRoom.GetComponent<DungeonRoom>().doors[2].GetComponent<Door>().baseDoors = true;
                newRoom.GetComponent<DungeonRoom>().doors[2].GetComponent<Door>().sceneName = "Home";

                previousRoom = newRoom;
                newRoom.SetActive(false);
                rooms.Add(newRoom);
                currentRoom++;
            }
            else
            {
                System.Random rnd = new();
                int rndOffset = rnd.Next(1, maxRoomOffset);
                List<Vector2> directions;
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

                // Prefered direction
                if (preferedDirection != null && preferedDirection != Vector2.zero)
                {
                    // Right
                    if (preferedDirection == new Vector2(0, 1))
                        directions.Add(new(0, rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2)));
                    // Left
                    else if (preferedDirection == new Vector2(0, -1))
                        directions.Add(new(0, -(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.x + roomSize.x) / 2))));
                    // Up
                    else if (preferedDirection == new Vector2(1, 0))
                        directions.Add(new(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2), 0));
                    // Down
                    else if (preferedDirection == new Vector2(-1, 0))
                        directions.Add(new(-(rndOffset + ((previousRoom.GetComponent<DungeonRoom>().size.y + roomSize.y) / 2)), 0));
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
                            preferedDirection = new(dir.x, dir.y);
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
                        preferedDirection = Vector2.zero;
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
    }
    public GameObject GenerateRoom(Vector2 size /* !Has to be odd numbers! */)
    {
        GameObject newRoom = new("Room-" + rooms.Count.ToString());
        newRoom.transform.SetParent(dungeonScene);
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
        Instantiate(dungeonDatabase.floorMousePointer, new(0, 0, 0), Quaternion.identity, floor.transform);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Instantiate(dungeonDatabase.floors[0], new(x * 3, 0, y * 3), Quaternion.identity, floor.transform);
                // Doors
                if(x == (size.x - 1) / 2)
                {
                    if(y == 0)
                    {
                        Instantiate(dungeonDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("Doors")).GetComponentInChildren<MeshRenderer>().material = dungeonDatabase.wallMaterials[1];
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), newRoom.transform.Find("DoorWalls")).GetComponentInChildren<MeshRenderer>().material = dungeonDatabase.wallMaterials[1];
                    }
                    else if (y == size.y - 1)
                    {
                        Instantiate(dungeonDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("Doors"));
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), newRoom.transform.Find("DoorWalls"));
                    }
                }
                else if(y == (size.y - 1) / 2)
                {
                    if (x == 0)
                    {
                        Instantiate(dungeonDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("Doors"));
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), newRoom.transform.Find("DoorWalls"));
                    }
                    else if (x == size.x - 1)
                    {
                        Instantiate(dungeonDatabase.doors[0], new(x * 3, 0, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform.Find("Doors"));
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), newRoom.transform.Find("DoorWalls"));
                    }
                }
                else
                {
                    // Walls
                    if (x == 0)
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, -90, 0), walls.transform);
                    if (x == size.x - 1)
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 90, 0), walls.transform);
                    if (y == 0)
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 180, 0), walls.transform).GetComponentInChildren<MeshRenderer>().material = dungeonDatabase.wallMaterials[1];
                    if (y == size.y - 1)
                        Instantiate(dungeonDatabase.walls[0], new(x * 3, -0.1f, y * 3), Quaternion.Euler(0, 0, 0), walls.transform);
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
        Dictionary<Vector2, GameObject> objs = new();
        Dictionary<Vector2, GameObject> pop = new();
        System.Random random = new();

        // Set min/max count of obstacles, enemies, lootboxes etc. depending on difficulty/roomSize/etc.
        int tilesCount = (int)(room.GetComponent<DungeonRoom>().size.x * room.GetComponent<DungeonRoom>().size.y * 9);
        int obstacleCount = random.Next(5, (int)(tilesCount * 0.05f));
        int meleeEnemiesCount = random.Next(2, (int)(tilesCount * 0.01f));
        int rangedEnemiesCount = random.Next(1, (int)(tilesCount * 0.005f));
        int lootBoxesCount = random.Next(0, (int)(tilesCount * 0.0075f));

        // Adding obstacles to tiles
        for (int i = 0; i < obstacleCount; i++)
        {
            bool done = false;
            while(!done)
            {
                random = new();
                Vector2 id = new(random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.x * 3) - 2), random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.y * 3) - 2));
                if (!objs.ContainsKey(id))
                {
                    objs.Add(id, dungeonDatabase.obstacles_shoot[random.Next(0, dungeonDatabase.obstacles_shoot.Count)]);
                    done = true;
                }
            }
        }

        // Assign lootBoxes to tiles
        for (int i = 0; i < lootBoxesCount; i++)
        {
            bool done = false;
            while (!done)
            {
                random = new();
                Vector2 id = new(random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.x * 3) - 2), random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.y * 3) - 2));
                if (!objs.ContainsKey(id))
                {
                    objs.Add(id, dungeonDatabase.lootBoxes[random.Next(0, dungeonDatabase.lootBoxes.Count)]);
                    done = true;
                }
            }
        }

        // Assign mineable resources to tiles
        for (int i = 0; i < lootBoxesCount; i++)
        {
            bool done = false;
            while (!done)
            {
                random = new();
                Vector2 id = new(random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.x * 3) - 2), random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.y * 3) - 2));
                if (!objs.ContainsKey(id))
                {
                    objs.Add(id, dungeonDatabase.resources[random.Next(0, dungeonDatabase.resources.Count)]);
                    done = true;
                }
            }
        }
        
        if(room.GetComponent<DungeonRoom>().roomID != 0)
        {
            // Assign melee enemies to tiles
            for (int i = 0; i < meleeEnemiesCount; i++)
            {
                bool done = false;
                while (!done)
                {
                    random = new();
                    Vector2 id = new(random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.x * 3) - 2), random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.y * 3) - 2));
                    if (!objs.ContainsKey(id) && !pop.ContainsKey(id))
                    {
                        //pop.Add(id, objDatabase.meleeEnemies[random.Next(0, objDatabase.meleeEnemies.Count)]);
                        done = true;
                    }
                }
            }

            // Assign ranged enemies to tiles
            for (int i = 0; i < rangedEnemiesCount; i++)
            {
                bool done = false;
                while (!done)
                {
                    random = new();
                    Vector2 id = new(random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.x * 3) - 2), random.Next(2, (int)(room.GetComponent<DungeonRoom>().size.y * 3) - 2));
                    if (!objs.ContainsKey(id) && !pop.ContainsKey(id))
                    {
                        //pop.Add(id, objDatabase.rangedEnemies[random.Next(0, objDatabase.rangedEnemies.Count)]);
                        done = true;
                    }
                }
            }
        }

        // Spawn population and objs
        GameObject objFolder = new("Objs");
        objFolder.transform.parent = room.transform;
        foreach (Vector2 id in objs.Keys)
            if (objs[id] != null)
                Instantiate(objs[id], new Vector3(id.x, 0, id.y), Quaternion.Euler(0, new System.Random().Next(-180, 180), 0), objFolder.transform);
        GameObject popFolder = new("Population");
        popFolder.transform.parent = room.transform;
        foreach (Vector2 id in pop.Keys)
        {
            if (pop[id] != null)
            {
                Instantiate(pop[id], new Vector3(id.x, 0, id.y), Quaternion.identity, popFolder.transform);
                room.GetComponent<DungeonRoom>().enemies.Add(pop[id].GetComponent<EnemyStats>());
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

class VirtualDungeonRoom
{
    public int id;
    public int distance;
    public List<Vector2> position;
    public Editor_Room room;
    public Vector2 dirBias;
    public VirtualDoor startDoor;
    public List<VirtualDoor> doors;
    public List<VirtualDungeonRoom> neighbourRooms = new();
    // Branches
    public bool canCreateNewBranches = false;
    public int noBranchCount = 0;

    public VirtualDungeonRoom(int distance, List<Vector2> position, Editor_Room room, Vector2 dirBias, int id)
    {
        this.distance = distance;
        this.position = position;
        this.room = room;
        this.dirBias = dirBias;
        this.id = id;
        doors = new();
        for (int i = 0; i < room.doors.Count; i++)
        {
            VirtualDoor newDoor = new(i);
            newDoor.SetSide(room);
            newDoor.SetPos(this);
            doors.Add(newDoor);
        }
    }
}

class VirtualDoor
{
    public int id;
    public Vector2 side;
    public Vector2 pos;
    public VirtualDoor leadToDoor;
    public bool isOccupied;

    public VirtualDoor(int id) { this.id = id; }
    public void SetSide(Editor_Room room)
    {
        int x = (int)room.roomSize.x;
        int y = (int)room.roomSize.y;

        if (id < x)
            side = Vector2.down;
        else if (id < x + y)
            side = Vector2.right;
        else if (id < (2 * x) + y)
            side = Vector2.up;
        else if (id < 2 * (x + y))
            side = Vector2.left;
        else
            side = Vector2.zero;
    }
    public void SetPos(VirtualDungeonRoom roomRef) { pos = GetPosOfDoor(roomRef); }
    private Vector2 GetPosOfDoor(VirtualDungeonRoom roomRef)
    {
        Vector2 pos = roomRef.position[0];
        if (roomRef.room.roomSize == new Vector2(1, 1))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y),
                2 => new(pos.x, pos.y + 1),
                3 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(2, 1))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y),
                3 => new(pos.x + 1, pos.y + 1),
                4 => new(pos.x, pos.y + 1),
                5 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(3, 1))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y - 1),
                3 => new(pos.x + 3, pos.y),
                4 => new(pos.x + 2, pos.y + 1),
                5 => new(pos.x + 1, pos.y + 1),
                6 => new(pos.x, pos.y + 1),
                7 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(1, 2))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y),
                2 => new(pos.x + 1, pos.y + 1),
                3 => new(pos.x, pos.y + 2),
                4 => new(pos.x - 1, pos.y + 1),
                5 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(2, 2))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y),
                3 => new(pos.x + 2, pos.y + 1),
                4 => new(pos.x + 1, pos.y + 2),
                5 => new(pos.x, pos.y + 2),
                6 => new(pos.x - 1, pos.y + 1),
                7 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(3, 2))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y - 1),
                3 => new(pos.x + 3, pos.y),
                4 => new(pos.x + 3, pos.y + 1),
                5 => new(pos.x + 2, pos.y + 2),
                6 => new(pos.x + 1, pos.y + 2),
                7 => new(pos.x, pos.y + 2),
                8 => new(pos.x - 1, pos.y + 1),
                9 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(1, 3))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y),
                2 => new(pos.x + 1, pos.y + 1),
                3 => new(pos.x + 1, pos.y + 2),
                4 => new(pos.x, pos.y + 3),
                5 => new(pos.x - 1, pos.y + 2),
                6 => new(pos.x - 1, pos.y + 1),
                7 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(2, 3))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y),
                3 => new(pos.x + 2, pos.y + 1),
                4 => new(pos.x + 2, pos.y + 2),
                5 => new(pos.x + 1, pos.y + 3),
                6 => new(pos.x, pos.y + 3),
                7 => new(pos.x - 1, pos.y + 2),
                8 => new(pos.x - 1, pos.y + 1),
                9 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else if (roomRef.room.roomSize == new Vector2(3, 3))
            return id switch
            {
                0 => new(pos.x, pos.y - 1),
                1 => new(pos.x + 1, pos.y - 1),
                2 => new(pos.x + 2, pos.y - 1),
                3 => new(pos.x + 3, pos.y),
                4 => new(pos.x + 3, pos.y + 1),
                5 => new(pos.x + 3, pos.y + 2),
                6 => new(pos.x + 2, pos.y + 3),
                7 => new(pos.x + 1, pos.y + 3),
                8 => new(pos.x, pos.y + 3),
                9 => new(pos.x - 1, pos.y + 2),
                10 => new(pos.x - 1, pos.y + 1),
                11 => new(pos.x - 1, pos.y),
                _ => pos
            };
        else
            return pos;
    }
}