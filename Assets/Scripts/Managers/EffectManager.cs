using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> bloodStains;
    [SerializeField]
    private List<Material> poisonStains;

    [SerializeField]
    private GameObject stainPrefab;

    public enum StainType
    {
        Blood,
        Poison
    }

    private Dungeon dungeon;
    private GameObject currentRoom;


    private void Update()
    {
        if(dungeon == null)
            dungeon = FindAnyObjectByType<Dungeon>();
        else
            currentRoom = dungeon.currentRoom;
    }

    public void SpawnStain(Transform t, StainType stainType)
    {
        System.Random rand = new();
        GameObject newStain = Instantiate(stainPrefab, new Vector3((float)(t.position.x + (rand.Next(100) / 100)), 0.001f, (float)(t.position.z + (rand.Next(100) / 100))), Quaternion.Euler(90, 0, 0), currentRoom.transform);
        newStain.GetComponent<MeshRenderer>().material = GetRandomMaterial(stainType);
        rand = new();
        newStain.transform.localScale = new(10f / rand.Next(10, 20), 10f / rand.Next(10, 20), 1);
    }

    private Material GetRandomMaterial(StainType stainType)
    {
        System.Random rnd = new();
        return stainType switch
        {
            StainType.Blood => bloodStains[rnd.Next(bloodStains.Count - 1)],
            StainType.Poison => poisonStains[rnd.Next(poisonStains.Count - 1)],
            _ => null,
        };
    }
}
