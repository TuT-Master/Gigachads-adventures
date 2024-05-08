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

    public List<GameObject> obstacles;

    public List<GameObject> resources;

    public List<GameObject> lootBoxes;

    public List<string> firstChampionNames;

    public List<string> secondChampionNames;

    public List<GameObject> meleeEnemies;

    public List<Material> weaponMaterials;


    public Material GetWeaponMaterial(string name)
    {
        foreach (Material mat in weaponMaterials)
            if(mat.name == name)
                return mat;
        return null;
    }
}