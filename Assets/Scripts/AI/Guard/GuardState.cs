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
    /* Get the Guard's current Pathfinding target */
    public abstract Vector3 GetTargetPosition();
    public abstract void OnTargetReached();
    /* Get the Guard's target rotation used for stationary rotation */
    public abstract float GetTargetAngle();
    /* What happens on discraction */
    public abstract void OnDistraction();
    /* What happens when a player is seen */
    public abstract void OnSeePlayer();

    public abstract void OnStateExit();

    private void Start()
    {
        OnStateEnter();
    }

}
