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
        gameData = dataHandler.Load();
        if (gameData == null)
            NewGame();
        else
            StartCoroutine(LoadGame());
    }

    public void NewGame()
    {
        gameData = new GameData();

        // Starter pack
        GameObject playerInventory = FindAnyObjectByType<PlayerInventory>().backpackInventory;
        gameData.playerInventory.Add(playerInventory.transform.GetChild(0).GetComponent<Slot>().id, "Primitive club-1");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(1).GetComponent<Slot>().id, "Primitive bow-1");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(2).GetComponent<Slot>().id, "Primitive arrow-20");
        gameData.playerInventory.Add(playerInventory.transform.GetChild(3).GetComponent<Slot>().id, "Primitive magic staff-1");
        string rndCrystalName = new System.Random().Next(4) switch
        {
            0 => "Fire crystal-1",
            1 => "Water crystal-1",
            2 => "Wind crystal-1",
            3 => "Earth crystal-1",
        };
        gameData.playerInventory.Add(playerInventory.transform.GetChild(4).GetComponent<Slot>().id, rndCrystalName);

        FindAnyObjectByType<StartScreen>().CreateNewCharacter();
    }

    public IEnumerator LoadGame()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < FindAnyObjectByType<StartScreen>(FindObjectsInactive.Exclude).transform.childCount; i++)
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
