using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private GameObject testObject;
    public GameObject currentRoom;
    [SerializeField] private DungeonGenerator dungeonGenerator;
    [SerializeField] private DungeonDatabase database;

    [SerializeField] private Door doorFromBaseToDungeon;

    [SerializeField] private float roomSize = 6f; // Size of each room in the dungeon

    private List<GameObject> dungeonRooms = new();
    private List<Door> allDoors = new();


    public enum DungeonType
    {
        Normal,
        Resourcefull,
    }
    [SerializeField] private DungeonType dungeonType;


    public void EnterDungeon(Transform player)
    {
        FindAnyObjectByType<DungeonGenerator>().GenerateDungeon(dungeonType);

        SpawnDungeonRooms(dungeonGenerator.placedRooms);

        currentRoom = dungeonRooms[0];

        // Add entrance door manually
        GameObject entranceDoor = AddEntranceDoor(dungeonGenerator.placedRooms[0], currentRoom);
        entranceDoor.GetComponent<Door>().leadToDoor = doorFromBaseToDungeon;
        entranceDoor.GetComponent<Door>().SetVisualState(true);

        foreach(GameObject room in dungeonRooms)
            if (room != currentRoom)
                room.SetActive(false); // Hide all rooms except the current one

        // Start first room
        currentRoom.GetComponent<DungeonRoom>().startRoom = true;

        // Teleport player to the first room
        player.position = entranceDoor.transform.position;
    }

    public void ExitDungeon()
    {
        for (int i = 0; i < dungeonRooms.Count; i++)
            Destroy(dungeonRooms[i]);
        dungeonRooms.Clear();
        currentRoom = null;
        allDoors.Clear();
    }

    private GameObject AddEntranceDoor(VirtualDungeonRoom firstRoom, GameObject firstRoomObj)
    {
        if (firstRoom.doors.Count == 0) return null;

        VirtualDoor entryVDoor = firstRoom.doors[0]; // Pick first door
        Vector3 localPos = new Vector3(entryVDoor.pos.x - firstRoom.position[0].x, 0, -1) * roomSize / 3;
        Quaternion rot = Quaternion.Euler(0, 180, 0);

        GameObject newDoor = Instantiate(database.wall_withDoors, firstRoomObj.transform.Find("Objs"));
        newDoor.transform.SetLocalPositionAndRotation(localPos, rot);
        Door doorComp = newDoor.GetComponent<Door>();
        doorComp.room = firstRoomObj;
        doorComp.virtualDoor = entryVDoor;
        doorComp.baseDoors = true;
        doorComp.sceneName = "Home";

        allDoors.Add(doorComp);

        // Delete wall behind the door
        for(int i = 0; i < firstRoomObj.transform.Find("Objs").childCount; i++)
        {
            Transform child = firstRoomObj.transform.Find("Objs").GetChild(i);
            if (child.gameObject.name.ToLower().Contains("wall") && child.position == newDoor.transform.position)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        return newDoor;
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

        // Link doors
        LinkDoors();
    }

    private GameObject SpawnRoom(VirtualDungeonRoom room)
    {
        GameObject newRoom = Instantiate(database.roomPrefab, Vector3.zero, Quaternion.identity);
        newRoom.name = $"Room_{room.id} ({room.room.roomType})";
        GameObject structuresParent = new("Objs");
        structuresParent.transform.SetParent(newRoom.transform);

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
                GameObject floorTile = Instantiate(database.floor, structuresParent.transform);
                floorTile.transform.localPosition = tilePos;

                // Walls / Doors
                Vector2 worldTilePos = room.position[0] + new Vector2(x, y);

                // Bottom edge
                if (y == 0)
                {
                    Vector2 bottomPos = worldTilePos + Vector2.down;
                    if (doorPositions.Contains(bottomPos) && doorLookup[bottomPos].leadToDoor != null)
                    {
                        GameObject newDoor = Instantiate(
                            database.wall_withDoors,
                            tilePos - new Vector3(0, 0, roomSize / 3),
                            Quaternion.Euler(0, 180, 0),
                            structuresParent.transform);
                        newDoor.GetComponent<Door>().room = newRoom;
                        newDoor.GetComponent<Door>().virtualDoor = doorLookup[bottomPos];
                        newDoor.GetComponent<Door>().leadToVirtualDoor = doorLookup[bottomPos].leadToDoor;
                        allDoors.Add(newDoor.GetComponent<Door>());
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos - new Vector3(0, 0, roomSize / 3), Quaternion.Euler(0, 180, 0), structuresParent.transform);
                }

                // Top edge
                if (y == room.room.roomSize.y - 1)
                {
                    Vector2 topPos = worldTilePos + Vector2.up;
                    if (doorPositions.Contains(topPos) && doorLookup[topPos].leadToDoor != null)
                    {
                        GameObject newDoor = Instantiate(
                            database.wall_withDoors,
                            tilePos + new Vector3(0, 0, roomSize / 3),
                            Quaternion.identity,
                            structuresParent.transform);
                        newDoor.GetComponent<Door>().room = newRoom;
                        newDoor.GetComponent<Door>().virtualDoor = doorLookup[topPos];
                        newDoor.GetComponent<Door>().leadToVirtualDoor = doorLookup[topPos].leadToDoor;
                        allDoors.Add(newDoor.GetComponent<Door>());
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos + new Vector3(0, 0, roomSize / 3), Quaternion.identity, structuresParent.transform);
                }

                // Left edge
                if (x == 0)
                {
                    Vector2 leftPos = worldTilePos + Vector2.left;
                    if (doorPositions.Contains(leftPos) && doorLookup[leftPos].leadToDoor != null)
                    {
                        GameObject newDoor = Instantiate(
                            database.wall_withDoors,
                            tilePos - new Vector3(roomSize / 3, 0, 0),
                            Quaternion.Euler(0, -90, 0),
                            structuresParent.transform);
                        newDoor.GetComponent<Door>().room = newRoom;
                        newDoor.GetComponent<Door>().virtualDoor = doorLookup[leftPos];
                        newDoor.GetComponent<Door>().leadToVirtualDoor = doorLookup[leftPos].leadToDoor;
                        allDoors.Add(newDoor.GetComponent<Door>());
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos - new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, -90, 0), structuresParent.transform);
                }

                // Right edge
                if (x == room.room.roomSize.x - 1)
                {
                    Vector2 rightPos = worldTilePos + Vector2.right;
                    if (doorPositions.Contains(rightPos) && doorLookup[rightPos].leadToDoor != null)
                    {
                        GameObject newDoor = Instantiate(
                            database.wall_withDoors,
                            tilePos + new Vector3(roomSize / 3, 0, 0),
                            Quaternion.Euler(0, 90, 0),
                            structuresParent.transform);
                        newDoor.GetComponent<Door>().room = newRoom;
                        newDoor.GetComponent<Door>().virtualDoor = doorLookup[rightPos];
                        newDoor.GetComponent<Door>().leadToVirtualDoor = doorLookup[rightPos].leadToDoor;
                        allDoors.Add(newDoor.GetComponent<Door>());
                    }
                    else
                        Instantiate(database.wall_noDoors, tilePos + new Vector3(roomSize / 3, 0, 0), Quaternion.Euler(0, 90, 0), structuresParent.transform);
                }
            }
        }

        // Populate the room
        // Get all tiles prepared in Editor and filter out empty tiles
        Dictionary<int, string> tiles = new();
        for (int i = 0; i < room.room.tiles.Count; i++)
            if (room.room.tiles[i] != "0")
                tiles.Add(i, room.room.tiles[i]);

        GameObject pop = new("Population");
        pop.transform.SetParent(newRoom.transform);
        pop.transform.localPosition = new(-3.75f, 0, -3.75f);
        Transform parent = pop.transform;
        GameObject GetObjectFromDatabaseByType(string tileType)
        {
            return tileType switch
            {
                "1" => database.obstacles_noShoot_1x1[Random.Range(0, database.obstacles_noShoot_1x1.Count)],
                "2" => database.obstacles_noShoot_2x1[Random.Range(0, database.obstacles_noShoot_2x1.Count)],
                "4" => database.obstacles_noShoot_3x1[Random.Range(0, database.obstacles_noShoot_3x1.Count)],
                "5" => database.obstacles_noShoot_2x2[Random.Range(0, database.obstacles_noShoot_2x2.Count)],
                "6" => database.obstacles_shoot_1x1[Random.Range(0, database.obstacles_shoot_1x1.Count)],
                "7" => database.lightsources[Random.Range(0, database.lightsources.Count)],
                "8" => database.resources[Random.Range(0, database.resources.Count)],
                "9" => database.enemies_mAgressive[Random.Range(0, database.enemies_mAgressive.Count)],
                "10" => database.enemies_mEvasive[Random.Range(0, database.enemies_mEvasive.Count)],
                "11" => database.enemies_mWandering[Random.Range(0, database.enemies_mWandering.Count)],
                "12" => database.enemies_mStealth[Random.Range(0, database.enemies_mStealth.Count)],
                "13" => database.enemies_rStatic[Random.Range(0, database.enemies_rStatic.Count)],
                "14" => database.enemies_rWandering[Random.Range(0, database.enemies_rWandering.Count)],
                "15" => database.traps[Random.Range(0, database.traps.Count)],
                _ => null
            };
        }
        foreach (int key in tiles.Keys)
        {
            string value = tiles[key];

            // Each "fine tile" size inside one big tile
            float fineTileSize = roomSize / 6f; // e.g. 9 / 6 = 1.5 unit per fine tile

            // Convert key into fine-grid coordinates
            int fineX = key % ((int)room.room.roomSize.x * 6);
            int fineY = key / ((int)room.room.roomSize.x * 6);

            // Calculate local position relative to bottom-left corner
            Vector3 localPos = new(fineX * fineTileSize, 0, fineY * fineTileSize);

            // Place object
            GameObject newObject = Instantiate(GetObjectFromDatabaseByType(value), parent);
            newObject.transform.localPosition = localPos;
        }

        // Create nav mesh plan
        newRoom.GetComponent<NavMeshSurface>().BuildNavMesh();

        return newRoom;
    }

    private void LinkDoors()
    {
        // Build lookup: VirtualDoor -> Scene Door
        Dictionary<VirtualDoor, Door> doorMap = allDoors
            .Where(d => d.virtualDoor != null)
            .ToDictionary(d => d.virtualDoor, d => d);

        foreach (Door door in allDoors)
        {
            if (door.virtualDoor == null || door.leadToVirtualDoor == null)
                continue;

            if (doorMap.TryGetValue(door.leadToVirtualDoor, out Door partner))
            {
                door.leadToDoor = partner;
                door.leadToRoom = partner.room;

                partner.leadToDoor = door;
                partner.leadToRoom = door.room;
            }

            door.canInteract = true;
        }

        Debug.Log($"Linked {allDoors.Count} doors.");
    }
}
