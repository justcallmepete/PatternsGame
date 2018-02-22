using UnityEngine;
using Mainframe.Utils;

/*
 * In this state the guard is chasing the player.
 */
public class ChaseGuardState : AlertGuardState
{

    private float timer;
    private ChargeSystem chargeSystem;


    public ChaseGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        timer = 0.0f;

        chargeSystem = context.transform.GetComponentInChildren<ChargeSystem>();
        chargeSystem.BeginCharge();
    }

    public override void Update()
    {
        base.Update();

        if (!context.PlayerVisible)
        {
            context.GoToState(new LosingGuardState(context));
        }
    }

    public override void OnStateExit()
    {
       // Debug.Log("EXIT CHASE");
        chargeSystem.StopCharge();
        base.OnStateExit();
    }

    public override void OnDistraction(Vector3 target)
    {
        base.OnDistraction(target);
    }

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

}
