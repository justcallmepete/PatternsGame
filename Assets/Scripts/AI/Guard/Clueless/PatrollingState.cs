using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingState : GuardState {

    private int waypointIndex = 0;
    public Waypoint[] waypoints;
    private NavMeshAgent navMeshAgent;

    public PatrollingState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        waypoints = context.waypoints;

        waypointIndex = context.LastWaypointIndex;

        navMeshAgent = context.GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
       navMeshAgent.SetDestination(GetTargetPosition());

        if (Vector3.Distance(context.gameObject.transform.position, GetTargetPosition()) < 2.0f)
        {
            OnTargetReached();
        }
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

    public override void OnTargetReached()
    {
        Debug.Log("Target Reached");

        if (waypointIndex == waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }
    }

    public override void OnStateExit()
    {
        context.LastWaypointIndex = waypointIndex;
    }
}
