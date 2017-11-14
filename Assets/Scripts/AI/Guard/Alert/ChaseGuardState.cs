using UnityEngine;
using UnityEngine.AI;

/*
 * In this state the guard is chasing the player.
 */
public class ChaseGuardState : AlertGuardState
{
    public ChaseGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void Update()
    {
        base.Update();
        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        if (GetDistanceToTarget() > context.stoppingDistance)
        {
            context.NavigationAgent.isStopped = false;
            context.NavigationAgent.SetDestination(GetTargetPosition());
        }
        else
        {
            context.NavigationAgent.isStopped = true;
            Vector3 target = -(context.transform.position - context.TargetPlayer.transform.position);
            RotateTo(target.normalized);
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("EXIT CHASE");
        context.NavigationAgent.isStopped = false;
        base.OnStateExit();
    }

    private Vector3 GetTargetPosition()
    {
        if (context.TargetPlayer)
        {
            return context.TargetPlayer.transform.position;
        }

        return context.transform.position;
    }

    public override void OnDistraction(Vector3 target)
    {
        base.OnDistraction(target);
    }

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

    private void RotateTo(Vector3 rotateTarget)
    {
        Vector3 from = context.transform.forward;
        Vector3 newRotation = Vector3.RotateTowards(from, rotateTarget, Time.deltaTime * context.rotationSpeed * Mathf.Deg2Rad, 0.0f);
        context.transform.rotation = Quaternion.LookRotation(newRotation);
        if (Vector3.Angle(context.transform.forward, rotateTarget) < 0.5f)
        {
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        //Nothing here yet.
    }

    private float GetDistanceToTarget()
    {
        Vector3 currentPos = context.NavigationAgent.transform.position;
        Vector3 targetPos = GetTargetPosition();

        NavMeshPath path = new NavMeshPath();
        path.ClearCorners();
        if (NavMesh.CalculatePath(currentPos, targetPos, 1, path))
        {
            float lng = 0.0f;

            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }
        return 0.0f;
    }
}
