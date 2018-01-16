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
    private List<LevelBase> newLevelBases;
    public List<Room> rooms;

    [HideInInspector]
    public Room roomBeingBuild;
    [HideInInspector]
    public LevelBase wallBeingMade;
    [HideInInspector]
    public bool roomIsRotated, wallIsRotated; 
    public float testValue;
    // Use this for initialization
    private void OnEnable()
    {
        roomIsRotated = false;
        wallIsRotated = false;
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
        LevelBase levelBaseStart = Instantiate(levelBasePrefab);
        levelBaseStart.transform.localScale = new Vector3(levelCreatorInfo.baseWidth, levelCreatorInfo.wallHeight, levelCreatorInfo.baseLenght);

        levelBaseStart.transform.parent = levelBaseMain.transform;
        levelBases = new List<LevelBase>();
        rooms = new List<Room>();
        roomBeingBuild = null;
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
        DestroyImmediate(roomBeingBuild.gameObject);
        SelectInInspector();
    }

    /// <summary>
    /// Update meshes will be called when the room is confirmed for palcing.
    /// It will delete all intersecting levelbases and start creating new levelbases around the room.
    /// </summary>
    /// <param name="pUpdatedRoom"></param>
    public void UpdateMeshes(Room pUpdatedRoom)
    {
        newLevelBases = new List<LevelBase>();
        Collider roomCol = pUpdatedRoom.GetComponent<Collider>();
        //pUpdatedRoom.intersectingRooms = new List<Room>();
        for (int i = 0; i < levelBases.Count; i++)
        {
            if (!levelBases[i])
            {
                Debug.Log("Empty levelBases");
                continue;
            }
            BoxCollider levelBaseCol = levelBases[i].GetComponent<BoxCollider>();

            //Debug.DrawLine(pUpdatedRoom.transform.position, levelBases[i].transform.position, Color.blue, 10f);
            if (roomCol.bounds.Intersects(levelBaseCol.bounds)) //Remove levelbase
            {
                //TO DO: 
                //Debug.DrawLine(pUpdatedRoom.transform.position, levelBases[i].transform.position, Color.red, 10f);
                SpliceUpMesh(pUpdatedRoom, levelBases[i]);
                DestroyImmediate(levelBases[i].gameObject);
                levelBases.RemoveAt(i);
                i -= 1;
            }
        }

        for (int i = 0; i < newLevelBases.Count; i++)
        {
            levelBases.Add(newLevelBases[i]);
        }
    }

    void SpliceUpMesh(Room pUpdatedRoom, LevelBase pSpliceMesh)
    {
        LevelCreatorUtils.WallsBounds levelBaseWallBounds = LevelCreatorUtils.BoxColliderToWallbounds(pSpliceMesh.transform.position, pSpliceMesh.GetComponent<BoxCollider>());
        //Check if room corners are intersecting
        int cornersInLevelbase = (pUpdatedRoom.wallBounds.TotalCornersInBound(levelBaseWallBounds));
        if (cornersInLevelbase > 0)
        {
            if (cornersInLevelbase > 3) // Splice in 4
            {
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, 1));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(-1, 0, 0));
            }
            else
            {
                //Check wich corner is intersecting
                if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopLeft(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, 1));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(-1, 0, 0));
                    if (cornersInLevelbase == 2)
                    {
                        if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                        }
                        else //Bottom left
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                        }
                    }
                }
                else if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerBottomRight(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                    if (cornersInLevelbase == 2)
                    {
                        if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, 1));
                        }
                        else //Bottom left
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(-1, 0, 0));
                        }
                    }
                }
                else if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, 1));
                }
                else //Bottom left solo
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(-1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                }
            }
        }
        else // Check wich 
        {
            int cornersInRoom = levelBaseWallBounds.TotalCornersInBound(pUpdatedRoom.wallBounds);
            if (cornersInRoom > 3)// All corners are in room, delete mesh
            {
                Debug.Log("more than 3 corners in base. not making a new one");
                return;
            }
            else // Move mesh to side
            {
                if(pUpdatedRoom.wallBounds.minWallsX > levelBaseWallBounds.minWallsX) //Left
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(-1, 0, 0));
                    if (pUpdatedRoom.wallBounds.maxWallsX < levelBaseWallBounds.maxWallsX)
                    {
                        CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                    }
                }
                else if(pUpdatedRoom.wallBounds.maxWallsZ < levelBaseWallBounds.maxWallsZ) //Top
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, 1));
                    if (pUpdatedRoom.wallBounds.minWallsZ > levelBaseWallBounds.minWallsZ)
                    {
                        CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                    }
                }
                else if (pUpdatedRoom.wallBounds.maxWallsX < levelBaseWallBounds.maxWallsX) //Right
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(1, 0, 0));
                }
                else //Bottom
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom, new Vector3(0, 0, -1));
                }
            }
        }
    }

    void CreateNewSubMesh(LevelBase originalLevelBase, Room pUpdatedRoom, Vector3 pDirection)
    {
        LevelBase subMesh = Instantiate(levelBasePrefab);
        subMesh.transform.position = originalLevelBase.transform.position;

        float zOffset = 0;
        LevelCreatorUtils.WallsBounds originalBounds = LevelCreatorUtils.BoxColliderToWallbounds(originalLevelBase.transform.position, originalLevelBase.GetComponent<BoxCollider>());
        float xScale = originalLevelBase.transform.localScale.x;
        float zScale = originalLevelBase.transform.localScale.z;
        if (pDirection.x != 0)
        {
            xScale = pDirection.x == 1 ? originalBounds.maxWallsX - pUpdatedRoom.wallBounds.maxWallsX : originalBounds.minWallsX - pUpdatedRoom.wallBounds.minWallsX;
            float topReduction = 0;
            float bottomReduction = 0;
            //Delete top scale
            if (pUpdatedRoom.wallBounds.maxWallsZ < originalBounds.maxWallsZ)
            {
                topReduction = originalBounds.maxWallsZ - pUpdatedRoom.wallBounds.maxWallsZ;
            }
            //Delete bottom 
            if (pUpdatedRoom.wallBounds.minWallsZ > originalBounds.minWallsZ)
            {
                bottomReduction = pUpdatedRoom.wallBounds.minWallsZ - originalBounds.minWallsZ;
            }
            zScale -= (topReduction + bottomReduction);
            //set to correct z pos
            if (originalBounds.maxWallsZ > pUpdatedRoom.wallBounds.maxWallsZ) zOffset -= (originalBounds.maxWallsZ - pUpdatedRoom.wallBounds.maxWallsZ) / 2;
            if (pUpdatedRoom.wallBounds.minWallsZ > originalBounds.minWallsZ) zOffset += (pUpdatedRoom.wallBounds.minWallsZ - originalBounds.minWallsZ) / 2;

            //zOffset = (((originalBounds.maxWallsZ - pUpdatedRoom.wallBounds.maxWallsZ) / 2) - ((pUpdatedRoom.wallBounds.minWallsZ - originalBounds.minWallsZ) / 2)) * -1;
            subMesh.transform.position = new Vector3(pUpdatedRoom.transform.position.x, 0, originalLevelBase.transform.position.z + zOffset);
        }
        else
        {
            zScale = pDirection.z == 1 ? originalBounds.maxWallsZ - pUpdatedRoom.wallBounds.maxWallsZ : originalBounds.minWallsZ - pUpdatedRoom.wallBounds.minWallsZ;
            subMesh.transform.position = new Vector3(originalLevelBase.transform.position.x, 0, pUpdatedRoom.transform.position.z);
        }
        subMesh.transform.localScale = new Vector3(Mathf.Abs(xScale), levelCreatorInfo.wallHeight, Mathf.Abs(zScale));

        subMesh.transform.position += new Vector3(((subMesh.transform.localScale.x / 2 + pUpdatedRoom.transform.localScale.x / 2) * pDirection.x),
                                                   0,
                                                  ((subMesh.transform.localScale.z / 2 + pUpdatedRoom.transform.localScale.z / 2) * pDirection.z));
        newLevelBases.Add(subMesh);
        subMesh.transform.parent = levelBaseMain.transform;
    }

    public void SpawnWall()
    {
        //Instantiate wall
        wallBeingMade = Instantiate(levelBasePrefab);
        wallBeingMade.transform.localScale = new Vector3(2, levelCreatorInfo.wallHeight, 2);
        ActiveEditorTracker.sharedTracker.isLocked = true;
        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = wallBeingMade.gameObject;
        Selection.objects = selectObject;
        SetWallRatio(1, 1);
        //Check if intersecting with walls

        //Cut of part that is intersecting with wall
    }

    public void ConfirmWallPlacement()
    {
        wallBeingMade.ConfirmPlacement();
        levelBases.Add(wallBeingMade);
        wallBeingMade.transform.parent = levelBaseMain.transform;
    }

    public void DenyWallPlacement()
    {
        DestroyImmediate(wallBeingMade.gameObject);
        SelectInInspector();
    }

    public void SetWallRatio(int pNumerator, int pDenominator)
    {

        wallBeingMade.numerator = pNumerator;
        wallBeingMade.denominator = pDenominator;

        roomBeingBuild.UpdateSize(wallIsRotated);
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
}
