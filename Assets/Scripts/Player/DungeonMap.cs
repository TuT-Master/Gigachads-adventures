using System.Collections.Generic;
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



    public RectTransform minimapContainer; // Assign in Inspector
    public GameObject roomIconPrefab; // Small square Image prefab

    [Header("Display Settings")]
    public float roomSpacing = 20f; // Distance between rooms in minimap
    public Color normalRoomColor = Color.white;
    public Color bossRoomColor = Color.red;
    public Color entranceRoomColor = Color.green;

    private Dictionary<Vector2, GameObject> icons = new();

    public void DrawMinimap(List<VirtualDungeonRoom> rooms)
    {
        // Clear old icons
        foreach (Transform child in minimapContainer)
            Destroy(child.gameObject);
        icons.Clear();

        foreach (var room in rooms)
        {
            // Spawn icon
            GameObject icon = Instantiate(roomIconPrefab, minimapContainer);

            // Set name
            icon.name = $"Room_{room.id}";

            // Set color
            Image img = icon.GetComponent<Image>();
            if (room.room.roomType == Editor_Room.RoomType.Boss)
                img.color = bossRoomColor;
            else if (room.id == 0)
                img.color = entranceRoomColor;
            else
                img.color = normalRoomColor;

            Vector2 size = room.room.roomSize; // in tiles
            Vector2 bottomLeft = room.position[0]; // already bottom-left in your generator

            // Position in minimap space
            Vector2 minimapPos = bottomLeft * roomSpacing;// + (size * roomSpacing / 2f); // shift so icon is centered over its footprint

            icon.GetComponent<RectTransform>().anchoredPosition = minimapPos;
            icon.GetComponent<RectTransform>().sizeDelta = size * 16;

            icons[bottomLeft] = icon;
        }
    }
}
