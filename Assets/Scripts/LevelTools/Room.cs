using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour {
    public bool movedRoom;
    private Vector3 oldPosition;

    [HideInInspector]
    public float editorSize;
    [HideInInspector]
    public int numerator, denominator;

    [HideInInspector]
    public LevelCreatorUtils.WallsBounds wallBounds;
    [HideInInspector]
    public List<Room> intersectingRooms; 

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
        UpdatePreviousInfo();
        //If the first GameObject's Bounds enters the second GameObject's Bounds, output the message

        oldPosition = this.transform.position;
    }

    public void ConfirmPLacement()
    {
        wallBounds = LevelCreatorUtils.BoxColliderToWallbounds(transform.position, GetComponent<BoxCollider>());
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
