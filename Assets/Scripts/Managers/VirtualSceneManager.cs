using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSceneManager : MonoBehaviour
{
    public enum CurrentScene
    {
        Home,
        Dungeon,
        Shop,
        TutorialDungeon,
    }
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Transform entranceFromDungeon;
    [SerializeField]
    private Transform entranceFromShop;

    // 0-Home, 1-Dungeon, 2-Shop
    [SerializeField]
    private List<GameObject> sceneList;
    [SerializeField]
    private List<GameObject> tutorialDungeons;



    public void LoadScene(string sceneName, CurrentScene currentScene)
    {
        // Set active scene and deactivate others
        GameObject newScene = null;
        foreach (GameObject go in sceneList)
        {
            if(go.name == sceneName)
                newScene = go;
            else
                go.SetActive(false);
        }

        if (newScene != null)
        {
            newScene.SetActive(true);
            if (newScene.name == "Home")
            {
                if (FindAnyObjectByType<PlayerStats>().playerStats["hp"] <= 0)
                    FindAnyObjectByType<PlayerMovement>().transform.position = spawnPoint.position;
                else if (currentScene == CurrentScene.Dungeon || currentScene == CurrentScene.TutorialDungeon)
                    FindAnyObjectByType<PlayerMovement>().transform.position = entranceFromDungeon.position;
                else if (currentScene == CurrentScene.Shop)
                    FindAnyObjectByType<PlayerMovement>().transform.position = entranceFromShop.position;
            }
            else if (newScene.name == "Dungeon")
                sceneList[1].transform.Find("Room-0").GetComponent<DungeonRoom>().StartRoom();
            else if (newScene.name == "Shop")
                FindAnyObjectByType<PlayerMovement>().transform.position = sceneList[2].transform.Find("SpawnPoint").transform.position;
        }
        else
            Debug.LogError("Parameter *sceneName* not set or wrong!");
    }

    public bool DungeonLoaded()
    {
        if(sceneList[1].activeInHierarchy)
            return true;
        return false;
    }
}
