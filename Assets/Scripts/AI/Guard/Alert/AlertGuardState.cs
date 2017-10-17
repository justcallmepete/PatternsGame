using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlertGuardState : GuardState
{
    private float deathTime = 3.0f;
    private float timer;

    private bool canSeePlayer;


    public AlertGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction(Vector3 target)
    {
        return;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > deathTime)
        {
            context.Shoot();
        }
    }

    public override void OnSeePlayer(GameObject player)
    {
        context.NavigationAgent.SetDestination(player.transform.position);
    }

    public override void OnStateEnter()
    {
        timer = 0.0f;
        context.IndicatorColor = Color.red;
    }

    public override void OnStateExit()
    {
    }

   
}
