using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    private GameObject player;

    private DungeonGenerator dungeonGenerator;

    private AsyncOperation operation;




    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(FindAnyObjectByType<DungeonGenerator>() != null)
            dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
    }

    public void LoadScene(string sceneName)
    {
        if (operation != null && operation.progress > 0)
            return;
        // Load Scene
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Load new Scene
        operation = SceneManager.LoadSceneAsync(sceneName);

        // Activate loading screen GameObject.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log(progressValue);

            yield return null;
        }
    }
}
