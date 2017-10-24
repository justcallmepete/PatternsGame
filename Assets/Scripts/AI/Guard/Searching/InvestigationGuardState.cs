using UnityEngine;

/*
 * This state is for the Looking Around Behaviour of the Guard.
 */
public class InvestigationGuardState : SearchingGuardState
{

    private float lerpSpeed;
    private float lerpTime;

    public InvestigationGuardState(GuardStateMachine context, Vector3 targetPosition) : base(context, targetPosition)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override Vector3 GetTargetPosition()
    {
        return base.GetTargetPosition();
    }

    public override void OnDistraction(Vector3 target)
    {
        base.OnDistraction(target);
    }

    public override void OnSeePlayer(GameObject player)
    {
        base.OnSeePlayer(player);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private void RotateGuard(float from, float to)
    {
        Vector3 toVector = new Vector3(0.0f, to, 0.0f);
        lerpTime += lerpSpeed * Time.deltaTime;
        if (Vector3.Distance(context.transform.eulerAngles, toVector) > 2.0f)
        {
            context.transform.eulerAngles = new Vector3(0.0f, Mathf.LerpAngle(from, to, lerpTime), 0.0f);
        }
        else
        {
            context.transform.eulerAngles = toVector;
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {

    }

}
