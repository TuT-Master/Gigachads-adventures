using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    public bool mapOpened;

    [SerializeField]
    private GameObject dungeonMapCanvas;
    [SerializeField]
    private GameObject mapContentArea;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject roomPrefab;

    private GameObject[,] tileMap;

    private HUDmanager hudmanager;


    void Start()
    {
        hudmanager = GetComponent<HUDmanager>();
        ToggleMap(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            hudmanager.ToggleMap(!mapOpened);
    }

    public void BuildMap(Dictionary<Vector2, DungeonGenerator.Cell> board)
    {

    }

    public void DrawMap(Dictionary<Vector2, DungeonGenerator.Cell> board)
    {
        int size = (int)Mathf.Sqrt(board.Count);
        tileMap = new GameObject[size, size];
        mapContentArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * tilePrefab.GetComponent<RectTransform>().sizeDelta.x);
        mapContentArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size * tilePrefab.GetComponent<RectTransform>().sizeDelta.y);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 id = new(x, size - 1 - y);
                GameObject newTile = Instantiate(tilePrefab, mapContentArea.transform);
                if (board[id] == DungeonGenerator.Cell.None)
                    newTile.GetComponent<Image>().color = new(0, 0, 0);
                else if (board[id] == DungeonGenerator.Cell.Room)
                    newTile.GetComponent<Image>().color = new(0, 255, 0);
                else // Cell.HallWay
                    newTile.GetComponent<Image>().color = new(0, 0, 255);
                tileMap[(int)id.x, (int)id.y] = newTile;

                newTile.transform.localPosition = new(id.x * tilePrefab.GetComponent<RectTransform>().sizeDelta.x, id.y * tilePrefab.GetComponent<RectTransform>().sizeDelta.y, 0);
            }
        }
    }

    public void UpdateMap()
    {

    }

    public void ToggleMap(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 0f;
            dungeonMapCanvas.SetActive(true);
            mapOpened = true;
        }
        else
        {
            Time.timeScale = 1f;
            dungeonMapCanvas.SetActive(false);
            mapOpened = false;
        }
    }
}
