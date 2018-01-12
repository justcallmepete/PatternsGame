using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour {
    public bool movedRoom;
    private Vector3 oldPosition;

    void Start()
    {
        //Check that the first GameObject exists in the Inspector and fetch the Collider
        
    }

    void Update()
    {
        if (!Selection.Contains(this.gameObject))
        {
            if (movedRoom)
            {
                LevelCreator lc = (LevelCreator)FindObjectOfType(typeof(LevelCreator));
                movedRoom = false;
                // Get room width and lenght
                lc.UpdateMeshes(this);
            }
            return;
        }
        UpdatePreviousInfo();
        //If the first GameObject's Bounds enters the second GameObject's Bounds, output the message

        oldPosition = this.transform.position;
    }

    void UpdatePreviousInfo()
    {
        if (this.transform.position != oldPosition)
        {
            movedRoom = true;
        }        
    }
}
