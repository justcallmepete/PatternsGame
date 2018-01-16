using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindedGuardState : GuardState
{
    public BlindedGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnDistraction(Vector3 target)
    {
        
    }

    public override void OnSeePlayer(GameObject player)
    {

    }

    public override void OnStateEnter()
    {
        context.Indicator = GuardStateMachine.IndicatorImage.Blinded;

    }

    public override void OnStateExit()
    {
    }

    public override void Update()
    {
        
    }
}
