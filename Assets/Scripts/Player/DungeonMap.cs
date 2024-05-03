using System;
using System.Collections;
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

    public void BuildMap(Dictionary<Vector2, DungeonGenerator.Cell> board, List<GameObject> boardRooms)
    {
        rooms = new GameObject[boardRooms.Count];
        mapContentArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MathF.Sqrt(board.Count) * 30);
        mapContentArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MathF.Sqrt(board.Count) * 30);
        for (int i = 0; i < boardRooms.Count; i++)
        {
            roomPrefab.GetComponent<DungeonRoomUI>().SetRoomUp(boardRooms[i].GetComponent<DungeonRoom>());
            rooms[i] = Instantiate(roomPrefab, mapContentArea.transform);
            rooms[i].transform.localPosition = new Vector3(boardRooms[i].GetComponent<DungeonRoom>().boardPos.x * 30, boardRooms[i].GetComponent<DungeonRoom>().boardPos.y * 30, 0);
        }
    }

    void UpdateMap()
    {
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
}
