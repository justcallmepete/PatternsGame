﻿using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public partial class LevelCreator : MonoBehaviour {

    private LevelCreatorInfo levelCreatorInfo;
    
    public GameObject levelBaseMain;
    private LevelBase levelBasePrefab;
    private Room basicRoomPrefab;
    private SlidingDoor doorPrefab;
    
    public List<LevelBase> levelBases;
    public List<LevelBase> newLevelBases;
    public List<Room> rooms;
    public List<SlidingDoor> doors;
    
    public Room roomBeingBuild;
    public LevelBase wallBeingMade;
    public SlidingDoor doorBeingMade;

    public bool roomIsRotated, wallIsRotated, doorIsRotated;
    public Vector3 doorDirection;

    // Use this for initialization
#if UNITY_EDITOR
    private void OnEnable()
    {
        roomIsRotated = false;
        wallIsRotated = false;
        if (!levelBaseMain)
        {
            levelBaseMain = GameObject.Find("levelBaseMain");
            if (levelBaseMain)
            {
                GetLevelCreatorInfo();
            }
        }
        basicRoomPrefab = levelCreatorInfo.basicRoom;
        levelBasePrefab = levelCreatorInfo.levelBase;
        levelBases = GetAllWalls();

    }

    public void SelectInInspector()
    {
        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = this.gameObject;
        Selection.objects = selectObject;
    }

    /// <summary>
    /// Creates the base for the level where rooms can be added in
    /// </summary>
    public void SpawnBaseLevel()
    {
        levelCreatorInfo = AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/LevelCreatorInfo.prefab", typeof(LevelCreatorInfo)) as LevelCreatorInfo;
        levelBaseMain = new GameObject();
        levelBaseMain.name = "levelBaseMain";
        levelBasePrefab = levelCreatorInfo.levelBase;
        basicRoomPrefab = levelCreatorInfo.basicRoom;
        levelBasePrefab = levelCreatorInfo.levelBase;
        levelBases = GetAllWalls();
        LevelBase levelBaseStart = Instantiate(levelBasePrefab);
        levelBaseStart.transform.localScale = new Vector3(levelCreatorInfo.baseWidth, levelCreatorInfo.wallHeight, levelCreatorInfo.baseLenght);

        levelBaseStart.transform.parent = levelBaseMain.transform;
        levelBases = new List<LevelBase>();
        rooms = new List<Room>();
        roomBeingBuild = null;
        levelBases.Add(levelBaseStart);
    }
    
    //Wall methods region
    #region
    public void SpawnWall()
    {
        //Instantiate wall
        levelCreatorInfo = AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/LevelCreatorInfo.prefab", typeof(LevelCreatorInfo)) as LevelCreatorInfo;
        levelBasePrefab = levelCreatorInfo.levelBase;
        wallBeingMade = Instantiate(levelBasePrefab);
        wallBeingMade.transform.localScale = new Vector3(2, levelCreatorInfo.wallHeight, 2);
        ActiveEditorTracker.sharedTracker.isLocked = true;
        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = wallBeingMade.gameObject;
        Selection.objects = selectObject;

        wallBeingMade.denominator = 1;
        wallBeingMade.numerator = 1;
        //Check if intersecting with walls

        //Cut of part that is intersecting with wall
    }

    public void ConfirmWallPlacement()
    {
        wallBeingMade.ConfirmPlacement();
        levelBases.Add(wallBeingMade);
        wallBeingMade.transform.parent = levelBaseMain.transform;
        wallBeingMade = null;
    }

    public void DenyWallPlacement()
    {
        DestroyImmediate(wallBeingMade.gameObject);
        SelectInInspector();
        wallBeingMade = null;
    }

    public void SetWallRatio(int pNumerator, int pDenominator)
    {

        wallBeingMade.numerator = pNumerator;
        wallBeingMade.denominator = pDenominator;

        wallBeingMade.UpdateSize(wallIsRotated);
    }

    public float setWallScale
    {
        get
        {
            return wallBeingMade.editorSize;
        }
        set
        {
            wallBeingMade.editorSize = value;
            wallBeingMade.UpdateSize(wallIsRotated);
        }
    }

    public void ToggleRotateWall()
    {
        wallIsRotated = wallIsRotated ? false : true;
    }
    #endregion

    void AddNewWalls()
    {
        //TO DO: Have a method that handles all new walls

        //TO DO: Implement undo function with walls
    }

    void RemoveObject()
    {
        //TO DO: Add to undo queue as removed object
    }

    List<LevelBase> GetAllWalls()
    {
        Debug.Log("Getting new walls");
        LevelBase[] levelBases = GameObject.Find("levelBaseMain").GetComponentsInChildren<LevelBase>();
        List<LevelBase> currentLevelBases = new List<LevelBase>();

        for (int i = 0; i < levelBases.Length; i++)
        {
            currentLevelBases.Add(levelBases[i]);
        }
        return currentLevelBases;
    }

    void GetLevelCreatorInfo()
    {
        levelCreatorInfo = AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/LevelCreatorInfo.prefab", typeof(LevelCreatorInfo)) as LevelCreatorInfo;
        Debug.Log(levelCreatorInfo);
        levelBasePrefab = levelCreatorInfo.levelBase;
        basicRoomPrefab = levelCreatorInfo.basicRoom;
        doorPrefab = levelCreatorInfo.doorPrefab;
    }
#endif
}