using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    public static WallsBounds BoxColliderToWallbounds(Vector3 pPosition, BoxCollider pWallCollider)
    {
        WallsBounds newWallBounds = new WallsBounds();
        Vector3 extends = pWallCollider.bounds.extents;
        
        newWallBounds.center = pPosition;
        newWallBounds.minWallsX = newWallBounds.center.x - extends.x;
        newWallBounds.maxWallsX = newWallBounds.center.x + extends.x;
        newWallBounds.minWallsZ = newWallBounds.center.z - extends.z;
        newWallBounds.maxWallsZ = newWallBounds.center.z + extends.z;
        return newWallBounds;
    }

    public struct WallsBounds
    {
        public Vector3 center;
        public float minWallsX;
        public float minWallsZ;
        public float maxWallsX;
        public float maxWallsZ;

        public override string ToString()
        {
            return "center: "+ center.ToString()+ " minWallsX: "+ minWallsX + " minWallsZ: "+ minWallsZ + " maxWallsX: " + maxWallsX + " maxWallsZ: " + maxWallsZ;
        }

        public void DrawCorners(float pDuration)
        {
            Debug.Log("draw corner");
            Debug.DrawLine(center, new Vector3(minWallsX, 0, maxWallsZ), Color.red, pDuration); // Top left
            Debug.DrawLine(center, new Vector3(maxWallsX, 0, maxWallsZ), Color.red, pDuration); // Top right
            Debug.DrawLine(center, new Vector3(minWallsX, 0, minWallsZ), Color.red, pDuration); // Bottom left
            Debug.DrawLine(center, new Vector3(maxWallsX, 0, minWallsZ), Color.red, pDuration); // Bottom right
        }
    }

    public static WallsBounds ExtendWallBounds(WallsBounds boundsToExtend, WallsBounds boundsReference)
    {
        if (boundsReference.minWallsX < boundsToExtend.minWallsX)
        {
            boundsToExtend.center.x -= (boundsToExtend.minWallsX - boundsReference.minWallsX) / 2;
            boundsToExtend.minWallsX = boundsReference.minWallsX;
        }
        if (boundsReference.minWallsZ < boundsToExtend.minWallsZ)
        {
            boundsToExtend.center.z -= (boundsToExtend.minWallsZ - boundsReference.minWallsZ) / 2;
            boundsToExtend.minWallsZ = boundsReference.minWallsZ;
        }
        if (boundsReference.maxWallsX > boundsToExtend.maxWallsX)
        {
            boundsToExtend.center.x -= (boundsToExtend.maxWallsX - boundsReference.maxWallsX) / 2;
            boundsToExtend.maxWallsX = boundsReference.maxWallsX;
        }
        if (boundsReference.maxWallsZ > boundsToExtend.maxWallsZ)
        {
            boundsToExtend.center.z -= (boundsToExtend.minWallsZ - boundsReference.minWallsZ) / 2;
            boundsToExtend.maxWallsZ = boundsReference.maxWallsZ;
        }
        return boundsToExtend;
    }
}
