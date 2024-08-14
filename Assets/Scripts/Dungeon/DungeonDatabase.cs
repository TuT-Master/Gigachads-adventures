using System.Collections;
using System.Collections.Generic;
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

    public List<Material> weaponMaterials;

    public List<string> firstChampionNames;

    public List<string> secondChampionNames;


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
}