using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomUI : MonoBehaviour
{
    private DungeonRoom room;


    void Start()
    {
        room = GetComponent<DungeonRoom>();
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, room.size.x * 30);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, room.size.y * 30);
    }

    void Update()
    {
        
    }
}
