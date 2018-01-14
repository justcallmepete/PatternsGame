using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreator : MonoBehaviour {

    private LevelCreatorInfo levelCreatorInfo;

    [HideInInspector]
    public GameObject levelBaseMain;
    private LevelBase levelBasePrefab;
    private Room basicRoomPrefab;

    [SerializeField]
    private List<LevelBase> levelBases;
    public List<Room> rooms;

    [HideInInspector]
    public Room roomBeingBuild;
    [HideInInspector]
    public bool roomIsRotated; 
    public float testValue;
    // Use this for initialization
    private void OnEnable()
    {
        roomIsRotated = false;
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
        rooms = new List<Room>();
        levelBases.Add(levelBaseStart);
    }

    public void SpawnRoom()
    {
        basicRoomPrefab = levelCreatorInfo.basicRoom;
        roomBeingBuild = Instantiate(basicRoomPrefab);
        roomBeingBuild.transform.localScale = new Vector3(10, levelCreatorInfo.wallHeight, 10);

        //Lock inspector to level creator
        ActiveEditorTracker.sharedTracker.isLocked = true;
        //Select the room object
        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = roomBeingBuild.gameObject;
        Selection.objects = selectObject;
    }


    public void SetRoomRatio(int pNumerator, int pDenominator)
    {

        roomBeingBuild.numerator = pNumerator;
        roomBeingBuild.denominator = pDenominator;

        roomBeingBuild.UpdateSize(roomIsRotated);
    }
    public float setRoomScale
    { 
        get
        {
            return roomBeingBuild.editorSize;
        }
        set
        {
            roomBeingBuild.editorSize = value;
            roomBeingBuild.UpdateSize(roomIsRotated);
        }
    }

    public void ToggleRotateRoom()
    {
        roomIsRotated = roomIsRotated ? false : true;
    }
    public void ConfirmRoomPlacement()
    {
        roomBeingBuild.ConfirmPLacement();
        UpdateMeshes(roomBeingBuild);
        rooms.Add(roomBeingBuild);
    }

    public void DenyRoomPlacement()
    {
        DestroyImmediate(roomBeingBuild);
    }

    public void UpdateMeshes(Room pUpdatedRoom)
    {
        //Chech for all boxcolliders if they intersect whith updatedRoom
        Collider roomCol = pUpdatedRoom.GetComponent<Collider>();
        
        for (int i = 0; i < levelBases.Count; i++)
        {
            if (!levelBases[i]) continue;
            BoxCollider levelBaseCol = levelBases[i].GetComponent<BoxCollider>();
            if (roomCol.bounds.Intersects(levelBaseCol.bounds)) //Remove levelbase
            {
                //Check if wallbounds can be extended
                LevelCreatorUtils.WallsBounds levelBaseWallBounds = LevelCreatorUtils.BoxColliderToWallbounds(levelBases[i].transform.position, levelBaseCol);
                pUpdatedRoom.wallBounds = LevelCreatorUtils.ExtendWallBounds(roomBeingBuild.wallBounds, levelBaseWallBounds);
                DestroyImmediate(levelBases[i].gameObject);
                levelBases.RemoveAt(i);
                i -= 1;
            }
        }
        CreateNewMeshes(pUpdatedRoom);
    }

    void CreateNewMeshes(Room pUpdatedRoom)
    {
        //Create new mesh
        //Get size to the top

        CreateNewSubMesh(pUpdatedRoom, new Vector3(0, 0, 1));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, 1));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, 0));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(1, 0, -1));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(0, 0, -1));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, -1));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, 0));
        CreateNewSubMesh(pUpdatedRoom, new Vector3(-1, 0, 1));
        //Check if mesh intersects with other levelbases OR rooms
        //IF mesh intersect, build new mesh that touches last intersected mesh
        //Go to next one
    }

    void CreateNewSubMesh(Room pUpdatedRoom, Vector3 pDirection)
    {
        LevelBase subMesh = Instantiate(levelBasePrefab);
        float newScaleX = (pDirection.x == 1 ? pUpdatedRoom.wallBounds.maxWallsX : pUpdatedRoom.wallBounds.minWallsX) - (pUpdatedRoom.transform.position.x + pUpdatedRoom.transform.localScale.x / 2 * pDirection.x);
        float newScaleZ = (pDirection.z == 1 ? pUpdatedRoom.wallBounds.maxWallsZ : pUpdatedRoom.wallBounds.minWallsZ) - (pUpdatedRoom.transform.position.z + pUpdatedRoom.transform.localScale.z / 2 * pDirection.z);


        float finalScaleX = ((pDirection.z == 0 ^ pDirection.x == 0) && pDirection.z != 0 ? pUpdatedRoom.transform.localScale.x : newScaleX);
        float finalScaleY = ((pDirection.z == 0 ^ pDirection.x == 0) && pDirection.x != 0 ? pUpdatedRoom.transform.localScale.z : newScaleZ);
        subMesh.transform.localScale = new Vector3(Mathf.Abs(finalScaleX), levelCreatorInfo.wallHeight, Mathf.Abs(finalScaleY));
        SetSubMeshPosition(subMesh, pUpdatedRoom, pDirection);
        levelBases.Add(subMesh);
    }

    void SetSubMeshPosition(LevelBase pSubmesh, Room pUpdatedRoom, Vector3 pDirection)
    {
        pSubmesh.transform.position = pUpdatedRoom.transform.position;
        pSubmesh.transform.position += new Vector3(((pSubmesh.transform.localScale.x / 2 + pUpdatedRoom.transform.localScale.x / 2) * pDirection.x),
                                                    0,
                                                   ((pSubmesh.transform.localScale.z / 2 + pUpdatedRoom.transform.localScale.z / 2) * pDirection.z));
        pSubmesh.transform.parent = levelBaseMain.transform;
    }
}
