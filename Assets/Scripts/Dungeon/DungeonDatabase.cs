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

    public List<GameObject> meleeEnemies;
}