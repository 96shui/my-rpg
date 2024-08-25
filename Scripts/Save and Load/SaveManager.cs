using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]

    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        dataHandler.Delete();
    }


    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)//循环调用所有的找到脚本的LoadData和SaveData到，这样便可以将所有的数据汇聚到gameData中，并从中拿到data
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("Loaded currency " + gameData.currency);
    }

    public void SaveGame()//循环调用所有的找到脚本的LoadData和SaveData到，这样便可以将所有的数据汇聚到gameData中，并从中拿到data
    {

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<ISaveManager> FindAllSaveManagers()//全局寻找带ISave的脚本的函数
    {
        IEnumerable<ISaveManager> saveManager = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManager);
    }

    public bool HasSaveData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}