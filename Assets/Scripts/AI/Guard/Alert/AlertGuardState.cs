using Mainframe.Utils;
using UnityEngine;

/* This state contains the logic for when the guard can see the player. */
public class AlertGuardState : GuardState
{

    public AlertGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        context.IndicatorColor = Color.red;
        context.MovementSpeed = context.alertMovementSpeed;   
    }

    public override void OnDistraction(Vector3 target)
    {
        //Ignore distractions
        return;
    }

    public override void OnSeePlayer(GameObject player)
    {
        context.NavigationAgent.SetDestination(player.transform.position);
    }

    public override void OnStateExit()
    {
    }

    public override void Update()
    {
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

    private Vector3 GetTargetPosition()
    {
        if (context.TargetPlayer)
        {
            return context.TargetPlayer.transform.position;
        }

        return context.transform.position;
    }

    private void RotateTo(Vector3 rotateTarget)
    {
        Vector3 from = context.transform.forward;
        Vector3 newRotation = Vector3.RotateTowards(from, rotateTarget, Time.deltaTime * context.RotationSpeed * Mathf.Deg2Rad, 0.0f);
        context.transform.rotation = Quaternion.LookRotation(newRotation, Vector3.up);

        //if (Vector3.Angle(context.transform.forward, rotateTarget) < 0.5f)
        //{
        //    OnTargetReached();
        //}
    }

    private float GetDistanceToTarget()
    {
        return NavMeshUtils.CalculatePathLength(context.NavigationAgent.transform.position, GetTargetPosition());
    }
}
