using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private void OnEnable()
    {
        FindAnyObjectByType<DungeonGenerator>().BuildDungeon();
    }

    private void OnDisable()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
