using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDatabase", menuName = "Scriptable objects/Dungeon database")]
public class DungeonDatabase : ScriptableObject
{
    public GameObject roomPrefab;

    public List<GameObject> floors;

    public List<GameObject> walls;

    public List<Material> wallMaterials;

    public List<GameObject> doors;

    public List<GameObject> obstacles_noShoot_1x1;
    public List<GameObject> obstacles_noShoot_2x1;
    public List<GameObject> obstacles_noShoot_3x1;
    public List<GameObject> obstacles_noShoot_2x2;
    public List<GameObject> obstacles_shoot_1x1;
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

    public GameObject GetPopulationByName(string name)
    {
        foreach (GameObject go in enemies_boss)
            if (go.name == name)
                return go;
        foreach (GameObject go in traps)
            if (go.name == name)
                return go;
        foreach (GameObject go in obstacles_noShoot_1x1)
            if(go.name == name)
                return go;
        foreach (GameObject go in obstacles_shoot_1x1)
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
    public GameObject floor;

    [Header("Ceiling")]
    public GameObject ceiling;

    [Header("Walls")]
    public GameObject wall_noDoors;
    public GameObject wall_withDoors;

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

        objs.AddRange(obstacles_noShoot_1x1);
        objs.AddRange(obstacles_shoot_1x1);
        objs.AddRange(resources);
        objs.AddRange(lootBoxes);
        objs.AddRange(lightsources);
        objs.AddRange(enemies_mAgressive);
        objs.AddRange(enemies_mEvasive);
        objs.AddRange(enemies_mWandering);
        objs.AddRange(enemies_mStealth);
        objs.AddRange(enemies_rStatic);
        objs.AddRange(enemies_rWandering);
        objs.AddRange(enemies_boss);
        objs.AddRange(traps);

        return objs;
    }
    public string GetRandomFirstChampionName()
    {
        System.Random rnd = new();
        return firstChampionNames[rnd.Next(firstChampionNames.Count)];
    }
    public string GetRandomLastChampionName()
    {
        System.Random rnd = new();
        return secondChampionNames[rnd.Next(secondChampionNames.Count)];
    }
}