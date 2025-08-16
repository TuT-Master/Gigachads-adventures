using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<BrushType, int> statistics = new();
    [SerializeField] private TMP_Dropdown dropdownBrushes;
    public enum BrushType
    {
        None,
        Obstacle_noShoot_1x1,
        Obstacle_noShoot_2x1,
        Obstacle_noShoot_3x1,
        Obstacle_noShoot_2x2,
        Obstacle_shoot_1x1,
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
    public Brush activeBrush;
    public string specificObjName;
    [SerializeField] private GameObject specificObjsContectArea;
    [SerializeField] private GameObject specificObjPrefab;
    [SerializeField] private Transform workspace;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject doorPrefab;
    private Editor_Tile[,] tiles;

    private Editor_Room room;
    private Dictionary<int, Editor_CustomDoor> customDoors;
    private bool deletingLog = false;


    private void Start()
    {
        newRoomTab.SetActive(false);
        editorTab.SetActive(false);
        customDoorsTab.SetActive(false);
        bossTypeDropdown.SetActive(false);

        SetDropDownBrushes();
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
        statistics.Clear();
        for (int i = 0; i < brushes.Count; i++)
            statistics.Add(brushes[i].BrushType, 0);
        foreach (Editor_Tile tile in tiles)
            if (tile.tileType != BrushType.None)
                statistics[tile.tileType]++;
        foreach (BrushType brushType in statistics.Keys)
            statisticsText.text += $"{brushType}: {statistics[brushType]}\n";
    }
    private IEnumerator DeleteLog(float delay)
    {
        deletingLog = true;
        yield return new WaitForSecondsRealtime(delay);
        log.text = "";
        deletingLog = false;
    }

    private void SetDropDownBrushes()
    {
        dropdownBrushes.ClearOptions();
        for (int i = 0; i < brushes.Count; i++)
        {
            TMP_Dropdown.OptionData option = new()
            {
                text = brushes[i].Name,
            };
            dropdownBrushes.options.Add(option);
        }
    }

    // Brushes
    private List<Brush> brushes = new()
    {
        new Brush_none(),
        new Brush_obstacleNoShoot1x1(),
        new Brush_obstacleNoShoot2x1(),
        new Brush_obstacleNoShoot3x1(),
        new Brush_obstacleNoShoot2x2(),
        new Brush_obstacleShoot1x1(),
        new Brush_lightsource(),
        new Brush_lootbox(),
        new Brush_resource(),
        new Brush_enemyMeleeAggresive(),
        new Brush_enemyMeleeEvasive(),
        new Brush_enemyMeleeWandering(),
        new Brush_enemyMeleeStealth(),
        new Brush_enemyRangedStatic(),
        new Brush_enemyRangedWandering(),
        new Brush_trap(),
    };
    public void BrushChanged(TMP_Dropdown dropdown)
    {
        bool brushFound = false;
        foreach (Brush b in brushes)
        {
            if (b.Name == dropdown.options[dropdown.value].text)
            {
                activeBrush = b;
                brushDescription.text = b.Description;
                brushFound = true;
                break;
            }
        }
        if(!brushFound)
        {
            Debug.LogError("Brush not found: " + dropdown.options[dropdown.value].text);
            return;
        }
        if (activeBrush.BrushType == BrushType.Specific)
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
            for (int i = 0; i < specificObjsContectArea.transform.childCount; i++)
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
            room.SaveToFile(tiles);
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
        tiles = new Editor_Tile[(int)room.roomSize.x * 6, (int)room.roomSize.y * 6];
        Vector2 startPos = new(room.roomSize.x * 150, room.roomSize.y * 150);
        for (int y = 0; y < room.roomSize.y * 6; y++)
        {
            for (int x = 0; x < room.roomSize.x * 6; x++)
            {
                GameObject newTile = Instantiate(tilePrefab, workspace);
                newTile.GetComponent<Editor_Tile>().SetUpTile(new(x, y), this);
                newTile.GetComponent<RectTransform>().localPosition = new Vector3((x * 50) - startPos.x, (y * 50) - startPos.y, 0);
                tiles[x, y] = newTile.GetComponent<Editor_Tile>();
            }
        }
        // Wall and doors
        int currentDoors = 0;
        Dictionary<int, GameObject> doors = new();
        for (int i = 0; i < 2 * (int)(room.roomSize.x + room.roomSize.y); i++)
            doors.Add(i, null);

        var wallConfigs = new[]
        {
            new { Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => x == 0), Rot = new Vector3(0, 0, 90) },                  // Left
            new { Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => y == 0), Rot = new Vector3(0, 0, 180) },                 // Bottom
            new { Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => x == r.roomSize.x - 1), Rot = new Vector3(0, 0, -90) },  // Right
            new { Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => y == r.roomSize.y - 1), Rot = new Vector3(0, 0, 0) },    // Top
        };
        for (int y = 0; y < room.roomSize.y; y++)
        {
            for (int x = 0; x < room.roomSize.x; x++)
            {
                Vector3 pos = new((x * 300) - startPos.x + 150, (y * 300) - startPos.y + 150, 0);
                foreach (var config in wallConfigs)
                {
                    if (config.Check(x, y, room))
                    {
                        GameObject newObj = Instantiate(doorPrefab, workspace);
                        RectTransform rt = newObj.GetComponent<RectTransform>();
                        rt.rotation = Quaternion.Euler(config.Rot);
                        rt.localPosition = pos;

                        doors[currentDoors] = newObj;
                        currentDoors++;
                    }
                }
            }
        }

        // Set up doors
        room.doors = new();
        if (customDoors != null)
        {
            for (int i = 0; i < customDoors.Count; i++)
            {
                room.doors.Add(i, doors[i].GetComponent<Editor_Door>());
                room.doors[i].doorState = customDoors[i].doorState;
                doors[i].transform.Find("Door").GetComponent<Image>().color = room.doors[i].doorState switch
                {
                    1 => Color.green,
                    2 => Color.black,
                    _ => Color.blue,
                };
            }
        }
        else
        {
            foreach (int id in doors.Keys)
            {
                room.doors.Add(id, doors[id].GetComponent<Editor_Door>());
                doors[id].transform.Find("Door").GetComponent<Image>().color = room.doors[id].doorState switch
                {
                    1 => Color.green,
                    2 => Color.black,
                    _ => Color.blue,
                };
            }
        }
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
        for (int i = 0; i < customDoorsTilesTransformParent.childCount; i++)
            Destroy(customDoorsTilesTransformParent.GetChild(i).gameObject);

        customDoors = new();
        for (int i = 0; i < 2 * (int)(room.roomSize.x + room.roomSize.y); i++)
            customDoors.Add(i, null);
        int currentDoorId = 0;
        customDoorsTilesTransformParent.GetComponent<GridLayoutGroup>().constraintCount = (int)room.roomSize.x;

        var wallConfigs = new[]
        {
            new { Name = "Left", Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => x == 0), Pos = new Vector3(-45, 45, 0), Rot = new Vector3(0, 0, 90) },
            new { Name = "Bottom", Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => y == 0), Pos = new Vector3(0, 0, 0),    Rot = new Vector3(0, 0, 180) },
            new { Name = "Right", Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => x == r.roomSize.x - 1), Pos = new Vector3(45, 45, 0),  Rot = new Vector3(0, 0, -90) },
            new { Name = "Top", Check = (Func<int, int, Editor_Room, bool>)((x, y, r) => y == r.roomSize.y - 1), Pos = new Vector3(0, 90, 0),   Rot = new Vector3(0, 0, 0) },
        };
        for (int y = 0; y < room.roomSize.y; y++)
        {
            for (int x = 0; x < room.roomSize.x; x++)
            {
                GameObject newTile = Instantiate(customDoorsTilePrefab, customDoorsTilesTransformParent);
                foreach (var config in wallConfigs)
                {
                    if (config.Check(x, y, room))
                    {
                        GameObject newDoor = Instantiate(customDoorsWallPrefab, newTile.transform);
                        RectTransform rt = newDoor.GetComponent<RectTransform>();
                        rt.localPosition = config.Pos;
                        rt.rotation = Quaternion.Euler(config.Rot);

                        customDoors[currentDoorId] = newDoor.GetComponentInChildren<Editor_CustomDoor>();
                        currentDoorId++;
                    }
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
        room.roomType = dropdown.value switch
        {
            0 => Editor_Room.RoomType.Basic,
            1 => Editor_Room.RoomType.Resources,
            2 => Editor_Room.RoomType.Start,
            3 => Editor_Room.RoomType.Boss,
            _ => Editor_Room.RoomType.Basic,
        };
        if (room.roomType == Editor_Room.RoomType.Boss)
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

    public void TileClicked(Editor_Tile tile)
    {
        activeBrush.Paint(tiles, tile.position.x, tile.position.y);
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


    public void SaveToFile(Editor_Tile[,] tiles)
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                Editor_Tile tile = tiles[x, y];
                string value;
                if (tile.specificObjName == null || tile.specificObjName == "")
                    value = ((int)tile.tileType).ToString();
                else
                    value = tile.specificObjName;
                this.tiles.Add((tiles.GetLength(0) * y) + x, value);
            }
        }
        // Make some normal IDs for doors (cause I'm too stoopid to do that right away -_- )
        Dictionary<Vector2, int[]> doorMappings = new()
        {
            [new Vector2(1, 1)] = new[] { 1, 2, 3, 0 },
            [new Vector2(1, 2)] = new[] { 1, 2, 4, 5, 3, 0 },
            [new Vector2(1, 3)] = new[] { 1, 2, 4, 6, 7, 5, 3, 0 },
            [new Vector2(2, 1)] = new[] { 1, 3, 4, 5, 2, 0 },
            [new Vector2(2, 2)] = new[] { 1, 2, 3, 6, 7, 5, 4, 0 },
            [new Vector2(2, 3)] = new[] { 1, 2, 3, 5, 8, 9, 7, 6, 4, 0 },
            [new Vector2(3, 1)] = new[] { 1, 3, 5, 6, 7, 4, 2, 0 },
            [new Vector2(3, 2)] = new[] { 1, 2, 3, 4, 8, 9, 7, 6, 5, 0 },
            [new Vector2(3, 3)] = new[] { 1, 2, 3, 4, 6, 10, 11, 9, 8, 7, 5, 0 },
        };

        if (doorMappings.TryGetValue(roomSize, out int[] mapping))
        {
            doorsSave = new();
            for (int i = 0; i < mapping.Length; i++)
                doorsSave[i] = doors[mapping[i]].doorState;
        }
        else
            Debug.LogWarning($"Unsupported room size: {roomSize}");

        // Path
        string path = Path.Combine(Application.dataPath, "Resources", "DungeonRooms");

        // Count existing files
        int fileCount = Directory.GetFiles(path, "*.txt").Length;
        string name = $"dungeonRoom_{fileCount}.txt";

        string fullPath = Path.Combine(path, name);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(this, true);

            File.WriteAllText(fullPath, dataToStore);

            AssetDatabase.Refresh();
            Debug.Log($"Room saved to {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving room file: {e}");
        }
    }

    public void SetDoorsFromSave()
    {
        doors = new();
        foreach (int key in doorsSave.Keys)
            doors.Add(key, new() { doorState = doorsSave[key], id = key });
    }
}
