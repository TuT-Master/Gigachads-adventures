using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class Editor : MonoBehaviour
{
    [Header("Dungeon database")]
    [SerializeField] private DungeonDatabase dungeonDatabase;

    [Header("UI")]
    [SerializeField] private GameObject newRoomTab;
    [SerializeField] private GameObject customDoorsTab;
    [SerializeField] private GameObject customDoorsButton;
    [SerializeField] private GameObject editorTab;
    [SerializeField] private GameObject bossTypeDropdown;

    [Header("Log")]
    [SerializeField] private TextMeshProUGUI log;

    [Header("Prefabs")]
    [SerializeField] private GameObject customDoorsTilePrefab;
    [SerializeField] private GameObject customDoorsWallPrefab;
    [SerializeField] private Transform customDoorsTilesTransformParent;
    [SerializeField] private TMP_InputField inputField_height;
    [SerializeField] private TMP_InputField inputField_width;
    [SerializeField] private GameObject buttonDone;

    [Header("Editor")]
    [SerializeField] private TextMeshProUGUI brushDescription;
    [SerializeField] private TextMeshProUGUI statisticsText;
    private Dictionary<BrushType, int> statistics;
    public enum BrushType
    {
        None,
        Obstacle_noShoot,
        Obstacle_shoot,
        Lightsource,
        Lootbox,
        Resource,
        Enemy_mAggresive,
        Enemy_mEvasive,
        Enemy_mWandering,
        Enemy_mStealth,
        Enemy_rStatic,
        Enemy_rWandering,
        Trap,
        Specific,
    }
    public BrushType brushType;
    public string specificObjName;
    [SerializeField] private GameObject specificObjsContectArea;
    [SerializeField] private GameObject specificObjPrefab;
    [SerializeField] private Transform workspace;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject doorPrefab;
    private Dictionary<int, Editor_Tile> tiles;

    [Header("Saving to file")]
    [SerializeField] private UnityEngine.Object saveFolder;


    private Editor_Room room;
    private Dictionary<int, Editor_CustomDoor> customDoors;
    private bool deletingLog = false;


    private void Start()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        bossTypeDropdown.SetActive(false);
    }
    private void Update()
    {
        // Autoclearing log
        if (log.text != "" && !deletingLog)
            StartCoroutine(DeleteLog(4f));



        if (room == null)
            return;

        if (room.bossRoom)
            bossTypeDropdown.SetActive(true);
        else
            bossTypeDropdown.SetActive(false);

        if (newRoomTab.activeInHierarchy && room.roomSize != null && room.roomSize.x != 0 && room.roomSize.y != 0)
        {
            customDoorsButton.SetActive(true);
            buttonDone.SetActive(true);
        }
        else
        {
            customDoorsButton.SetActive(false);
            buttonDone.SetActive(false);
        }

        if (tiles == null)
            return;

        statisticsText.text = "";
        statistics = new()
        {
            {BrushType.Obstacle_noShoot, 0},
            {BrushType.Obstacle_shoot, 0},
            {BrushType.Lightsource, 0},
            {BrushType.Resource, 0},
            {BrushType.Lootbox, 0},
            {BrushType.Enemy_mAggresive, 0},
            {BrushType.Enemy_mEvasive, 0},
            {BrushType.Enemy_mWandering, 0},
            {BrushType.Enemy_mStealth, 0},
            {BrushType.Enemy_rStatic, 0},
            {BrushType.Enemy_rWandering, 0},
            {BrushType.Trap, 0},
            {BrushType.Specific, 0},
        };
        foreach (Editor_Tile tile in tiles.Values)
            if (tile.tileType != BrushType.None)
                statistics[tile.tileType]++;
        foreach (BrushType brushType in statistics.Keys)
            statisticsText.text += brushType.ToString() + ": " + statistics[brushType] + "\n";
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
                brushType = BrushType.None;
                brushDescription.text = "None\nBasically an eraser";
                break;
            case 1:
                brushType = BrushType.Obstacle_noShoot;
                brushDescription.text = "Obstacle\nNo one can shoot over it";
                break;
            case 2:
                brushType = BrushType.Obstacle_shoot;
                brushDescription.text = "Obstacle\nEveryone can shoot over it";
                break;
            case 3:
                brushType = BrushType.Lightsource;
                brushDescription.text = "Lightsource\n";
                break;
            case 4:
                brushType = BrushType.Resource;
                brushDescription.text = "Resource\nMinable or gatherable resource like mineral, stone, bush etc.";
                break;
            case 5:
                brushType = BrushType.Lootbox;
                brushDescription.text = "Resource\nMinable or gatherable resource like mineral, stone, bush etc.";
                break;
            case 6:
                brushType = BrushType.Enemy_mAggresive;
                brushDescription.text = "Enemy melle agressive\nThis bad boi will rush player right when he enters the room";
                break;
            case 7:
                brushType = BrushType.Enemy_mEvasive;
                brushDescription.text = "Enemy melle evasive\nThis bad boi will rush player right when he enters the room. Jumps from side to side while rushing player.";
                break;
            case 8:
                brushType = BrushType.Enemy_mWandering;
                brushDescription.text = "Enemy melle wandering\nThese guys walks in random paterns through the room and attacks player when they get close enough.";
                break;
            case 9:
                brushType = BrushType.Enemy_mStealth;
                brushDescription.text = "Enemy melle stealth\nWhen player gets close enough this enemy spawns and attack player.";
                break;
            case 10:
                brushType = BrushType.Enemy_rStatic;
                brushDescription.text = "Enemy ranged static\nStatic enemy who shoots player when close enough. Mostly turrets.";
                break;
            case 11:
                brushType = BrushType.Enemy_rWandering;
                brushDescription.text = "Enemy ranged wandering\nThese guys walks in random paterns through the room and shoots at player when close enough. Actively trying to keeps a safe distance between them and player.";
                break;
            case 12:
                brushType = BrushType.Trap;
                brushDescription.text = "Trap\nPlaceable sourse of various types of damages.";
                break;
            case 13:
                brushType = BrushType.Specific;
                brushDescription.text = "Specific object\nChoose any object you would like to place.";
                break;
            default:
                brushType = BrushType.Obstacle_noShoot;
                brushDescription.text = "Obstacle\nNo one can shoot over it";
                break;
        }

        if(brushType == BrushType.Specific)
        {
            // Set specific obj list
            List<GameObject> allObjs = dungeonDatabase.GetAllPlaceableObjs();
            foreach (GameObject obj in allObjs)
            {
                GameObject newSpecificObj = Instantiate(specificObjPrefab, specificObjsContectArea.transform);
                newSpecificObj.GetComponent<Editor_SpecificObj>().SetUp(obj.name, this);
            }
            specificObjsContectArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10 + (allObjs.Count * 30));
        }
        else
            for(int i = 0; i < specificObjsContectArea.transform.childCount; i++)
                Destroy(specificObjsContectArea.transform.GetChild(i).gameObject);
    }
    public void NewRoom_button()
    {
        newRoomTab.SetActive(true);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        customDoorsButton.SetActive(false);
        room = new Editor_Room();
        for (int i = 0; i < workspace.childCount; i++)
            Destroy(workspace.GetChild(i).gameObject);
        inputField_height.text = "";
        inputField_width.text = "";
        customDoors = null;
        tiles = null;

        // Set bosses
        bossTypeDropdown.GetComponent<TMP_Dropdown>().options.Clear();
        foreach (GameObject go in dungeonDatabase.enemies_boss)
            bossTypeDropdown.GetComponent<TMP_Dropdown>().options.Add(new(go.name));
    }
    public void Exit_button()
    {
        Application.Quit();
    }
    public void Export_button()
    {
        if (room == null)
            log.text = "You must first make some room object before export!";
        else
        {
            room.SaveToFile(saveFolder, tiles);
            room = null;
        }
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
        // Tiles
        tiles = new();
        Vector2 startPos = new(room.roomSize.x * 150, room.roomSize.y * 150);
        int count = 0;
        for (int y = 0; y < room.roomSize.y * 6; y++)
        {
            for (int x = 0; x < room.roomSize.x * 6; x++)
            {
                GameObject newTile = Instantiate(tilePrefab, workspace);
                newTile.GetComponent<Editor_Tile>().SetUpTile(new(x, y), this);
                newTile.GetComponent<RectTransform>().localPosition = new Vector3((x * 50) - startPos.x, (y * 50) - startPos.y, 0);
                tiles.Add(count, newTile.GetComponent<Editor_Tile>());
                count++;
            }
        }
        // Wall and doors
        int currentDoors = 0;
        Dictionary<int, GameObject> doors = new();
        for (int i = 0; i < 2 * (int)(room.roomSize.x + room.roomSize.y); i++)
            doors.Add(i, null);
        for (int y = 0; y < room.roomSize.y; y++)
        {
            for(int x = 0; x < room.roomSize.x; x++)
            {
                Vector3 pos = new((x * 300) - startPos.x + 150, (y * 300) - startPos.y + 150, 0);
                if (x == 0)
                {
                    // Left walls
                    GameObject newObj = Instantiate(doorPrefab, workspace);
                    newObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    newObj.GetComponent<RectTransform>().localPosition = pos;
                    doors[currentDoors] = newObj;
                    currentDoors++;
                }
                if (y == 0)
                {
                    // Bottom walls
                    GameObject newObj = Instantiate(doorPrefab, workspace);
                    newObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    newObj.GetComponent<RectTransform>().localPosition = pos;
                    doors[currentDoors] = newObj;
                    currentDoors++;
                }
                if (x == room.roomSize.x - 1)
                {
                    // Right walls
                    GameObject newObj = Instantiate(doorPrefab, workspace);
                    newObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                    newObj.GetComponent<RectTransform>().localPosition = pos;
                    doors[currentDoors] = newObj;
                    currentDoors++;
                }
                if (y == room.roomSize.y - 1)
                {
                    // Upper walls
                    GameObject newObj = Instantiate(doorPrefab, workspace);
                    newObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    newObj.GetComponent<RectTransform>().localPosition = pos;
                    doors[currentDoors] = newObj;
                    currentDoors++;
                }
            }
        }

        // Set up doors
        room.doors = new();
        if(customDoors != null)
        {
            for (int i = 0; i < customDoors.Count; i++)
            {
                room.doors.Add(i, doors[i].GetComponent<Editor_Door>());
                room.doors[i].doorState = customDoors[i].doorState;
                switch (room.doors[i].doorState)
                {
                    case 1:
                        doors[i].transform.Find("Door").GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        doors[i].transform.Find("Door").GetComponent<Image>().color = Color.black;
                        break;
                    default:
                        doors[i].transform.Find("Door").GetComponent<Image>().color = Color.blue;
                        break;
                }
            }
        }
        else
        {
            foreach (int id in doors.Keys)
            {
                room.doors.Add(id, doors[id].GetComponent<Editor_Door>());
                switch (room.doors[id].doorState)
                {
                    case 1:
                        doors[id].transform.Find("Door").GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        doors[id].transform.Find("Door").GetComponent<Image>().color = Color.black;
                        break;
                    default:
                        doors[id].transform.Find("Door").GetComponent<Image>().color = Color.blue;
                        break;
                }
            }
        }


        // Set first brush description
        brushType = BrushType.None;
        brushDescription.text = "None\nBasically an eraser";
    }
    public void DoorsDone_button()
    {
        newRoomTab.SetActive(true);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
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
        for(int i = 0; i < customDoorsTilesTransformParent.childCount; i++)
            Destroy(customDoorsTilesTransformParent.GetChild(i).gameObject);

        customDoors = new();
        for (int i = 0; i < 2 * (int)(room.roomSize.x + room.roomSize.y); i++)
            customDoors.Add(i, null);
        int currentDoorId = 0;
        customDoorsTilesTransformParent.GetComponent<GridLayoutGroup>().constraintCount = (int)room.roomSize.x;
        for (int y = 0; y < room.roomSize.y; y++)
        {
            for (int x = 0; x < room.roomSize.x; x++)
            {
                GameObject newTile = Instantiate(customDoorsTilePrefab, customDoorsTilesTransformParent);
                if(x == 0)
                {
                    // Left wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(-45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_CustomDoor>();
                    currentDoorId++;
                }
                if (y == 0)
                {
                    // Bottom wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 0, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_CustomDoor>();
                    currentDoorId++;
                }
                if (x == room.roomSize.x - 1)
                {
                    // Right wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(45, 45, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_CustomDoor>();
                    currentDoorId++;
                }
                if (y == room.roomSize.y - 1)
                {
                    // Upper wall
                    GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                    newDoor.GetComponent<RectTransform>().localPosition = new(0, 90, 0);
                    newDoor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_CustomDoor>();
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
    public void BossType(TMP_Dropdown dropdown)
    {
        room.bossName = dropdown.options[dropdown.value].text;
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

        if(room.roomType == Editor_Room.RoomType.Boss)
        {
            room.bossRoom = true;
            bossTypeDropdown.SetActive(true);
        }
        else
        {
            room.bossRoom = false;
            bossTypeDropdown.SetActive(false);
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
    public string bossName;
    public RoomType roomType;

    public bool bossRoom;
    public Vector2 roomSize;
    public Dictionary<int, Editor_Door> doors;
    public SerializableDictionary<int, int> doorsSave = new();
    public SerializableDictionary<int, string> tiles = new();


    public void SaveToFile(UnityEngine.Object saveFolder, Dictionary<int, Editor_Tile> tiles)
    {
        foreach (int i in tiles.Keys)
        {
            string value = "";
            if (tiles[i].specificObjName == null || tiles[i].specificObjName == "")
            {
                switch (tiles[i].tileType)
                {
                    case Editor.BrushType.None:
                        value = 0.ToString();
                        break;
                    case Editor.BrushType.Obstacle_noShoot:
                        value = 1.ToString();
                        break;
                    case Editor.BrushType.Obstacle_shoot:
                        value = 2.ToString();
                        break;
                    case Editor.BrushType.Lightsource:
                        value = 3.ToString();
                        break;
                    case Editor.BrushType.Resource:
                        value = 4.ToString();
                        break;
                    case Editor.BrushType.Lootbox:
                        value = 5.ToString();
                        break;
                    case Editor.BrushType.Enemy_mAggresive:
                        value = 6.ToString();
                        break;
                    case Editor.BrushType.Enemy_mEvasive:
                        value = 7.ToString();
                        break;
                    case Editor.BrushType.Enemy_mWandering:
                        value = 8.ToString();
                        break;
                    case Editor.BrushType.Enemy_mStealth:
                        value = 9.ToString();
                        break;
                    case Editor.BrushType.Enemy_rStatic:
                        value = 10.ToString();
                        break;
                    case Editor.BrushType.Enemy_rWandering:
                        value = 11.ToString();
                        break;
                    case Editor.BrushType.Trap:
                        value = 12.ToString();
                        break;
                }
            }
            else
                value = tiles[i].specificObjName;
            this.tiles.Add(i, value);
        }
        // Make some normal IDs for doors (cause I'm too stoopid to do that right away -_- )
        if (roomSize == new Vector2(1, 1))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[3].doorState },
                { 3, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(1, 2))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[4].doorState },
                { 3, doors[5].doorState },
                { 4, doors[3].doorState },
                { 5, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(1, 3))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[4].doorState },
                { 3, doors[6].doorState },
                { 4, doors[7].doorState },
                { 5, doors[5].doorState },
                { 6, doors[3].doorState },
                { 7, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(2, 1))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[3].doorState },
                { 2, doors[4].doorState },
                { 3, doors[5].doorState },
                { 4, doors[2].doorState },
                { 5, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(2, 2))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[3].doorState },
                { 3, doors[6].doorState },
                { 4, doors[7].doorState },
                { 5, doors[5].doorState },
                { 6, doors[4].doorState },
                { 7, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(2, 3))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[3].doorState },
                { 3, doors[5].doorState },
                { 4, doors[8].doorState },
                { 5, doors[9].doorState },
                { 6, doors[7].doorState },
                { 7, doors[6].doorState },
                { 8, doors[4].doorState },
                { 9, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(3, 1))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[3].doorState },
                { 2, doors[5].doorState },
                { 3, doors[6].doorState },
                { 4, doors[7].doorState },
                { 5, doors[4].doorState },
                { 6, doors[2].doorState },
                { 7, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(3, 2))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[3].doorState },
                { 3, doors[4].doorState },
                { 4, doors[8].doorState },
                { 5, doors[9].doorState },
                { 6, doors[7].doorState },
                { 7, doors[6].doorState },
                { 8, doors[5].doorState },
                { 9, doors[0].doorState },
            };
        }
        else if (roomSize == new Vector2(3, 3))
        {
            doorsSave = new()
            {
                { 0, doors[1].doorState },
                { 1, doors[2].doorState },
                { 2, doors[3].doorState },
                { 3, doors[4].doorState },
                { 4, doors[6].doorState },
                { 5, doors[10].doorState },
                { 6, doors[11].doorState },
                { 7, doors[9].doorState },
                { 8, doors[8].doorState },
                { 9, doors[7].doorState },
                { 10, doors[5].doorState },
                { 11, doors[0].doorState },
            };
        }
        // Set path
        string path = Path.GetFullPath(AssetDatabase.GetAssetPath(saveFolder));

        // Set proper name to a save file
        string name = "dungeonRoom_" + (new DirectoryInfo(path).EnumerateFiles().Count() / 2).ToString() + ".txt";


        string fullPath = Path.Combine(path , name);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(this, true);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(dataToStore);
            writer.Close();
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file " + fullPath + "\n" + e);
        }
    }
}
