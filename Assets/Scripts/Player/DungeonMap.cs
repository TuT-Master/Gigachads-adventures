using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    public bool mapOpened;

    [SerializeField]
    private GameObject dungeonMapCanvas;
    [SerializeField]
    private GameObject mapContentArea;

    private HUDmanager hudmanager;

    private Dungeon dungeon;

    // New map
    private GameObject[] rooms;
    [SerializeField] private GameObject roomPrefab;


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
        foreach (GameObject room in rooms)
            room.SetActive(false);

        // Cleared rooms
        for (int i = 0; i < dungeon.transform.childCount; i++)
            if (dungeon.transform.GetChild(i).GetComponent<DungeonRoom>().cleared)
            {
                rooms[i].SetActive(true);
                rooms[i].GetComponent<Image>().color = Color.blue;
            }

        // Tracking current room
        rooms[dungeon.currentRoom.GetComponent<DungeonRoom>().roomID].SetActive(true);
        rooms[dungeon.currentRoom.GetComponent<DungeonRoom>().roomID].GetComponent<Image>().color = Color.green;
    }

    public void ClearMap()
    {
        if(rooms == null)
            return;

        for (int i = 0;i < rooms.Length;i++)
        {
            Destroy(rooms[i]);
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



    public RectTransform minimapContainer;
    public GameObject roomIconPrefab;
    public GameObject doorIconPrefab;

    [Header("Display Settings")]
    public float roomSpacing = 20f; // Distance between rooms in minimap
    public float roomSize = 32f; // Size of each room icon in minimap
    public Color normalRoomColor = Color.white;
    public Color bossRoomColor = Color.red;
    public Color entranceRoomColor = Color.green;
    public Color resourceRoomColor = Color.blue;

    private Dictionary<Vector2, GameObject> icons = new();

    public void DrawMinimap(List<VirtualDungeonRoom> rooms)
    {
        // Resize icon prefab
        roomIconPrefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * roomSize;

        // Clear old icons
        foreach (Transform child in minimapContainer)
            Destroy(child.gameObject);
        icons.Clear();

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
            Vector2 minimapPos = bottomLeft * roomSpacing + (size * roomSpacing / 2f);

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
                doorPixelPos -= side * 16f;

                newDoor.GetComponent<RectTransform>().anchoredPosition = doorPixelPos;
            }

            icons[bottomLeft] = icon;
        }
    }
}
