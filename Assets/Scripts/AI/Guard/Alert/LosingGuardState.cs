using UnityEngine;

/**
 *  When a guard loses sight of a player, they still know where that player is for a few seconds.
 *  This state exsist for that 
 */
public class LosingGuardState : AlertGuardState {

    public LosingGuardState(GuardStateMachine context) : base(context)
    {
    }

    private float chaseTimer;

    public override void OnStateEnter()
    {
        chaseTimer = 0.0f;
        
    }

    public override void OnSeePlayer(GameObject player)
    {
        context.GoToState(new ChaseGuardState(context));
    }

    public override void Update()
    {
        base.Update();
        chaseTimer += Time.deltaTime;
        if (chaseTimer > context.chaseTime)
        {
            context.ForgetPlayer();
        }
    }
}

