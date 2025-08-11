using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject currentRoom;

    private void Update()
    {
        if (transform.childCount > 0)
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).gameObject.activeInHierarchy)
                    currentRoom = transform.GetChild(i).gameObject;
    }

    private void OnDisable()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        FindAnyObjectByType<DungeonMap>().ClearMap();
    }
}
