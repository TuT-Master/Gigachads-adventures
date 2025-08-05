using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDatabase", menuName = "Scriptable objects/Dungeon database")]
public class DungeonDatabase : ScriptableObject
{
    public GameObject floorMousePointer;

    public List<GameObject> floors;

    public List<GameObject> walls;

    public List<Material> wallMaterials;

    public List<GameObject> doors;

    public List<GameObject> obstacles_noShoot;
    public List<GameObject> obstacles_shoot;
    public List<GameObject> resources;
    public List<GameObject> lightsources;
    public List<GameObject> lootBoxes;
    public List<GameObject> enemies_mAgressive;
    public List<GameObject> enemies_mEvasive;
    public List<GameObject> enemies_mStealth;
    public List<GameObject> enemies_mWandering;
    public List<GameObject> enemies_rStatic;
    public List<GameObject> enemies_rWandering;
    public List<GameObject> enemies_boss;
    public List<GameObject> traps;
    public GameObject GetRandomPop(int tileType)
    {
        List<GameObject> list = null;
        switch (tileType)
        {
            case 1:
                list = obstacles_noShoot;
                break;
            case 2:
                list = obstacles_shoot;
                break;
            case 3:
                list = lightsources;
                break;
            case 4:
                list = resources;
                break;
            case 5:
                list = lootBoxes;
                break;
            case 6:
                list = enemies_mAgressive;
                break;
            case 7:
                list = enemies_mEvasive;
                break;
            case 8:
                list = enemies_mWandering;
                break;
            case 9:
                list = enemies_mStealth;
                break;
            case 10:
                list = enemies_rStatic;
                break;
            case 11:
                list = enemies_rWandering;
                break;
            case 12:
                list = traps;
                break;
        }
        if (list != null)
            return list[new System.Random().Next(list.Count)];
        else
            return null;
    }
    public GameObject GetPopByName(string name)
    {
        foreach (GameObject go in enemies_boss)
            if (go.name == name)
                return go;
        foreach (GameObject go in traps)
            if (go.name == name)
                return go;
        foreach (GameObject go in obstacles_noShoot)
            if(go.name == name)
                return go;
        foreach (GameObject go in obstacles_shoot)
            if (go.name == name)
                return go;
        foreach (GameObject go in resources)
            if (go.name == name)
                return go;
        foreach (GameObject go in lightsources)
            if (go.name == name)
                return go;
        foreach (GameObject go in lootBoxes)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_mAgressive)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_mEvasive)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_mStealth)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_mWandering)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_rStatic)
            if (go.name == name)
                return go;
        foreach (GameObject go in enemies_rWandering)
            if (go.name == name)
                return go;
        return null;
    }


    public List<Material> weaponMaterials;

    public List<string> firstChampionNames;

    public List<string> secondChampionNames;

    [Header("Floors")]
    [SerializeField] private GameObject floor_1x1;
    [SerializeField] private GameObject floor_1x2;
    [SerializeField] private GameObject floor_1x3;
    [SerializeField] private GameObject floor_2x1;
    [SerializeField] private GameObject floor_2x2;
    [SerializeField] private GameObject floor_2x3;
    [SerializeField] private GameObject floor_3x1;
    [SerializeField] private GameObject floor_3x2;
    [SerializeField] private GameObject floor_3x3;

    [Header("Ceiling")]
    [SerializeField] private GameObject ceiling_1x1;
    [SerializeField] private GameObject ceiling_1x2;
    [SerializeField] private GameObject ceiling_1x3;
    [SerializeField] private GameObject ceiling_2x1;
    [SerializeField] private GameObject ceiling_2x2;
    [SerializeField] private GameObject ceiling_2x3;
    [SerializeField] private GameObject ceiling_3x1;
    [SerializeField] private GameObject ceiling_3x2;
    [SerializeField] private GameObject ceiling_3x3;

    [Header("Walls")]
    [SerializeField] private GameObject wall_1_0;
    [SerializeField] private GameObject wall_1_1;
    [SerializeField] private GameObject wall_2_00;
    [SerializeField] private GameObject wall_2_01;
    [SerializeField] private GameObject wall_2_10;
    [SerializeField] private GameObject wall_2_11;
    [SerializeField] private GameObject wall_3_000;
    [SerializeField] private GameObject wall_3_001;
    [SerializeField] private GameObject wall_3_010;
    [SerializeField] private GameObject wall_3_011;
    [SerializeField] private GameObject wall_3_100;
    [SerializeField] private GameObject wall_3_101;
    [SerializeField] private GameObject wall_3_110;
    [SerializeField] private GameObject wall_3_111;

    [Header("Rooms")]
    [SerializeField] private UnityEngine.Object saveFile;
    public List<Editor_Room> dungeonRooms = new();
    public void LoadRooms()
    {
        string path = Path.GetFullPath(AssetDatabase.GetAssetPath(saveFile));
        foreach(var roomFile in new DirectoryInfo(path).EnumerateFiles())
        {
            if(roomFile.Name.Contains(".meta"))
                continue;
            string fullPath = Path.GetFullPath(Path.Combine(path, roomFile.Name));
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = string.Empty;
                    using (FileStream stream = new(fullPath, FileMode.Open))
                    {
                        using StreamReader read = new(stream);
                        dataToLoad = read.ReadToEnd();
                    }
                    Editor_Room newRoom = JsonUtility.FromJson<Editor_Room>(dataToLoad);
                    newRoom.SetDoorsFromSave();
                    dungeonRooms.Add(newRoom);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file " + fullPath + "\n" + e);
                }
            }
        }
    }
    public Editor_Room GetRoomByType(Editor_Room.RoomType type)
    {
        List<Editor_Room> rooms = new();
        foreach(var room in dungeonRooms)
            if(room.roomType == type)
                rooms.Add(room);
        if (rooms.Count > 0)
            return rooms[new System.Random().Next(rooms.Count)];
        else
            return null;
    }


    public Material GetWeaponMaterial(string name)
    {
        foreach (Material mat in weaponMaterials)
            if(mat.name == name)
                return mat;
        return null;
    }
    public List<GameObject> GetAllPlaceableObjs()
    {
        List<GameObject> objs = new();
        foreach(GameObject go in obstacles_noShoot)
            objs.Add(go);
        foreach (GameObject go in obstacles_shoot)
            objs.Add(go);
        foreach (GameObject go in resources)
            objs.Add(go);
        foreach (GameObject go in lootBoxes)
            objs.Add(go);
        foreach (GameObject go in lightsources)
            objs.Add(go);
        foreach (GameObject go in enemies_mAgressive)
            objs.Add(go);
        foreach (GameObject go in enemies_mEvasive)
            objs.Add(go);
        foreach (GameObject go in enemies_mWandering)
            objs.Add(go);
        foreach (GameObject go in enemies_mStealth)
            objs.Add(go);
        foreach (GameObject go in enemies_rStatic)
            objs.Add(go);
        foreach (GameObject go in enemies_rWandering)
            objs.Add(go);
        foreach (GameObject go in enemies_boss)
            objs.Add(go);
        foreach (GameObject go in traps)
            objs.Add(go);
        return objs;
    }
    public GameObject GetFloorBySize(Vector2 size)
    {
        if (size == new Vector2(1, 1))
            return floor_1x1;
        else if (size == new Vector2(1, 2))
            return floor_1x2;
        else if (size == new Vector2(1, 3))
            return floor_1x3;
        else if (size == new Vector2(2, 1))
            return floor_2x1;
        else if (size == new Vector2(2, 2))
            return floor_2x2;
        else if (size == new Vector2(2, 3))
            return floor_2x3;
        else if (size == new Vector2(3, 1))
            return floor_3x1;
        else if (size == new Vector2(3, 2))
            return floor_3x2;
        else if (size == new Vector2(3, 3))
            return floor_3x3;
        else
            return null;
    }
    public GameObject GetCeilingBySize(Vector2 size)
    {
        if (size == new Vector2(1, 1))
            return ceiling_1x1;
        else if (size == new Vector2(1, 2))
            return ceiling_1x2;
        else if (size == new Vector2(1, 3))
            return ceiling_1x3;
        else if (size == new Vector2(2, 1))
            return ceiling_2x1;
        else if (size == new Vector2(2, 2))
            return ceiling_2x2;
        else if (size == new Vector2(2, 3))
            return ceiling_2x3;
        else if (size == new Vector2(3, 1))
            return ceiling_3x1;
        else if (size == new Vector2(3, 2))
            return ceiling_3x2;
        else if (size == new Vector2(3, 3))
            return ceiling_3x3;
        else
            return null;
    }
    public GameObject GetWallBySizeAndDoors(int lenght, bool[] doors)
    {
        if (lenght == 1)
        {
            if(doors[0])
                return wall_1_1;
            else
                return wall_1_0;
        }
        else if (lenght == 2)
        {
            if (doors[0] && doors[1])
                return wall_2_11;
            else if (doors[0] && !doors[1])
                return wall_2_10;
            else if (!doors[0] && doors[1])
                return wall_2_01;
            else if (!doors[0] && !doors[1])
                return wall_2_00;
            else
                return null;
        }
        else if (lenght == 3)
        {
            if (doors[0] && doors[1] && doors[2])
                return wall_3_111;
            else if (!doors[0] && doors[1] && doors[2])
                return wall_3_100;
            else if (doors[0] && !doors[1] && doors[2])
                return wall_3_010;
            else if (!doors[0] && !doors[1] && doors[2])
                return wall_3_001;
            else if (doors[0] && doors[1] && !doors[2])
                return wall_3_110;
            else if (!doors[0] && doors[1] && !doors[2])
                return wall_3_101;
            else if (doors[0] && !doors[1] && !doors[2])
                return wall_3_011;
            else if (!doors[0] && !doors[1] && !doors[2])
                return wall_3_000;
            else
                return null;
        }
        else
            return null;
    }
}