using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* This state is for waiting and looking around. */
public class WaitingState : GuardState {


    public WaitingState(GuardStateMachine context) : base(context)
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

    public override float GetTargetAngle()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSeePlayer()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTargetReached()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }  
}
