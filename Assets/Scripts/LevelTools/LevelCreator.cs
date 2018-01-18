using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreator : MonoBehaviour {

    private LevelCreatorInfo levelCreatorInfo;

    [HideInInspector]
    public GameObject levelBaseMain;
    private LevelBase levelBasePrefab;
    private Room basicRoomPrefab;

    [SerializeField]
    public List<LevelBase> levelBases;
    public List<LevelBase> newLevelBases;
    public List<Room> rooms;
    public List<SlidingDoor> doors;

    [HideInInspector]
    public Room roomBeingBuild;
    [HideInInspector]
    public LevelBase wallBeingMade;
    public SlidingDoor doorBeingMade;
    public Vector3 doorDirection;
    [HideInInspector]
    public bool roomIsRotated, wallIsRotated, doorIsRotated; 
    public float testValue;
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
                levelCreatorInfo = Resources.Load("Prefabs/LevelCreatorInfo.prefab") as LevelCreatorInfo;                 
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

    //Room methods region
    #region 
    public void SpawnRoom()
    {
        levelCreatorInfo = AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/LevelCreatorInfo.prefab", typeof(LevelCreatorInfo)) as LevelCreatorInfo;
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
        DestroyImmediate(roomBeingBuild.GetComponent<BoxCollider>());
        roomBeingBuild = null;
    }

    public void DenyRoomPlacement()
    {
        DestroyImmediate(roomBeingBuild.gameObject);
        SelectInInspector();
        roomBeingBuild = null;
    }
    #endregion
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

    //Door methods region
    #region
    public void SpawnDoor()
    {
        doorBeingMade = Instantiate(levelCreatorInfo.doorPrefab);
        doorDirection = new Vector3(1, 0, 0);


        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = doorBeingMade.gameObject;
        Selection.objects = selectObject;
    }

    public void ToggleRotateDoor()
    {//Rotate clockwise
        if(doorDirection.x != 0)
        {
            float oldDir = doorDirection.x;
            doorBeingMade.transform.Rotate(0, -90, 0);
            doorDirection = new Vector3(0, 0, 1);
        }
        else
        {
            float oldDir = doorDirection.z;
            doorDirection = new Vector3(1, 0, 0);
            doorBeingMade.transform.Rotate(0, +90, 0);
        }
    }
    public void ConfirmDoorPlacement()
    {
        Vector3 leftWallPos = doorBeingMade.transform.Find("WallSideLeft").position;
        Vector3 rightWallPos = doorBeingMade.transform.Find("WallSideRight").position;
        doors.Add(doorBeingMade);
        Vector3 doorScale = new Vector3(1, 0, 4);
        if (doorDirection.x != 0)
        {
            LevelBase topWall = Instantiate(levelBasePrefab);
            LevelBase bottomWall = Instantiate(levelBasePrefab);
            //Create top mesh to max
            float topScale = Mathf.Abs(levelCreatorInfo.baseLenght / 2 - leftWallPos.z);
            float bottomScale = Mathf.Abs(rightWallPos.z - levelCreatorInfo.baseLenght/2);
            topWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, topScale);
            bottomWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, bottomScale);
            topWall.transform.position = doorBeingMade.transform.position + new Vector3(0,0, topWall.transform.localScale.z/2) + new Vector3(0, 0, (leftWallPos.z - rightWallPos.z) / 2);
            bottomWall.transform.position = doorBeingMade.transform.position - new Vector3(0, 0, bottomWall.transform.localScale.z / 2) + new Vector3(0, 0, (leftWallPos.z - rightWallPos.z) / 2);

            BoxCollider firstBoxcollider = topWall.GetComponent<BoxCollider>();
            BoxCollider secondBoxCollider = bottomWall.GetComponent<BoxCollider>();
            float firstSmallestScale = topWall.transform.localScale.z;
            float secondSmallestScale = bottomWall.transform.localScale.z;
            for (int i = 0; i < levelBases.Count; i++)
            {
                BoxCollider levelBaseBoxCollider = levelBases[i].GetComponent<BoxCollider>();
                LevelCreatorUtils.WallsBounds levelBaseBounds = LevelCreatorUtils.BoxColliderToWallbounds(levelBases[i].transform.position, levelBaseBoxCollider);
                if (firstBoxcollider.bounds.Intersects(levelBaseBoxCollider.bounds)) //Top box
                {
                    if (levelBaseBounds.minWallsZ < leftWallPos.z)
                    {
                        Debug.LogWarning("Corner of door is in a wall");
                        Debug.Log(levelBaseBounds.ToString());
                        continue;
                    }
                    float zDistanceToWall = levelBaseBounds.minWallsZ - leftWallPos.z;
                    if (zDistanceToWall < firstSmallestScale)
                    {
                        firstSmallestScale = zDistanceToWall;
                    }
                }
                if (secondBoxCollider.bounds.Intersects(levelBaseBoxCollider.bounds)) // Bottom box
                {
                    Debug.DrawLine(levelBaseBounds.center, Vector3.zero, Color.red, 10f);
                    Debug.Log(levelBaseBounds.ToString());
                    if (levelBaseBounds.maxWallsZ > rightWallPos.z)
                    {
                        Debug.LogWarning("Corner of door is in a wall");
                        continue;
                    }
                    float zDistanceToWall = rightWallPos.z - levelBaseBounds.maxWallsZ;
                    if (zDistanceToWall < secondSmallestScale)
                    {
                        secondSmallestScale = zDistanceToWall;
                    }
                }
            }

            topWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, firstSmallestScale);
            bottomWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, -secondSmallestScale);
            topWall.transform.position = doorBeingMade.transform.position + new Vector3(0, 0, topWall.transform.localScale.z / 2) + new Vector3(0, 0, (leftWallPos.z - rightWallPos.z) / 2);
            bottomWall.transform.position = doorBeingMade.transform.position + new Vector3(0, 0, bottomWall.transform.localScale.z / 2) - new Vector3(0, 0, (leftWallPos.z - rightWallPos.z) / 2);


            levelBases.Add(topWall);
            levelBases.Add(bottomWall);
        }
        else
        {
            LevelBase leftWall = Instantiate(levelBasePrefab);
            LevelBase rightWall = Instantiate(levelBasePrefab);
            float leftScale = Mathf.Abs(levelCreatorInfo.baseWidth / 2 - leftWallPos.x);
            float rightScale = Mathf.Abs(leftWallPos.x - levelCreatorInfo.baseWidth / 2);
            leftWall.transform.localScale = new Vector3(leftScale, levelCreatorInfo.wallHeight,1);
            rightWall.transform.localScale = new Vector3(rightScale, levelCreatorInfo.wallHeight,1);
            leftWall.transform.position = doorBeingMade.transform.position - new Vector3(leftWall.transform.localScale.x / 2,0,0) - new Vector3((rightWallPos.x-leftWallPos.x) / 2, 0, 0);
            rightWall.transform.position = doorBeingMade.transform.position + new Vector3(rightWall.transform.localScale.x / 2,0,0) + new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);
            
            BoxCollider leftBoxCollider = leftWall.GetComponent<BoxCollider>();
            BoxCollider rightBoxCollider = rightWall.GetComponent<BoxCollider>();
            float leftSmallestScale = leftWall.transform.localScale.x;
            float rightSmallestScale = rightWall.transform.localScale.x;
            for (int i = 0; i < levelBases.Count; i++)
            {
                if (levelBases[i] == null) continue;
                BoxCollider levelBaseBoxCollider = levelBases[i].GetComponent<BoxCollider>();
                LevelCreatorUtils.WallsBounds levelBaseBounds = LevelCreatorUtils.BoxColliderToWallbounds(levelBases[i].transform.position, levelBaseBoxCollider);
                if (leftBoxCollider.bounds.Intersects(levelBaseBoxCollider.bounds)) //Top box
                {
                    Debug.DrawLine(levelBases[i].transform.position, leftWall.transform.position, Color.red, 10f);
                    if (levelBaseBounds.maxWallsX > leftWallPos.x)
                    {
                        Debug.LogWarning("Corner of door is in a wall");
                        Debug.Log(levelBaseBounds.ToString());
                        continue;
                    }
                    float xDistanceToWall = leftWallPos.x - levelBaseBounds.maxWallsX;
                    if (xDistanceToWall < leftSmallestScale)
                    {
                        Debug.DrawLine(doorBeingMade.transform.position, new Vector3(levelBaseBounds.maxWallsX, levelBases[i].transform.position.y, levelBases[i].transform.position.z), Color.yellow, 10f);
                        leftSmallestScale = xDistanceToWall;
                    }
                }
                if (rightBoxCollider.bounds.Intersects(levelBaseBoxCollider.bounds)) // Bottom box
                {
                    if (levelBaseBounds.minWallsX < rightWallPos.x)
                    {
                        Debug.LogWarning("Corner of door is in a wall");
                        continue;
                    }
                    float xDistanceToWall = levelBaseBounds.minWallsX - rightWallPos.x;
                    if (xDistanceToWall < rightSmallestScale)
                    {
                        rightSmallestScale = xDistanceToWall;
                    }
                }                
            }

            leftWall.transform.localScale = new Vector3(leftSmallestScale, levelCreatorInfo.wallHeight, 1);
            rightWall.transform.localScale = new Vector3(   rightSmallestScale, levelCreatorInfo.wallHeight, 1);
            leftWall.transform.position = doorBeingMade.transform.position - new Vector3(leftWall.transform.localScale.x / 2, 0, 0) - new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);
            rightWall.transform.position = doorBeingMade.transform.position + new Vector3(rightWall.transform.localScale.x / 2, 0, 0) + new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);

            leftWall.transform.name = "leftWall";
            rightWall.transform.name = "rightWall";
            levelBases.Add(leftWall);
            levelBases.Add(rightWall);
        }
    }

    public void DenyDoorPlacement()
    {
        DestroyImmediate(doorBeingMade.gameObject);
    }
    #endregion
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
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, 1));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(-1, 0, 0));
            }
            else
            {
                //Check wich corner is intersecting
                if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopLeft(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, 1));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(-1, 0, 0));
                    if (cornersInLevelbase == 2)
                    {
                        if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                        }
                        else //Bottom left
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
                        }
                    }
                }
                else if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerBottomRight(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
                    if (cornersInLevelbase == 2)
                    {
                        if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, 1));
                        }
                        else //Bottom left
                        {
                            CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(-1, 0, 0));
                        }
                    }
                }
                else if (pUpdatedRoom.wallBounds.IsCornerInBound(LevelCreatorUtils.WallsBounds.CornerTopRight(), levelBaseWallBounds))
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, 1));
                }
                else //Bottom left solo
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(-1, 0, 0));
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
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
                if (pUpdatedRoom.wallBounds.minWallsX > levelBaseWallBounds.minWallsX) //Left
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(-1, 0, 0));
                    if (pUpdatedRoom.wallBounds.maxWallsX < levelBaseWallBounds.maxWallsX)
                    {
                        CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                    }
                }
                else if (pUpdatedRoom.wallBounds.maxWallsZ < levelBaseWallBounds.maxWallsZ) //Top
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, 1));
                    if (pUpdatedRoom.wallBounds.minWallsZ > levelBaseWallBounds.minWallsZ)
                    {
                        CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
                    }
                }
                else if (pUpdatedRoom.wallBounds.maxWallsX < levelBaseWallBounds.maxWallsX) //Right
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(1, 0, 0));
                }
                else //Bottom
                {
                    CreateNewSubMesh(pSpliceMesh, pUpdatedRoom.wallBounds, new Vector3(0, 0, -1));
                }
            }
        }
    }

    void CreateNewSubMesh(LevelBase originalLevelBase,LevelCreatorUtils.WallsBounds roomWallbounds, Vector3 pDirection)
    {
        LevelBase subMesh = Instantiate(levelBasePrefab);
        subMesh.transform.position = originalLevelBase.transform.position;

        float zOffset = 0;
        LevelCreatorUtils.WallsBounds originalBounds = LevelCreatorUtils.BoxColliderToWallbounds(originalLevelBase.transform.position, originalLevelBase.GetComponent<BoxCollider>());
        float xScale = originalLevelBase.transform.localScale.x;
        float zScale = originalLevelBase.transform.localScale.z;
        if (pDirection.x != 0)
        {
            xScale = pDirection.x == 1 ? originalBounds.maxWallsX - roomWallbounds.maxWallsX : originalBounds.minWallsX - roomWallbounds.minWallsX;
            float topReduction = 0;
            float bottomReduction = 0;
            //Delete top scale
            if (roomWallbounds.maxWallsZ < originalBounds.maxWallsZ)
            {
                topReduction = originalBounds.maxWallsZ - roomWallbounds.maxWallsZ;
            }
            //Delete bottom 
            if (roomWallbounds.minWallsZ > originalBounds.minWallsZ)
            {
                bottomReduction = roomWallbounds.minWallsZ - originalBounds.minWallsZ;
            }
            zScale -= (topReduction + bottomReduction);
            //set to correct z pos
            if (originalBounds.maxWallsZ > roomWallbounds.maxWallsZ) zOffset -= (originalBounds.maxWallsZ - roomWallbounds.maxWallsZ) / 2;
            if (roomWallbounds.minWallsZ > originalBounds.minWallsZ) zOffset += (roomWallbounds.minWallsZ - originalBounds.minWallsZ) / 2;

            //zOffset = (((originalBounds.maxWallsZ - pUpdatedRoom.wallBounds.maxWallsZ) / 2) - ((pUpdatedRoom.wallBounds.minWallsZ - originalBounds.minWallsZ) / 2)) * -1;
            subMesh.transform.position = new Vector3(roomWallbounds.center.x, 0, originalLevelBase.transform.position.z + zOffset);
        }
        else
        {
            zScale = pDirection.z == 1 ? originalBounds.maxWallsZ - roomWallbounds.maxWallsZ : originalBounds.minWallsZ - roomWallbounds.minWallsZ;
            subMesh.transform.position = new Vector3(originalLevelBase.transform.position.x, 0, roomWallbounds.center.z);
        }
        subMesh.transform.localScale = new Vector3(Mathf.Abs(xScale), levelCreatorInfo.wallHeight, Mathf.Abs(zScale));

        subMesh.transform.position += new Vector3(((subMesh.transform.localScale.x / 2 + (roomWallbounds.maxWallsX - roomWallbounds.minWallsX) / 2) * pDirection.x),
                                                   0,
                                                  ((subMesh.transform.localScale.z / 2 + (roomWallbounds.maxWallsZ - roomWallbounds.minWallsZ) / 2) * pDirection.z));
        newLevelBases.Add(subMesh);
        subMesh.transform.parent = levelBaseMain.transform;
    }

    void CreateDoorMesh(Door pDoor, Vector3 pDirection)
    {

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
#endif
}