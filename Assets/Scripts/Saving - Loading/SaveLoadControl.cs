using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/* This class is placed on a empty game object in the scene. Every scene needs this controler. 
 *  
 * This class controls all the saving and loading processes.
 * For example: I want to save the current inventory status from the player. In the player class you call then:
 * SaveLoadControl.Instance.updateSavablePlayer1Data.inventory = inventory;
 * 
 */

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
    public SerializableData loadedCheckpoint = new SerializableData();

    public SavablePlayerData updatedSavablePlayer1Data = new SavablePlayerData();
    public SavablePlayerData updatedSavablePlayer2Data = new SavablePlayerData();
    public SavablePlayerProgressData updatedSavablePlayerProgressData = new SavablePlayerProgressData();
   

    public bool isSceneBeingLoaded;
    public bool isPlayer1Loaded;
    public bool isPlayer2Loaded;
    public bool isPlayerProgressDataLoaded;

    public bool isLoadingCheckpoint;

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

        if (isLoadingCheckpoint)
        {
            if(isPlayer1Loaded && isPlayer2Loaded)
            {
                isLoadingCheckpoint = false;
                isPlayer1Loaded = false;
                isPlayer2Loaded = false;
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

    public void SaveData(bool isCheckpoint)
    {
        if (!Directory.Exists("savegames"))
        {
            Directory.CreateDirectory("savegames");
        }

        UpdateSerializableData();

        BinaryFormatter formatter = new BinaryFormatter();
        if (isCheckpoint)
        {
            Debug.Log("save checkpoint");
            FileStream saveFile = File.Create("savegames/checkpoint.mainframe");
            formatter.Serialize(saveFile, serializableData);
            saveFile.Close();
        }

        if (!isCheckpoint)
        {
            Debug.Log("save game");
            FileStream saveFile = File.Create("savegames/save.mainframe");
            formatter.Serialize(saveFile, serializableData);
            saveFile.Close();
        }
    }

    public void LoadData(bool isCheckpoint)
    {
        if (isCheckpoint)
        {

            Debug.Log("load checkpoint");
            // Deserialize the binary to readable data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open("savegames/checkpoint.mainframe", FileMode.Open);

            loadedCheckpoint = (SerializableData)formatter.Deserialize(saveFile);

            saveFile.Close();

            isLoadingCheckpoint = true;

        }

        if (!isCheckpoint)
        {            
            Debug.Log("load game");
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
    }

    private void Update()
    {
        LoadingLevel();


        // Debug save/load
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData(false);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData(false);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData(true);
        }
    }

}
