using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrol : MonoBehaviour {

	public Waypoint[] GetWaypoints()
    {
        return GetComponentsInChildren<Waypoint>();
    }
}
