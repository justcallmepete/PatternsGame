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

        public int TotalCornersInBound(WallsBounds pBoundsToCheck)
        {
            int nCornersIntersecting = 0;
            //TopLeft
            if (IsCornerInBound(new Vector3(-1,0,1), pBoundsToCheck))
            {
                nCornersIntersecting++;
            }
            //Top Right
            if (IsCornerInBound(new Vector3(1, 0, 1), pBoundsToCheck))
            {
                nCornersIntersecting++;
            }
            //Bottom Left
            if (IsCornerInBound(new Vector3(-1, 0, -1), pBoundsToCheck))
            {
                nCornersIntersecting++;
            }
            //Bottom Right
            if (IsCornerInBound(new Vector3(1, 0, -1), pBoundsToCheck))
            {
                nCornersIntersecting++;
            }

            return nCornersIntersecting;
        }

        public bool IsCornerInBound(Vector3 pCorner, WallsBounds pBoundsToCheck)
        {
            if(pCorner.z == 1) //Top side
            {
                if(pCorner.x == 1) //Top right
                {
                    if (maxWallsX > pBoundsToCheck.minWallsX && maxWallsX < pBoundsToCheck.maxWallsX && maxWallsZ < pBoundsToCheck.maxWallsZ && maxWallsZ > pBoundsToCheck.minWallsZ)
                    {
                        return true;
                    }
                }
                else // Top left
                {
                    if (minWallsX > pBoundsToCheck.minWallsX && minWallsX < pBoundsToCheck.maxWallsX && maxWallsZ < pBoundsToCheck.maxWallsZ && maxWallsZ > pBoundsToCheck.minWallsZ)
                    {
                        return true;
                    }
                }
            }
            else //bottom side
            {
                if (pCorner.x == 1) //bottom right
                {
                    if (maxWallsX > pBoundsToCheck.minWallsX && maxWallsX < pBoundsToCheck.maxWallsX && minWallsZ < pBoundsToCheck.maxWallsZ && minWallsZ > pBoundsToCheck.minWallsZ)
                    {
                        return true;
                    }
                }
                else // bottom left
                {
                    if (minWallsX > pBoundsToCheck.minWallsX && minWallsX < pBoundsToCheck.maxWallsX && minWallsZ < pBoundsToCheck.maxWallsZ && minWallsZ > pBoundsToCheck.minWallsZ)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Vector3 CornerTopLeft()
        {
            return new Vector3(-1, 0, 1);
        }
        public static Vector3 CornerTopRight()
        {
            return new Vector3(1, 0, 1);
        }
        public static Vector3 CornerBottomLeft()
        {
            return new Vector3(-1, 0, -1);
        }
        public static Vector3 CornerBottomRight()
        {
            return new Vector3(1, 0, -1);
        }

        public Vector3 GetCornerPosition(Vector3 cornerDirection)
        {
            return Vector3.zero;
        }

        public void UpdateInfo(BoxCollider pBoxCol)
        {
            this = LevelCreatorUtils.BoxColliderToWallbounds(center, pBoxCol);
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

public class MaxStack<T> { 
    private int maxItems;
    private T[] items;

    private int top = 0;
    private int bottom = 0;
    private int count = 0;
    public MaxStack(int pMaxItems)
    {
        maxItems = pMaxItems;
        items = new T[pMaxItems];
    }

    public void Clear()
    {
        count = 0;
    }

    public int Count()
    {
        return count;
    }

    public T Peek()
    {
        return items[top];
    }

    public T Pop()
    {
        count--;
        if (count < 0)
        {
            count = 0;
            return default(T);
        }
        return items[--top];
    }

    public T Push(T pItem)
    {
        T itemToReturn = default(T);
        count++;
        count = count > items.Length ? items.Length : count;

        if (items.Length >= maxItems)
        {
            itemToReturn = items[bottom++];
            bottom = bottom > items.Length ? 0 : bottom;
        }
        items[top++] = pItem;
        top = top > items.Length ? 0 : top;
        //Check if reached max
        return itemToReturn;

    }

}
