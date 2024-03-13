using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomUI : MonoBehaviour
{
    private DungeonRoom room;
    [SerializeField] private GameObject[] doors;


    public void SetRoomUp(DungeonRoom room)
    {
        this.room = room;

        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, room.size.x * 30);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, room.size.y * 30);

        for (int i = 0; i < 4; i++)
        {
            if (room.entrances[i])
                doors[i].SetActive(true);
            else
                doors[i].SetActive(false);
        }
    }
}
