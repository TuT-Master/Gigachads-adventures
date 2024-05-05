using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject homeScene;
    [SerializeField]
    private GameObject dungeonScene;

    private List<GameObject> sceneList;


    private void Start()
    {
        sceneList = new(){
            homeScene,
            dungeonScene
        };

        LoadScene(homeScene.name);
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
            FindAnyObjectByType<PlayerMovement>().transform.position = newScene.transform.position;
    }

    public bool DungeonLoaded()
    {
        if(dungeonScene.activeInHierarchy)
            return true;
        return false;
    }
}
