using System.Collections;
using UnityEngine;

/* 
 * This script will rotate and/or move the laser. 
 * Patrol route is needed, which is the same prefab for the guards patrol.
 * All information is in the way point (position, rotation and duration).
 */ 

public class LaserMovement : MonoBehaviour
{
    [Header("General settings")]
    [Tooltip("Units per second")]
    public float moveSpeed = 1f;
    [Tooltip("Radial per second")]
    public float rotationSpeed = 1f;

    private int currentPathIndex = 0;
    private Waypoint[] patrolPoints;

    public bool isMoveable = true;

    private float step;
    public enum State
    {
        Move,
        Rotate,
        Wait,
    }

    private State currentState = State.Move;
    private bool waiting = false;

    void Start()
    {
        patrolPoints = gameObject.transform.parent.GetComponentInChildren<GuardPatrol>().GetWaypoints();
    }

    void FixedUpdate()
    {
        if (!isMoveable)
        {
            return;
        }

        switch (currentState)
        {
            case State.Move:
                MoveLaser();
                break;
            case State.Rotate:
                RotateLaser();
                break;
            case State.Wait:
                if (!waiting)
                {
                    StartCoroutine(NextPatrolPoint(patrolPoints[currentPathIndex].duration));
                }
                break;
            default:
                MoveLaser();
                break;
        }
    }

    private void MoveLaser()
    {
        // Update step
        step = moveSpeed * Time.deltaTime;
    
        // Set new position
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPathIndex].transform.position, step);
        // Change state if reached target
        if (Vector3.Distance(transform.position, patrolPoints[currentPathIndex].transform.position) < 0.1f)
        {
            ChangeState(State.Rotate);
        }
    }

    private void RotateLaser()
    {
        // Check if match rotation is true
        if (!patrolPoints[currentPathIndex].matchRotation)
        {
            ChangeState(State.Wait);
            return;
        }

        // Update step
        step = moveSpeed * Time.deltaTime;

        // Set new rotation
        Vector3 newDir = Vector3.RotateTowards(transform.forward, patrolPoints[currentPathIndex].transform.forward, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);

        // Change state is rotation is reached
        if (Vector3.Angle(transform.forward, patrolPoints[currentPathIndex].transform.forward) < 1f)
        {
            ChangeState(State.Wait);
        }
    }

    IEnumerator NextPatrolPoint(float pDuration)
    {
        waiting = true;

        // Wait for duration
        yield return new WaitForSeconds(pDuration);

        // Update current path index
        currentPathIndex++;

        if (patrolPoints.Length <= currentPathIndex)
        {
            currentPathIndex = 0;
        }

        // Change state to move
        ChangeState(State.Move);

        waiting = false;
    }

    private void ChangeState(State state)
    {
        currentState = state;
        step = 0;
    }
}
