using System;
using UnityEngine;
using UnityEngine.AI;

/* This state handles all logic for Patrolling Guards */
public class PatrollingState : CluelessGuardState {
    private int waypointIndex = 0;
    private bool incrementUp = true;
    public Waypoint[] waypoints;

    private NavMeshAgent navMeshAgent;

    public PatrollingState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        GuardPatrol patrol = context.PatrolRoute;
        waypoints = patrol != null ?  patrol.GetWaypoints() : new Waypoint[0];

        waypointIndex = context.LastWaypointIndex;
        navMeshAgent = context.GetComponent<NavMeshAgent>();
}

    public override void Update()
    {
        navMeshAgent.SetDestination(GetTargetPosition());

        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        if (Vector2.Distance(currentPos, targetPos) < 0.5f && Math.Abs(navMeshAgent.velocity.magnitude) < 0.01f)
        {
            if (waypoints.Length != 0 && !waypoints[waypointIndex].matchRotation)
            {
                OnTargetReached();
            }
            else
            {
                RotateTo(GetWaypointRotation());
            }
            }
        }

    private void RotateTo(Vector3 rotateTarget)
    {
        Vector3 from = context.transform.forward;
        Vector3 newRotation =  Vector3.RotateTowards(from, rotateTarget, Time.deltaTime * context.RotationSpeed * Mathf.Deg2Rad, 0.0f);
        context.transform.rotation = Quaternion.LookRotation(newRotation);
        if(Vector3.Angle(context.transform.forward, rotateTarget) < 0.5f)
        {
            OnTargetReached();
        }
    }

    private Vector3 GetWaypointRotation()
    {
        return waypoints.Length == 0 ? context.StartRotation : waypoints[waypointIndex].gameObject.transform.forward;
    }

    private Vector3 GetTargetPosition()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            return waypoints[waypointIndex].gameObject.transform.position;
        }
        else
        {
            return context.StartLocation;
        }
    }

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

    private void OnTargetReached()
    {

        int oldIndex = waypointIndex;

        waypointIndex = SelectNewWaypoint();

        context.LastWaypointIndex = waypointIndex;


        //if the waypoint has a waitingtime go to the waiting state and return here when done waiting.

        if (waypoints.Length != 0 && waypoints[oldIndex].duration > 0.0f)
        {
            context.GoToState(new CluelessWaitingState(context, waypoints[oldIndex].duration, this));
        }
    }

    private int SelectNewWaypoint()
    {
        switch(context.patrolStyle)
        {
            case GuardStateMachine.PatrolStyle.Stationary:
                return waypointIndex;
            case GuardStateMachine.PatrolStyle.Loop:
                if (waypointIndex == waypoints.Length - 1)
                {
                    return 0;
                }
                else
                {
                    return waypointIndex + 1;
                }
            case GuardStateMachine.PatrolStyle.BackAndForth:
                if (incrementUp)
                {
                    if (waypointIndex == waypoints.Length - 1)
                    {
                        incrementUp = !incrementUp;
                        return waypoints.Length - 1;
                     
                    }
                    else
                    {
                        return waypointIndex + 1;
                    }
                } else
                {
                    if (waypointIndex == 0)
                    {
                        incrementUp = !incrementUp;
                        return 0;
                    }
                    else
                    {
                        return waypointIndex - 1;
                    }
                }
            case GuardStateMachine.PatrolStyle.Roaming:
                return UnityEngine.Random.Range(0, waypoints.Length);

            default:
                if (waypointIndex == waypoints.Length - 1)
                {
                    return 0;
                }
                else
                {
                    return waypointIndex + 1;
                }
        }
        }

    public override void OnStateExit()
    {
        context.LastWaypointIndex = waypointIndex;
    }
} 
