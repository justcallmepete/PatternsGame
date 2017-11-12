using System;
using UnityEngine;

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

        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        if (GetDistanceToTarget() > context.stoppingDistance)
        {
            context.NavigationAgent.SetDestination(GetTargetPosition());
            Debug.Log(GetDistanceToTarget());
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
        base.OnStateExit();
    }

    public override Vector3 GetTargetPosition()
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
        throw new NotImplementedException();
    }
}
