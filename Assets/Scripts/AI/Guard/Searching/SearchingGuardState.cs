using System;
using UnityEngine;
using UnityEngine.AI;

/* This is the state for when the guard is investigating a sound or something he saw */
public class SearchingGuardState : GuardState
{

    protected Vector3 targetPosition;

    public SearchingGuardState(GuardStateMachine context, Vector3 targetPosition) : base(context)
    {
        this.targetPosition = targetPosition;
    }

    private Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public override void OnDistraction(Vector3 target)
    {
        context.GoToState(new SearchingGuardState(context, target));
    }

    public override void OnSeePlayer(GameObject player)
    {
        context.GoToState(new ChaseGuardState(context));
    }

    public override void OnStateEnter()
    {
        context.GetComponent<NavMeshAgent>().SetDestination(targetPosition);
        context.IndicatorColor = Color.yellow;
        context.MovementSpeed = context.searchingMovementSpeed;
    }

    public override void OnStateExit()
    {
       
    }

    public override void Update()
    {
        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        if (Vector2.Distance(currentPos, targetPos) < 1.0f && Math.Abs(context.NavigationAgent.velocity.magnitude) < 0.01f)
        {
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        context.GoToState(new InvestigationGuardState(context, targetPosition));
    }
}
