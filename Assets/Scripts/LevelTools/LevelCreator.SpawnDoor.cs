using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// This part of LevelCreator handles the methods for placing a door
/// </summary>
public partial class LevelCreator : MonoBehaviour {
    #region
    public void SpawnDoor()
    {
        GetLevelCreatorInfo();
        doorBeingMade = Instantiate(doorPrefab);
        doorDirection = new Vector3(1, 0, 0);
        levelBases = GetAllWalls();

        GameObject[] selectObject = new GameObject[1];
        selectObject[0] = doorBeingMade.gameObject;
        Selection.objects = selectObject;
    }

    public void ToggleRotateDoor()
    {//Rotate clockwise
        if (doorDirection.x != 0)
        {
            doorBeingMade.transform.Rotate(0, -90, 0);
            doorDirection = new Vector3(0, 0, 1);
        }
        else
        {
            doorDirection = new Vector3(1, 0, 0);
            doorBeingMade.transform.Rotate(0, +90, 0);
        }
    }
    public void ConfirmDoorPlacement()
    {
        tempUndoList = new Dictionary<GameObject, UndoRedoState>();
        Vector3 leftWallPos = doorBeingMade.transform.Find("WallSideLeft").position;
        Vector3 rightWallPos = doorBeingMade.transform.Find("WallSideRight").position;
        doors.Add(doorBeingMade);
        AddNewObject(doorBeingMade.gameObject);
        if (doorDirection.x != 0)
        {
            LevelBase topWall = Instantiate(levelBasePrefab);
            LevelBase bottomWall = Instantiate(levelBasePrefab);

            AddNewObject(topWall.gameObject);
            AddNewObject(bottomWall.gameObject);
            //Create top mesh to max
            float topScale = Mathf.Abs(levelCreatorInfo.baseLenght / 2 - leftWallPos.z);
            float bottomScale = Mathf.Abs(rightWallPos.z - levelCreatorInfo.baseLenght / 2);
            topWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, topScale);
            bottomWall.transform.localScale = new Vector3(1, levelCreatorInfo.wallHeight, bottomScale);
            topWall.transform.position = doorBeingMade.transform.position + new Vector3(0, 0, topWall.transform.localScale.z / 2) + new Vector3(0, 0, (leftWallPos.z - rightWallPos.z) / 2);
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
            topWall.transform.parent = levelBaseMain.transform;
            bottomWall.transform.parent = levelBaseMain.transform;


            levelBases.Add(topWall);
            levelBases.Add(bottomWall);
        }
        else
        {
            LevelBase leftWall = Instantiate(levelBasePrefab);
            LevelBase rightWall = Instantiate(levelBasePrefab);

            AddNewObject(leftWall.gameObject);
            AddNewObject(rightWall.gameObject);

            float leftScale = Mathf.Abs(levelCreatorInfo.baseWidth / 2 - leftWallPos.x);
            float rightScale = Mathf.Abs(leftWallPos.x - levelCreatorInfo.baseWidth / 2);
            leftWall.transform.localScale = new Vector3(leftScale, levelCreatorInfo.wallHeight, 1);
            rightWall.transform.localScale = new Vector3(rightScale, levelCreatorInfo.wallHeight, 1);
            leftWall.transform.position = doorBeingMade.transform.position - new Vector3(leftWall.transform.localScale.x / 2, 0, 0) - new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);
            rightWall.transform.position = doorBeingMade.transform.position + new Vector3(rightWall.transform.localScale.x / 2, 0, 0) + new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);

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
                    if (levelBaseBounds.maxWallsX > leftWallPos.x)
                    {
                        Debug.LogWarning("Corner of door is in a wall");
                        continue;
                    }
                    float xDistanceToWall = leftWallPos.x - levelBaseBounds.maxWallsX;
                    if (xDistanceToWall < leftSmallestScale)
                    {
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
            rightWall.transform.localScale = new Vector3(rightSmallestScale, levelCreatorInfo.wallHeight, 1);
            leftWall.transform.position = doorBeingMade.transform.position - new Vector3(leftWall.transform.localScale.x / 2, 0, 0) - new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);
            rightWall.transform.position = doorBeingMade.transform.position + new Vector3(rightWall.transform.localScale.x / 2, 0, 0) + new Vector3((rightWallPos.x - leftWallPos.x) / 2, 0, 0);
            leftWall.transform.parent = levelBaseMain.transform;
            rightWall.transform.parent = levelBaseMain.transform;

            leftWall.transform.name = "leftWall";
            rightWall.transform.name = "rightWall";
            levelBases.Add(leftWall);
            levelBases.Add(rightWall);
        }

        AddToUndo(tempUndoList);
    }

    public void DenyDoorPlacement()
    {
        DestroyImmediate(doorBeingMade.gameObject);
    }
    #endregion
}
