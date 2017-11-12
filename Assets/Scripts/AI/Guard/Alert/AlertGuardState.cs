using UnityEngine;
using UnityEngine.AI;

/* This state contains the logic for when the guard can see the player. */
public class AlertGuardState : GuardState
{
    private float deathTime = 3.0f;
    private float timer;

    private bool canSeePlayer;


    public AlertGuardState(GuardStateMachine context) : base(context)
    {
    }

    public override void OnStateEnter()
    {
        timer = 0.0f;
        context.IndicatorColor = Color.red;
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDistraction(Vector3 target)
    {
        //Ignore distractions
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

    public override void OnStateExit()
    {
    }

    protected float GetDistanceToTarget()
    {
        Vector2 currentPos = new Vector2(context.gameObject.transform.position.x, context.gameObject.transform.position.z);
        Vector2 targetPos = new Vector2(GetTargetPosition().x, GetTargetPosition().z);

        NavMeshPath path = new NavMeshPath();
        path.ClearCorners();
        if (NavMesh.CalculatePath(currentPos, targetPos, 0, path))
        {
            float lng = 0.0f;

            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }
        return 0.0f;
    }
    }
