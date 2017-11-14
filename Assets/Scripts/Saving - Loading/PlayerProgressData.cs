using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This class is used to contain all the data about the progress. That means the current scene number for now.
 *  
 * 
 */ 


public class PlayerProgressData : MonoBehaviour {

    public static PlayerProgressData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    private int savedLevel;

    private void Start()
    {
        LoadData();

        savedLevel = Application.loadedLevel;
        SaveLoadControl.Instance.updatedSavablePlayerProgressData.savedSceneID = savedLevel;
    }

    private void LoadData()
    {
        if (SaveLoadControl.Instance.isSceneBeingLoaded)
        {
            savedLevel = SaveLoadControl.Instance.loadedData.savedPlayerProgressData.savedSceneID;

            SaveLoadControl.Instance.isPlayerProgressDataLoaded = true;
        }
    }

}
