using UnityEngine;

/* This is the superstate for all substates for when a guard is "Clueless" */
public class CluelessGuardState : GuardState
{

    public CluelessGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        context.IndicatorColor = Color.cyan;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction(Vector3 target)
    {
        //TODO: handle State transition
        throw new System.NotImplementedException("State Transition not yet handled");
    }

    public override void OnSeePlayer(GameObject player)
    {
        //TODO: handle State transition
        context.GoToState(new ChaseGuardState(context));
        Debug.Log("I see the Player!");
    }

    public override void OnStateExit()
    {
        
    }
}
