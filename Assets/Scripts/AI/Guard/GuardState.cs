using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardState {

    protected GuardStateMachine context;

    public GuardState(GuardStateMachine context)
    {
        this.context = context;
    }

    public abstract void OnStateEnter();
    public abstract void Update();
    public abstract void OnStateExit();



    /* Get the Guard's current Pathfinding target */
    public abstract Vector3 GetTargetPosition();
    public abstract void OnTargetReached();
    /* Get the Guard's target rotation used for stationary rotation */
    public abstract float GetTargetAngle();
    /* What happens on discraction */
    public abstract void OnDistraction();
    /* What happens when a player is seen */
    public abstract void OnSeePlayer();

    

}
