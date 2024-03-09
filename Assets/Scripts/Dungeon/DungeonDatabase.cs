using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDatabase", menuName = "Scriptable objects/Dungeon database")]
public class DungeonDatabase : ScriptableObject
{
    public GameObject floorMousePointer;

    public List<GameObject> floors;

    public List<GameObject> walls;

    public List<GameObject> doors;
}
