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
                tileType = Editor.BrushType.None;
                image.color = Color.white;
                break;
            case Editor.BrushType.Obstacle_noShoot:
                tileType = Editor.BrushType.Obstacle_noShoot;
                image.color = Color.black;
                break;
            case Editor.BrushType.Obstacle_shoot:
                tileType = Editor.BrushType.Obstacle_shoot;
                image.color = Color.gray;
                break;
        }
    }
}
