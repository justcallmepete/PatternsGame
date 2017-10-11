using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* This state is for idling in the clueless state. The guard will wait and enter
 * The goToState*/
public class CluelessWaitingState : CluelessGuardState {

    float waitingTime;
    float timer;

    GuardState goToState;

    public CluelessWaitingState(GuardStateMachine context, float waitForSeconds, GuardState goToState) : base(context)
    {
        waitingTime = waitForSeconds;
        this.goToState = goToState;
    }

    public override void OnStateEnter()
    {
        timer = 0.0f;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitingTime)
        {
            context.GoToState(this.goToState);
        }
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
        base.OnDistraction();
    }

    public override void OnSeePlayer()
    {
        base.OnSeePlayer();
    }

    public override void OnTargetReached()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        
    }  
}
