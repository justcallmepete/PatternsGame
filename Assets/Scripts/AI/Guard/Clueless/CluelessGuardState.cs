using System.Collections.Generic;
using UnityEngine;

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
        //TODO: handle State transition
        throw new System.NotImplementedException();
    }

    public override void OnSeePlayer()
    {
        //TODO: handle State transition
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
