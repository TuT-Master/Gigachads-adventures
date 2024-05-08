using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject homeScene;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Transform entranceFromDungeon;

    [SerializeField]
    private GameObject dungeonScene;

    private List<GameObject> sceneList;


    private void Start()
    {
        sceneList = new(){
            homeScene,
            dungeonScene
        };
    }

    public void LoadScene(string sceneName)
    {
        GameObject newScene = null;
        foreach (GameObject go in sceneList)
        {
            if(go.name == sceneName)
                newScene = go;
            else
                go.SetActive(false);
        }

        if(newScene != null)
            newScene.SetActive(true);

        if(newScene.name == "Home")
        {
            if (FindAnyObjectByType<PlayerStats>().playerStats["hp"] <= 0)
                FindAnyObjectByType<PlayerMovement>().transform.position = spawnPoint.position;
            else
                FindAnyObjectByType<PlayerMovement>().transform.position = entranceFromDungeon.position;
        }
    }

    public bool DungeonLoaded()
    {
        if(dungeonScene.activeInHierarchy)
            return true;
        return false;
    }
}
