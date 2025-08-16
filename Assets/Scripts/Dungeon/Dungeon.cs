using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject currentRoom;
    [SerializeField] private DungeonGenerator dungeonGenerator;
    [SerializeField] private DungeonDatabase database;

    [SerializeField] private float roomSize = 6f; // Size of each room in the dungeon

    private List<GameObject> dungeonRooms = new();
    private List<Door> dungeonDoors = new();

    private void Update()
    {
        if (transform.childCount > 0)
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).gameObject.activeInHierarchy)
                    currentRoom = transform.GetChild(i).gameObject;
    }

    private void OnDisable()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        FindAnyObjectByType<DungeonMap>().ClearMap();
    }

    public void EnterDungeon()
    {
        FindAnyObjectByType<DungeonGenerator>().GenerateDungeon();

        SpawnDungeonRooms(dungeonGenerator.placedRooms);

        currentRoom = dungeonRooms[0];
    }

    private void SpawnDungeonRooms(List<VirtualDungeonRoom> rooms)
    {
        dungeonRooms.Clear();
        foreach (VirtualDungeonRoom room in rooms)
        {
            GameObject roomObj = SpawnRoom(room);
            roomObj.transform.SetParent(transform);
            roomObj.transform.position = new Vector3(room.position[0].x, 0, room.position[0].y) * roomSize;

            dungeonRooms.Add(roomObj);
        }
    }

    private GameObject SpawnRoom(VirtualDungeonRoom room)
    {
        GameObject newRoom = new($"Room_{room.id} ({room.room.roomType})");

        // Build a quick lookup for door positions
        HashSet<Vector2> doorPositions = new();
        Dictionary<Vector2, VirtualDoor> doorLookup = new();

        foreach (VirtualDoor door in room.doors)
        {
            doorPositions.Add(door.pos);
            doorLookup.Add(door.pos, door);
        }

        for (int y = 0; y < room.room.roomSize.y; y++)
        {
            for (int x = 0; x < room.room.roomSize.x; x++)
            {
                Vector3 tilePos = new Vector3(x, 0, y) * roomSize;

                // Floor
                GameObject floorTile = Instantiate(database.floor, newRoom.transform);
                floorTile.transform.localPosition = tilePos;

                // Walls / Doors
                Vector2 worldTilePos = room.position[0] + new Vector2(x, y);

                // Bottom edge
                if (y == 0)
                {
                    Vector2 bottomPos = worldTilePos + Vector2.down;
                    if (doorPositions.Contains(bottomPos) && doorLookup[bottomPos].leadToDoor != null)
                    {
                        Instantiate(database.wall_withDoors, tilePos - new Vector3(0, 0, roomSize / 3), Quaternion.Euler(0, 180, 0), newRoom.transform);
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos - new Vector3(0, 0, roomSize / 3), Quaternion.Euler(0, 180, 0), newRoom.transform);
                }

                // Top edge
                if (y == room.room.roomSize.y - 1)
                {
                    Vector2 topPos = worldTilePos + Vector2.up;
                    if (doorPositions.Contains(topPos) && doorLookup[topPos].leadToDoor != null)
                    {
                        Instantiate(database.wall_withDoors, tilePos + new Vector3(0, 0, roomSize / 3), Quaternion.identity, newRoom.transform);
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos + new Vector3(0, 0, roomSize / 3), Quaternion.identity, newRoom.transform);
                }

                // Left edge
                if (x == 0)
                {
                    Vector2 leftPos = worldTilePos + Vector2.left;
                    if (doorPositions.Contains(leftPos) && doorLookup[leftPos].leadToDoor != null)
                    {
                        Instantiate(database.wall_withDoors, tilePos - new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, -90, 0), newRoom.transform);
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos - new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, -90, 0), newRoom.transform);
                }

                // Right edge
                if (x == room.room.roomSize.x - 1)
                {
                    Vector2 rightPos = worldTilePos + Vector2.right;
                    if (doorPositions.Contains(rightPos) && doorLookup[rightPos].leadToDoor != null)
                    {
                        Instantiate(database.wall_withDoors, tilePos + new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, 90, 0), newRoom.transform);
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos + new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, 90, 0), newRoom.transform);
                }
            }
        }

        return newRoom;
    }
}
