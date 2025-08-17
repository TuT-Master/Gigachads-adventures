using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_Tile : MonoBehaviour, IPointerDownHandler
{
    public Editor.BrushType tileType;
    public Vector2Int position;
    public string specificObjName;

    private Editor editor;
    [SerializeField] private Image image;


    public void SetUpTile(Vector2Int pos, Editor editor)
    {
        position = pos;
        this.editor = editor;
    }

    public void OnPointerDown(PointerEventData eventData) => editor.TileClicked(this);

    public void SetVisual(Editor.BrushType brushType, bool setBrushTypeToo = true)
    {
        switch (brushType)
        {
            case Editor.BrushType.None:
                image.color = Color.white;
                break;
            case Editor.BrushType.Obstacle_noShoot_1x1:
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_noShoot_2x1:
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_noShoot_3x1:
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_noShoot_2x2:
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_shoot_1x1:
                image.color = Color.gray;
                break;
            case Editor.BrushType.Lightsource:
                image.color = new(1, 1, 0);
                break;
            case Editor.BrushType.Resource:
                image.color = new(0, 1, 1);
                break;
            case Editor.BrushType.Lootbox:
                image.color = Color.yellow;
                break;
            case Editor.BrushType.Enemy_mAggresive:
                image.color = Color.red;
                break;
            case Editor.BrushType.Enemy_mEvasive:
                image.color = Color.red;
                break;
            case Editor.BrushType.Enemy_mWandering:
                image.color = Color.red;
                break;
            case Editor.BrushType.Enemy_mStealth:
                image.color = Color.red;
                break;
            case Editor.BrushType.Enemy_rStatic:
                image.color = Color.red;
                break;
            case Editor.BrushType.Enemy_rWandering:
                image.color = Color.red;
                break;
            case Editor.BrushType.Trap:
                image.color = new(1, 0.4f, 0.2f);
                break;
            case Editor.BrushType.Specific:
                image.color = new(1, 0, 1);
                break;
        }
        if(setBrushTypeToo)
        {
            tileType = brushType;
            if (tileType == Editor.BrushType.Specific)
                specificObjName = editor.specificObjName;
            else
                specificObjName = "";
        }
    }
}
