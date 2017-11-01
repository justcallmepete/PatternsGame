﻿using System;
using UnityEngine;
using UnityEngine.AI;

/* This state handles all logic for Patrolling Guards */
public class PatrollingState : CluelessGuardState {

    private int waypointIndex = 0;
    private bool incrementUp = true;
    public Waypoint[] waypoints;

    private NavMeshAgent navMeshAgent;

    private Vector3 startRotation;
    private bool startUpdated;

    private float lerpSpeed;
    private float lerpTime;


    public PatrollingState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        GuardPatrol patrol = context.gameObject.transform.parent.gameObject.GetComponentInChildren<GuardPatrol>();

        waypoints = patrol.GetWaypoints();

        waypointIndex = context.LastWaypointIndex;
        navMeshAgent = context.GetComponent<NavMeshAgent>();

        startRotation = context.transform.rotation.eulerAngles;
        startUpdated = false;

        lerpSpeed = context.rotationSpeed / 180.0f;
}

public override void Update()
    {
       navMeshAgent.SetDestination(GetTargetPosition());

        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        if (Vector2.Distance(currentPos, targetPos) < 1.0f && Math.Abs(navMeshAgent.velocity.magnitude) < 0.01f)
        {
            if(!waypoints[waypointIndex].matchRotation)
            {
                OnTargetReached();
            }
            if (!startUpdated)
            {
                startRotation = context.transform.rotation.eulerAngles;
                startUpdated = true;
                lerpTime = 0.0f;
            }
            RotateGuard(startRotation.y, waypoints[waypointIndex].transform.eulerAngles.y);
        }
    }

    /* Rotate guard to the same rotation as the Waypoint */
    private void RotateGuard(float from, float to)
    {
            Vector3 toVector = new Vector3(0.0f, to, 0.0f );
            lerpTime += lerpSpeed * Time.deltaTime;
            if (Vector3.Distance(context.transform.eulerAngles, toVector) > 2.0f)
            {
            context.transform.eulerAngles = new Vector3(0.0f, Mathf.LerpAngle(from, to, lerpTime), 0.0f);
            }
            else
            {
                context.transform.eulerAngles = toVector;
                OnTargetReached();
            }
    }

    private float GetTargetAngle()
    {
        return waypoints[waypointIndex].gameObject.transform.rotation.y;
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

    public override void OnDistraction(Vector3 target)
    {
        context.GoToState(new SearchingGuardState(context, target));
    }

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

    private void OnTargetReached()
    {
        //Debug.Log("Target Reached");

        int oldIndex = waypointIndex;

        startUpdated = false;

        waypointIndex = SelectNewWaypoint();

        context.LastWaypointIndex = waypointIndex;


        //if the waypoint has a waitingtime go to the waiting state and return here when done waiting.
        if (waypoints[oldIndex].duration > 0.0f)
        {
            context.GoToState(new CluelessWaitingState(context, waypoints[oldIndex].duration, this));
        }
    }

    private int SelectNewWaypoint()
    {
        switch(context.patrolStyle)
        {
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
