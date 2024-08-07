using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string playerDataFileName;

    [SerializeField] private GameObject saveMessage;



    public static DataPersistanceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;


    public DataPersistanceManager()
    {
        if (instance != null)
            Debug.Log("There is error with DataPersistanceManager (more than one in the scene)");
        instance = this;
    }


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        dataHandler = new FileDataHandler(Application.persistentDataPath, playerDataFileName);
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        StartCoroutine(LoadGame());
    }

    public void NewGame()
    {
        gameData = new GameData();

        // Starter pack
        GameObject playerInventory = FindAnyObjectByType<PlayerInventory>().backpackInventory;
        gameData.playerInventory.Add(playerInventory.transform.GetChild(0), "Primitive club-1");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(1), "Primitive bow-1");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(2), "Primitive arrow-20");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(3), "Primitive magic staff-1");
        string rndCrystalName = "";
        switch (new System.Random().Next(4))
        {
            case 0:
                rndCrystalName = "Fire crystal-1";
                break;
            case 1:
                rndCrystalName = "Water crystal-1";
                break;
            case 2:
                rndCrystalName = "Wind crystal-1";
                break;
            case 3:
                rndCrystalName = "Earth crystal-1";
                break;
        }
        gameData.playerInventory.Add(playerInventory.transform.GetChild(4), rndCrystalName);

        FindAnyObjectByType<StartScreen>().CreateNewCharacter();

        dataHandler.SaveData(gameData);
    }

    public IEnumerator LoadGame()
    {
        yield return new WaitForEndOfFrame();
        gameData = dataHandler.Load();

        if (gameData == null)
            NewGame();
        else
            for(int i = 0; i < FindAnyObjectByType<StartScreen>(FindObjectsInactive.Exclude).transform.childCount; i++)
                FindAnyObjectByType<StartScreen>(FindObjectsInactive.Exclude).transform.GetChild(i).gameObject.SetActive(false);


        foreach (IDataPersistance obj in dataPersistanceObjects)
            obj.LoadData(gameData);
    }

    public void SaveGame()
    {
        foreach (IDataPersistance obj in dataPersistanceObjects)
            obj.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
        StartCoroutine(ShowSaveMessage());
    }
    private IEnumerator ShowSaveMessage()
    {
        saveMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        saveMessage.SetActive(false);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
