using UnityEngine;
/* A simple pathfinding waypoint used in the AI. Guards will move from one to the other top-to-bottom
 in the hierarchy.*/
public class Waypoint : MonoBehaviour
{
    public float duration;
    /* Show Indicator in play mode */
    public bool debugMode;

    private void Update()
    {
        if (!debugMode)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
