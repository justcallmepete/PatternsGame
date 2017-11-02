using UnityEngine;

/* This is the script for the GuardPatrol Object. Use it to group Waypoints */
public class GuardPatrol : MonoBehaviour {

	public Waypoint[] GetWaypoints()
    {
        return GetComponentsInChildren<Waypoint>();
    }
}
