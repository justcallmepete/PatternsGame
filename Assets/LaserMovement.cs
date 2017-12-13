using System.Collections;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
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
    // Use this for initialization
    void Start()
    {
        patrolPoints = gameObject.transform.parent.GetComponentInChildren<GuardPatrol>().GetWaypoints();
    }

    // Update is called once per frame
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
        step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPathIndex].transform.position, step);

        if (Vector3.Distance(transform.position, patrolPoints[currentPathIndex].transform.position) < 0.1f)
        {
            ChangeState(State.Rotate);
        }
    }

    private void RotateLaser()
    {
        if (!patrolPoints[currentPathIndex].matchRotation)
        {
            ChangeState(State.Wait);
            return;
        }

        step = moveSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, patrolPoints[currentPathIndex].transform.forward, step, 0.0F);

        transform.rotation = Quaternion.LookRotation(newDir);

        if (Vector3.Angle(transform.forward, patrolPoints[currentPathIndex].transform.forward) < 1f)
        {
            ChangeState(State.Wait);
        }
    }

    IEnumerator NextPatrolPoint(float pDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(pDuration);

        currentPathIndex++;

        if (patrolPoints.Length <= currentPathIndex)
        {
            currentPathIndex = 0;
        }

        ChangeState(State.Move);

        waiting = false;
    }

    private void ChangeState(State state)
    {
        currentState = state;
        step = 0;
    }
}
