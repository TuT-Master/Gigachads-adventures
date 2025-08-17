using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    public bool mapOpened;

    private HUDmanager hudmanager;
    private Dungeon dungeon;

    [SerializeField] private GameObject dungeonMapCanvas;
    [SerializeField] private RectTransform minimapContainer;
    [SerializeField] private GameObject roomIconPrefab;
    [SerializeField] private GameObject doorIconPrefab;

    [Header("Display Settings")]
    [SerializeField] private float roomSpacing = 20f; // Distance between rooms in minimap
    [SerializeField] private float roomSize = 32f; // Size of each room icon in minimap
    [SerializeField] private Color normalRoomColor = Color.white;
    [SerializeField] private Color bossRoomColor = Color.red;
    [SerializeField] private Color entranceRoomColor = Color.green;
    [SerializeField] private Color resourceRoomColor = Color.blue;
    [SerializeField] private Vector2 firstRoomPos;

    private Dictionary<Vector2, GameObject> rooms = new();

    void Start()
    {
        hudmanager = GetComponent<HUDmanager>();
        dungeon = FindAnyObjectByType<Dungeon>(FindObjectsInactive.Include);
        ToggleMap(false);
    }

    void Update()
    {
        if (rooms == null || dungeon.currentRoom == null)
            return;

        if(Input.GetKeyDown(KeyCode.M))
            hudmanager.ToggleMap(!mapOpened);
    }

    void UpdateMap()
    {
        // Hide rooms
        foreach (Vector2 room in rooms.Keys)
            rooms[room].SetActive(true);

        // Cleared rooms
        for (int i = 0; i < dungeon.transform.childCount; i++)
        {
            if (dungeon.transform.GetChild(i).GetComponent<DungeonRoom>().cleared)
            {
                Vector2 room = rooms.Keys.ToArray()[i];
                rooms[room].SetActive(true);
                rooms[room].GetComponent<Image>().color = Color.blue;
            }
        }
    }

    public void ClearMap()
    {
        if(rooms == null)
            return;

        for (int i = 0;i < rooms.Keys.Count;i++)
        {
            Vector2 key = rooms.Keys.ToArray()[i];
            Destroy(rooms[key]);
        }

        rooms = null;
    }

    public void ToggleMap(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 0f;
            dungeonMapCanvas.SetActive(true);
            mapOpened = true;
            UpdateMap();
        }
        else
        {
            Time.timeScale = 1f;
            dungeonMapCanvas.SetActive(false);
            mapOpened = false;
        }
    }

    public void DrawMinimap(List<VirtualDungeonRoom> rooms)
    {
        // Resize icon prefab
        roomIconPrefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * roomSize;

        // Clear old icons
        foreach (Transform child in minimapContainer)
            Destroy(child.gameObject);
        this.rooms.Clear();

        foreach (VirtualDungeonRoom room in rooms)
        {
            // Spawn icon
            GameObject icon = Instantiate(roomIconPrefab, minimapContainer);

            // Set name
            icon.name = $"Room_{room.id}";
            icon.GetComponentInChildren<TextMeshProUGUI>().text = room.id.ToString();

            // Set color
            Image img = icon.GetComponent<Image>();
            if (room.room.roomType == Editor_Room.RoomType.Boss)
                img.color = bossRoomColor;
            else if (room.room.roomType == Editor_Room.RoomType.Resources)
                img.color = resourceRoomColor;
            else if (room.id == 0)
                img.color = entranceRoomColor;
            else
                img.color = normalRoomColor;

            Vector2 size = room.room.roomSize; // in tiles
            Vector2 bottomLeft = room.position[0]; // already bottom-left in your generator

            // Position in minimap space
            Vector2 minimapPos = bottomLeft * roomSpacing + (size * roomSpacing / 2f) + firstRoomPos;

            icon.GetComponent<RectTransform>().anchoredPosition = minimapPos;
            icon.GetComponent<RectTransform>().sizeDelta = size * roomSize;

            // Place doors
            foreach (VirtualDoor door in room.doors)
            {
                if (!door.isOccupied)
                    continue;

                GameObject newDoor = Instantiate(doorIconPrefab, icon.transform);

                // Rotation
                Vector2 side = door.side;
                float rotZ = side == Vector2.up ? 180 :
                             side == Vector2.right ? -90 :
                             side == Vector2.left ? 90 : 0;
                newDoor.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                // Pixels per tile in minimap
                float tilePixelSize = roomSize;

                // Local position in tile space (relative to room's bottom-left)
                Vector2 localTilePos = door.pos - room.position[0];

                // Convert to pixels
                Vector2 doorPixelPos = (localTilePos * tilePixelSize) + (Vector2.one * (tilePixelSize / 2f));

                // Move 16 pixels toward room center
                doorPixelPos -= side * 48f;

                newDoor.GetComponent<RectTransform>().anchoredPosition = doorPixelPos;
            }

            this.rooms[bottomLeft] = icon;
        }
    }
}
