using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    [SerializeField] private DungeonDatabase dungeonDatabase;

    [SerializeField] private GameObject newRoomTab;
    [SerializeField] private GameObject customDoorsTab;
    [SerializeField] private GameObject customDoorsButton;
    [SerializeField] private GameObject editorTab;

    [SerializeField] private GameObject customDoorsTilePrefab;
    [SerializeField] private GameObject customDoorsWallPrefab;
    [SerializeField] private Transform customDoorsTilesTransformParent;

    [SerializeField] private GameObject bossTypeDropdown;
    [SerializeField] private TextMeshProUGUI brushDescription;

    [SerializeField] private TextMeshProUGUI log;

    private Editor_Room room;
    private Editor_Door[] customDoors;

    private bool deletingLog = false;


    private void Start()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
    }
    private void Update()
    {
        // Autoclearing log
        if (log.text != "" && !deletingLog)
            StartCoroutine(DeleteLog(4f));



        if (room == null)
            return;

        if (room.bossRoom)
            bossTypeDropdown.gameObject.SetActive(true);
        else
            bossTypeDropdown.gameObject.SetActive(false);

        if (newRoomTab.activeInHierarchy && room.roomSize != null && room.roomSize.x != 0 && room.roomSize.y != 0)
            customDoorsButton.SetActive(true);
        else
            customDoorsButton.SetActive(false);
    }
    private IEnumerator DeleteLog(float delay)
    {
        deletingLog = true;
        yield return new WaitForSecondsRealtime(delay);
        log.text = "";
        deletingLog = false;

    }

    public void BrushChanged(TMP_Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 0:

                brushDescription.text = "No one can shoot over these obstacles";
                break;
            case 1:

                brushDescription.text = "Everyone can shoot over these obstacles";
                break;
            case 2:

                brushDescription.text = "Enemy";
                break;
            case 3:

                brushDescription.text = "Lightsource";
                break;
            case 4:

                brushDescription.text = "Resource";
                break;
            case 5:

                brushDescription.text = "Lootbox";
                break;
            case 6:

                brushDescription.text = "Lootbox";
                break;
            case 7:

                brushDescription.text = "Lootbox";
                break;
            case 8:

                brushDescription.text = "Lootbox";
                break;
            case 9:

                brushDescription.text = "Lootbox";
                break;
            case 10:

                brushDescription.text = "Lootbox";
                break;
            case 11:

                brushDescription.text = "Lootbox";
                break;
            case 12:

                brushDescription.text = "Trap";
                break;
            default:

                break;
        }
    }
    public void NewRoom_button()
    {
        newRoomTab.SetActive(true);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        customDoorsButton.SetActive(false);
        room = new Editor_Room();
    }
    public void Exit_button()
    {
        Application.Quit();
    }
    public void Export_button()
    {

    }
    public void Cancel_button()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
    }
    public void Done_button()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(true);
        customDoorsTab.SetActive(false);
        // Generate room in editor

    }
    public void DoorsDone_button()
    {
        newRoomTab.SetActive(true);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        // Apply custom doors settings

    }
    public void CancelDoors_button()
    {
        newRoomTab.SetActive(true);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        customDoors = null;
    }
    public void CustomDoors_button()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(true);

        customDoors = new Editor_Door[2 * (int)(room.roomSize.x + room.roomSize.y)];
        int currentDoorId = 0;
        customDoorsTilesTransformParent.GetComponent<GridLayoutGroup>().constraintCount = (int)room.roomSize.x;
        for (int y = 0; y < room.roomSize.y; y++)
        {
            for (int x = 0; x < room.roomSize.x; x++)
            {
                GameObject newTile = Instantiate(customDoorsTilePrefab, customDoorsTilesTransformParent);
                if(x == 0 && y == 0)
                {
                    // Left bottom wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(-45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                    newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 0, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (x == room.roomSize.x - 1 && y == 0)
                {
                    // Right bottom wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 0, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                    newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (x == room.roomSize.x - 1 && y == room.roomSize.y - 1)
                {
                    // Right upper wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                    newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 90, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (x == 0 && y == room.roomSize.y - 1)
                {
                    // Left upper wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 90, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                    newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(-45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if(x == 0)
                {
                    // Left wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(-45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (x == room.roomSize.x - 1)
                {
                    // Right wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (y == 0)
                {
                    // Bottom wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 0, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
                else if (y == room.roomSize.y - 1)
                {
                    // Upper wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 90, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_Door>();
                    currentDoorId++;
                }
            }
        }
    }
    public void HeightValueChanged(TMP_InputField textInput)
    {
        if (room.roomSize == null)
            room.roomSize = new(0, 0);
        if (int.TryParse(textInput.text, out int height))
            room.roomSize = new(room.roomSize.x, height);
        else
            log.text = "Invalid input! Cannot convert height parameter to number.";
    }
    public void WidthValueChanged(TMP_InputField textInput)
    {
        if (room.roomSize == null)
            room.roomSize = new(0, 0);
        if (int.TryParse(textInput.text, out int width))
            room.roomSize = new(width, room.roomSize.y);
        else
            log.text = "Invalid input! Cannot convert width parameter to number.";
    }
    public void BossRoom()
    {
        room.bossRoom = !room.bossRoom;
    }
    public void BossType(TMP_Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 0:
                room.boss = Editor_Room.BossType.SomeBoss_1;
                break;
            case 1:
                room.boss = Editor_Room.BossType.SomeBoss_2;
                break;
            default:
                room.boss = Editor_Room.BossType.None;
                break;
        }
    }
    public void RoomType(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                room.roomType = Editor_Room.RoomType.Basic;
                break;
            case 1:
                room.roomType = Editor_Room.RoomType.Resources;
                break;
            case 2:
                room.roomType = Editor_Room.RoomType.Start;
                break;
            case 3:
                room.roomType = Editor_Room.RoomType.Boss;
                break;
            default:
                room.roomType = Editor_Room.RoomType.Basic;
                break;
        }
    }
}



public class Editor_Room
{
    public enum RoomType
    {
        Basic,
        Resources,
        Start,
        Boss,
    }
    public enum BossType
    {
        None,
        SomeBoss_1,
        SomeBoss_2,
    }
    public BossType boss;
    public RoomType roomType;

    public bool bossRoom;
    public string roomName;
    public Vector2 roomSize;


    public Editor_Room()
    {


    }


    public void SaveToFile()
    {

    }

    public void LoadFromFile()
    {

    }
}
