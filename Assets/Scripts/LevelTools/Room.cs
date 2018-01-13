using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour {
    public bool movedRoom;
    private Vector3 oldPosition;

    [HideInInspector]
    public float editorSize;
    [HideInInspector]
    public int numerator, denominator;

    private void OnEnable()
    {
        editorSize = 0;
        numerator = 1;
        denominator = 1;
    }

    void Start()
    {
        //Check that the first GameObject exists in the Inspector and fetch the Collider
        
    }

    void Update()
    {
        if (Selection.Contains(this.gameObject))
        {
            bool mouseWasDown = false;
            if (Input.GetMouseButton(0))
            {
                Debug.Log("mouse down");
                mouseWasDown = true;
            }
            else if (mouseWasDown)
            {
                Debug.Log("mouse was down");
            }
                //Check if moving
                //If moving
                //If not holding mouse button
                //if not moving
                //If was moving
                //Update meshes
        }
        if (!Selection.Contains(this.gameObject))
        {
            if (movedRoom)
            {
                Debug.Log("moved room");
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
    
    public void UpdateSize(bool isRotated = false, float pSize = Mathf.Infinity)
    {
        //update numerator and denominator
        if (isRotated)
        {
            transform.localScale = new Vector3(editorSize, 4, editorSize / denominator * numerator);
        }
        else
        {

            transform.localScale = new Vector3(editorSize / denominator * numerator, 4, editorSize);
        }
    }
}
