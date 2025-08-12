using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public int totalRooms = 10;
    public int minDistanceForBoss = 5;

    private List<VirtualDungeonRoom> placedRooms = new();
    private Queue<VirtualDungeonRoom> frontier = new(); // For BFS-like placement
    private HashSet<Vector2> occupiedPositions = new();

    public Dictionary<Editor_Room.RoomType, List<RoomData>> roomsByType = new();

    private void Start()
    {
        LoadRooms();
        GenerateDungeon();
    }

    public void LoadRooms()
    {
        roomsByType.Clear();

        TextAsset[] files = Resources.LoadAll<TextAsset>("DungeonRooms");

        foreach (TextAsset file in files)
        {
            RoomData room = JsonUtility.FromJson<RoomData>(file.text);

            if (!roomsByType.ContainsKey(room.roomType))
                roomsByType[room.roomType] = new List<RoomData>();

            roomsByType[room.roomType].Add(room);
        }

        Debug.Log($"- DungeonGenerator - Loaded {files.Length} rooms into {roomsByType.Count} types.");
    }

    public void GenerateDungeon()
    {
        placedRooms.Clear();
        frontier.Clear();
        occupiedPositions.Clear();

        // Create starting room at (0,0)
        RoomData startData = GetRandomRoomOfType(Editor_Room.RoomType.Start);
        List<Vector2> allTiles = CalculateRoomTiles(Vector2.zero, startData.roomSize);
        VirtualDungeonRoom startRoom = new(
            distance: 0,
            position: allTiles,
            room: startData,
            dirBias: Vector2.zero,
            id: 0
        );

        placedRooms.Add(startRoom);
        frontier.Enqueue(startRoom);
        foreach(Vector2 tile in allTiles)
            occupiedPositions.Add(tile);

        int roomId = 1;

        // BFS-like expansion
        while (placedRooms.Count < totalRooms && frontier.Count > 0)
        {
            VirtualDungeonRoom current = frontier.Dequeue();

            foreach (VirtualDoor door in current.doors)
            {
                if (door.isOccupied || UnityEngine.Random.Range(0, 100) >= 50f) continue;

                // New position is just current door position
                Vector2 newPos = door.pos;

                // Enforce "never below start room" rule
                if (newPos.y < 0)
                    continue;

                // Pick room type
                Editor_Room.RoomType typeToPlace = DecideRoomType(current.distance);
                RoomData data = GetRandomRoomOfType(typeToPlace);
                if (data == null) continue;

                // Skip if position already occupied
                allTiles = CalculateRoomTiles(newPos, data.roomSize);
                bool _continue = false;
                foreach (Vector2 tile in allTiles)
                {
                    if (occupiedPositions.Contains(tile))
                    {
                        _continue = true;
                        break;
                    }
                }
                if (_continue) continue;

                // anchorPos is the bottom-left tile
                VirtualDungeonRoom newRoom = new(
                    current.distance + 1,
                    allTiles,
                    data,
                    -door.side,
                    roomId++
                );

                // Link doors
                door.isOccupied = true;
                door.leadToDoor = newRoom.doors.Find(d => d.pos == current.position[0]);
                if (door.leadToDoor != null)
                {
                    door.leadToDoor.isOccupied = true;
                    door.leadToDoor.leadToDoor = door;
                }

                placedRooms.Add(newRoom);
                frontier.Enqueue(newRoom);
                foreach(Vector2 tile in allTiles)
                    occupiedPositions.Add(tile);

                if (placedRooms.Count >= totalRooms) break;
            }
        }

        // Draw the minimap
        FindObjectOfType<DungeonMap>().DrawMinimap(placedRooms);
    }

    private List<Vector2> CalculateRoomTiles(Vector2 anchorPos, Vector2 roomSize)
    {
        List<Vector2> tiles = new();
        for (int y = 0; y < roomSize.y; y++)
        {
            for (int x = 0; x < roomSize.x; x++)
            {
                tiles.Add(new Vector2(anchorPos.x + x, anchorPos.y + y));
            }
        }
        return tiles;
    }

    private Editor_Room.RoomType DecideRoomType(int distance)
    {
        if (distance >= minDistanceForBoss && !HasBossRoom())
            return Editor_Room.RoomType.Boss;

        // Example: 70% normal, 20% resource, 10% special
        float r = UnityEngine.Random.value;
        if (r < 0.85f) return Editor_Room.RoomType.Basic;
        return Editor_Room.RoomType.Resources;
    }

    private bool HasBossRoom()
    {
        return placedRooms.Any(r => r.room.roomType == Editor_Room.RoomType.Boss);
    }

    private RoomData GetRandomRoomOfType(Editor_Room.RoomType type)
    {
        if (!roomsByType.ContainsKey(type) || roomsByType[type].Count == 0)
            return null;

        var list = roomsByType[type];
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}

[Serializable]
public class RoomData
{
    public string bossName;
    public Editor_Room.RoomType roomType;
    public bool bossRoom;
    public Vector2 roomSize;
    public Dictionary<int, int> doorsSave;
    public Dictionary<int, string> tiles;
}
public class VirtualDungeonRoom
{
    public int id;
    public int distance;
    public List<Vector2> position;
    public RoomData room;
    public Vector2 dirBias;
    public VirtualDoor startDoor;
    public List<VirtualDoor> doors;
    public bool canCreateNewBranches = false;
    public int noBranchCount = 0;

    public VirtualDungeonRoom(int distance, List<Vector2> position, RoomData room, Vector2 dirBias, int id)
    {
        this.distance = distance;
        this.position = position;
        this.room = room;
        this.dirBias = dirBias;
        this.id = id;

        doors = new List<VirtualDoor>();

        // Generate doors based on roomSize
        int totalDoors = (int)(room.roomSize.x * 2 + room.roomSize.y * 2);
        for (int i = 0; i < totalDoors; i++)
        {
            VirtualDoor newDoor = new(i);
            newDoor.SetSide(room.roomSize);
            newDoor.SetPos(this);
            doors.Add(newDoor);
        }
    }
}
public class VirtualDoor
{
    public int id;
    public Vector2 side;
    public Vector2 pos;
    public VirtualDoor leadToDoor;
    public bool isOccupied;

    public VirtualDoor(int id) { this.id = id; }

    public void SetSide(Vector2 roomSize)
    {
        int x = (int)roomSize.x;
        int y = (int)roomSize.y;

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

    public void SetPos(VirtualDungeonRoom roomRef)
    {
        pos = GetPosOfDoor(roomRef);
    }

    public Vector2 GetPosOfDoor(VirtualDungeonRoom roomRef)
    {
        // Base position is first tile of the room
        Vector2 pos = roomRef.position[0];

        // The offsets here are always relative to the bottom-left tile of the room
        // so (0, 0) = bottom-left tile, then we offset based on the door index.
        Vector2 offset = Vector2.zero;

        if (roomRef.room.roomSize == new Vector2(1, 1))
            offset = id switch
            {
                0 => new(0, -1),  // bottom
                1 => new(1, 0),   // right
                2 => new(0, 1),   // top
                3 => new(-1, 0),  // left
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(2, 1))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, 0),
                3 => new(1, 1),
                4 => new(0, 1),
                5 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(3, 1))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, -1),
                3 => new(3, 0),
                4 => new(2, 1),
                5 => new(1, 1),
                6 => new(0, 1),
                7 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(1, 2))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, 0),
                2 => new(1, 1),
                3 => new(0, 2),
                4 => new(-1, 1),
                5 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(2, 2))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, 0),
                3 => new(2, 1),
                4 => new(1, 2),
                5 => new(0, 2),
                6 => new(-1, 1),
                7 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(3, 2))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, -1),
                3 => new(3, 0),
                4 => new(3, 1),
                5 => new(2, 2),
                6 => new(1, 2),
                7 => new(0, 2),
                8 => new(-1, 1),
                9 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(1, 3))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, 0),
                2 => new(1, 1),
                3 => new(1, 2),
                4 => new(0, 3),
                5 => new(-1, 2),
                6 => new(-1, 1),
                7 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(2, 3))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, 0),
                3 => new(2, 1),
                4 => new(2, 2),
                5 => new(1, 3),
                6 => new(0, 3),
                7 => new(-1, 2),
                8 => new(-1, 1),
                9 => new(-1, 0),
                _ => Vector2.zero
            };

        else if (roomRef.room.roomSize == new Vector2(3, 3))
            offset = id switch
            {
                0 => new(0, -1),
                1 => new(1, -1),
                2 => new(2, -1),
                3 => new(3, 0),
                4 => new(3, 1),
                5 => new(3, 2),
                6 => new(2, 3),
                7 => new(1, 3),
                8 => new(0, 3),
                9 => new(-1, 2),
                10 => new(-1, 1),
                11 => new(-1, 0),
                _ => Vector2.zero
            };

        return pos + offset;
    }
}
