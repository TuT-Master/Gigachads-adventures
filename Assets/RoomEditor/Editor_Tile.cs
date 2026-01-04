using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_Tile : MonoBehaviour, IPointerDownHandler
{
    public RoomEditor.BrushType tileType;
    public Vector2Int position;
    public string specificObjName;

    private RoomEditor editor;
    [SerializeField] private Image image;


    public void SetUpTile(Vector2Int pos, RoomEditor editor)
    {
        position = pos;
        this.editor = editor;
    }

    public void OnPointerDown(PointerEventData eventData) => editor.TileClicked(this);

    public void SetVisual(RoomEditor.BrushType brushType, bool setBrushTypeToo = true)
    {
        switch (brushType)
        {
            case RoomEditor.BrushType.None:
                image.color = Color.white;
                break;
            case RoomEditor.BrushType.Obstacle_noShoot_1x1:
                image.color = Color.black;
                break;
            case RoomEditor.BrushType.Obstacle_noShoot_2x1:
                image.color = Color.black;
                break;
            case RoomEditor.BrushType.Obstacle_noShoot_3x1:
                image.color = Color.black;
                break;
            case RoomEditor.BrushType.Obstacle_noShoot_2x2:
                image.color = Color.black;
                break;
            case RoomEditor.BrushType.Obstacle_shoot_1x1:
                image.color = Color.gray;
                break;
            case RoomEditor.BrushType.Lightsource:
                image.color = new(1, 1, 0);
                break;
            case RoomEditor.BrushType.Resource:
                image.color = new(0, 1, 1);
                break;
            case RoomEditor.BrushType.Lootbox:
                image.color = Color.yellow;
                break;
            case RoomEditor.BrushType.Enemy_mAggresive:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Enemy_mEvasive:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Enemy_mWandering:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Enemy_mStealth:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Enemy_rStatic:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Enemy_rWandering:
                image.color = Color.red;
                break;
            case RoomEditor.BrushType.Trap:
                image.color = new(1, 0.4f, 0.2f);
                break;
            case RoomEditor.BrushType.Specific:
                image.color = new(1, 0, 1);
                break;
        }
        if(setBrushTypeToo)
        {
            tileType = brushType;
            if (tileType == RoomEditor.BrushType.Specific)
                specificObjName = editor.specificObjName;
            else
                specificObjName = "";
        }
    }
}
