using UnityEngine;

/* This is the superstate for all substates for when a guard is "Clueless" */
public class CluelessGuardState : GuardState
{

    public CluelessGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction()
    {
        //TODO: handle State transition
        throw new System.NotImplementedException("State Transition not yet handled");
    }

    public override void OnSeePlayer()
    {
        //TODO: handle State transition
        throw new System.NotImplementedException("State Transition not yet handled");
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
