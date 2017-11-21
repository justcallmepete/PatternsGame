using UnityEngine;
using Mainframe.Utils;

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
        if (GetDistanceToTarget() > context.stoppingDistance)
        {
            context.NavigationAgent.isStopped = false;
            context.NavigationAgent.SetDestination(GetTargetPosition());
        }
        else
        {
            context.NavigationAgent.isStopped = true;
            Vector3 guardPos = Vector3.ProjectOnPlane(context.NavigationAgent.transform.position, Vector3.up);
            Vector3 playerPos = Vector3.ProjectOnPlane(context.TargetPlayer.transform.position, Vector3.up);            //playerPos.y = 0.0f;

            Vector3 target = -(guardPos - playerPos);
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
        context.transform.rotation = Quaternion.LookRotation(newRotation, Vector3.up);

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
        return NavMeshUtils.CalculatePathLength(context.NavigationAgent.transform.position, GetTargetPosition());
    }

}
