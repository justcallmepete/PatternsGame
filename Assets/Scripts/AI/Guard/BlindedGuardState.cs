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
        context.vision.SetActive(false);
        context.NavigationAgent.isStopped = true;

    }

    public override void OnStateExit()
    {
        context.vision.SetActive(true);
        context.NavigationAgent.isStopped = false;
    }

    public override void Update()
    {
        
    }
}
