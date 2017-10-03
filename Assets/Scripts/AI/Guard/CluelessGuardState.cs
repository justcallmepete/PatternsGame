using System.Collections.Generic;
using UnityEngine;

public class CluelessGuardState : GuardState {


    private int waypointIndex = 0;
    public Waypoint[] waypoints;

    public CluelessGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        waypoints = context.waypoints;

        waypointIndex = context.LastWaypointIndex;
        
    }

    public override float GetTargetAngle()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetTargetPosition()
    {
        
        if (waypoints != null && waypoints.Length > 0)
        {
            return waypoints[waypointIndex].gameObject.transform.position;
        }
        else
        {
            return context.gameObject.transform.position;
        }
    }

    public override void OnDistraction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSeePlayer()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        context.LastWaypointIndex = waypointIndex;
    }

    public override void OnTargetReached()
    {
        Debug.Log("Target Reached");

        if (waypointIndex == waypoints.Length -1)
        {
            waypointIndex = 0;
        } else
        {
            waypointIndex++;
        }
    }
}
