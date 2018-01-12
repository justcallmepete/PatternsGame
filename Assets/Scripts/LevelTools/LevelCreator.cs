﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreator : MonoBehaviour {

    private LevelCreatorInfo levelCreatorInfo;

    [HideInInspector]
    public GameObject levelBaseMain;
    private LevelBase levelBasePrefab;

    [SerializeField]
    private List<LevelBase> levelBases;
    public List<Room> rooms;

    public float testValue;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
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
        LevelBase levelBaseStart = Instantiate(levelBasePrefab);
        levelBaseStart.transform.localScale = new Vector3(levelCreatorInfo.baseWidth, levelCreatorInfo.wallHeight, levelCreatorInfo.baseLenght);

        levelBaseStart.transform.parent = levelBaseMain.transform;
        levelBases = new List<LevelBase>();
        levelBases.Add(levelBaseStart);
    }

    public void UpdateMeshes(Room pUpdatedRoom)
    {
        //Chech for all boxcolliders if they intersect whith updatedRoom
        Collider roomCol = pUpdatedRoom.GetComponent<Collider>();
        Debug.Log(roomCol);
        for (int i = 0; i < levelBases.Count; i++)
        {
            if (!levelBases[i]) continue;
            Collider levelBaseCol = levelBases[i].GetComponent<Collider>();
            if (roomCol.bounds.Intersects(levelBaseCol.bounds)) //Remove levelbase
            {
                Debug.Log("Removing component");
                DestroyImmediate(levelBases[i].gameObject);                
            }
        }
        CreateNewMeshes(pUpdatedRoom);
        // For all intersecting colliders: 
            //add new mesh and box collider
            //
    }

    void CreateNewMeshes(Room pUpdatedRoom)
    {
        //Create new mesh
        //Get size to the top
        
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(0, 0, 1)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, 1)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, 0)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, -1)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(0, 0, -1)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, -1)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, 0)));
        levelBases.Add(CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, 1)));
        //Check if mesh intersects with other levelbases OR rooms
        //IF mesh intersect, build new mesh that touches last intersected mesh
        //Go to next one
    }

    LevelBase CreateNewSubMesh(Room pUpdatedRoom, Vector3 pDirection)
    {
        LevelBase subMesh = Instantiate(levelBasePrefab);

        //Get new scale for mesh
        float newScaleX = 0;
        float newScaleZ = 0;
        float scaleToEndX = levelCreatorInfo.baseWidth / 2 - (pUpdatedRoom.transform.position.x + pUpdatedRoom.transform.localScale.x / 2 * pDirection.x) * pDirection.x;
        float scaleToEndZ = levelCreatorInfo.baseLenght / 2 - (pUpdatedRoom.transform.position.z + pUpdatedRoom.transform.localScale.z / 2 * pDirection.z) * pDirection.z;
        newScaleX = ((pDirection.z == 0 ^ pDirection.x == 0) && pDirection.z != 0 ? pUpdatedRoom.transform.localScale.x : scaleToEndX);
        newScaleZ = ((pDirection.z == 0 ^ pDirection.x == 0) && pDirection.x != 0 ? pUpdatedRoom.transform.localScale.z : scaleToEndZ);
        subMesh.transform.localScale = new Vector3(newScaleX, levelCreatorInfo.wallHeight, newScaleZ);

        //Set position
        subMesh.transform.position = pUpdatedRoom.transform.position;
        subMesh.transform.position += new Vector3(  (subMesh.transform.localScale.x / 2 + pUpdatedRoom.transform.localScale.x / 2) * pDirection.x,
                                                    0,
                                                    (subMesh.transform.localScale.z / 2 + pUpdatedRoom.transform.localScale.z / 2) * pDirection.z);
        subMesh.transform.parent = levelBaseMain.transform;
        return subMesh;
    }
}
