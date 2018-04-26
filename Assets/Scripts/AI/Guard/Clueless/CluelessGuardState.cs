using UnityEngine;

/* This is the superstate for all substates for when a guard is "Clueless" */
public class CluelessGuardState : GuardState
{

    public CluelessGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        context.Indicator = GuardStateMachine.IndicatorImage.None;
        context.MovementSpeed = context.cluelessMovementSpeed;
        context.VisionColor = Color.cyan;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction(Vector3 target)
    {
        context.GoToState(new SearchingGuardState(context, target));
    }

    public override void OnSeePlayer(GameObject player)
    {
        //TODO: handle State transition
        context.GoToState(new ChaseGuardState(context));
        //Debug.Log("I see the Player!");
    }

    public override void OnStateExit()
    {
        
    }
}
