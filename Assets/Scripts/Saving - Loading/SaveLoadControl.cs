using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadControl : MonoBehaviour {

    // Singleton
    public static SaveLoadControl Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    SerializableData serializableData = new SerializableData();
    public SerializableData loadedData = new SerializableData();

    public SavablePlayerData updatedSavablePlayer1Data = new SavablePlayerData();
    public SavablePlayerData updatedSavablePlayer2Data = new SavablePlayerData();
    public SavablePlayerProgressData updatedSavablePlayerProgressData = new SavablePlayerProgressData();
   

    public bool isSceneBeingLoaded;
    public bool isPlayer1Loaded;
    public bool isPlayer2Loaded;
    public bool isPlayerProgressDataLoaded;

    void LoadingLevel()
    {
        if (isSceneBeingLoaded)
        {
            if (isPlayer1Loaded && isPlayer2Loaded && isPlayerProgressDataLoaded)
            {
                isSceneBeingLoaded = false;
                isPlayer1Loaded = false;
                isPlayer2Loaded = false;
                isPlayerProgressDataLoaded = false;
            }
        }
    }

    void UpdateSerializableData()
    {
        serializableData.savedPlayer1Data = updatedSavablePlayer1Data;
        serializableData.savedPlayer2Data = updatedSavablePlayer2Data;
        serializableData.savedPlayerProgressData = updatedSavablePlayerProgressData;
    }

    public delegate void SaveDeligate(object sender, EventArgs args);
    public static event SaveDeligate SaveEvent;

    public void SaveData()
    {
        if (!Directory.Exists("savegames"))
        {
            Directory.CreateDirectory("savegames");
        }

        UpdateSerializableData();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("savegames/save.mainframe");

        formatter.Serialize(saveFile, serializableData);
        saveFile.Close();
    }

    public void LoadData()
    {
        // Deserialize the binary to readable data
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("savegames/save.mainframe", FileMode.Open);

        loadedData = (SerializableData)formatter.Deserialize(saveFile);
        saveFile.Close();

        // Load the scene
        isSceneBeingLoaded = true;
        int levelToLoad = loadedData.savedPlayerProgressData.savedSceneID;
        Application.LoadLevel(levelToLoad);
    }

    private void Update()
    {
        LoadingLevel();


        // Debug save/load
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("save");
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("load");
            LoadData();
        }
    }

}
