using UnityEngine;
using UnityEngine.AI;

/* This state contains the logic for when the guard can see the player. */
public class AlertGuardState : GuardState
{
    private float deathTime = 3.0f;
    private float timer;

    private bool canSeePlayer;

    private ChargeSystem chargeSystem;

    public AlertGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        timer = 0.0f;
        context.IndicatorColor = Color.red;

        chargeSystem = context.transform.GetComponentInChildren<ChargeSystem>();
        chargeSystem.BeginCharge();
    }

    public override void OnDistraction(Vector3 target)
    {
        //Ignore distractions
        return;
    }

    public override void Update()
    {

    }

    public override void OnSeePlayer(GameObject player)
    {
        context.NavigationAgent.SetDestination(player.transform.position);
    }

    public override void OnStateExit()
    {
        chargeSystem.StopCharge();
    }
}
