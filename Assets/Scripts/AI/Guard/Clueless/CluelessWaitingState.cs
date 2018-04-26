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
        base.OnStateEnter();
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

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

    public override void OnStateExit()
    {
        
    }  
}
