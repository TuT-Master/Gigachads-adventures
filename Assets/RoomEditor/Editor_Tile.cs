using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_Tile : MonoBehaviour, IPointerDownHandler
{
    public Editor.BrushType tileType;
    public Vector2 position;

    private Editor editor;
    [SerializeField] private Image image;


    public void SetUpTile(Vector2 pos, Editor editor)
    {
        position = pos;
        this.editor = editor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(editor.brushType)
        {
            case Editor.BrushType.None:
                image.color = Color.white;
                break;
            case Editor.BrushType.Obstacle_noShoot:
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_shoot:
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
        tileType = editor.brushType;
    }
}
