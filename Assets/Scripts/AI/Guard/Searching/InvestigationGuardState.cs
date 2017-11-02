
using UnityEngine;

/*
 * This state is for the Looking Around Behaviour of the Guard.
 */
public class InvestigationGuardState : SearchingGuardState
{
    private Vector3 startRotation;
    private Vector3 targetRotation;
    private bool startUpdated;

    private bool done;

    private float lerpSpeed;
    private float lerpTime;

    public InvestigationGuardState(GuardStateMachine context, Vector3 targetPosition) : base(context, targetPosition)
    {
    }

    public override void OnStateEnter()
    {
       //base.OnStateEnter();
        lerpSpeed = context.rotationSpeed / 180.0f;

        startRotation = context.transform.rotation.eulerAngles;
        targetRotation = new Vector3(startRotation.x, startRotation.y + 90.0f, startRotation.z);
        
        startUpdated = false;
    }

    public override void Update()
    {
        //base.Update();
        if (!startUpdated)
        {
            startRotation = context.transform.rotation.eulerAngles;
            startUpdated = true;
            lerpTime = 0.0f;
        }
        RotateGuard(startRotation.y, targetRotation.y);
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
        float deltalerp = Time.deltaTime * (1 / (System.Math.Abs(from - to) / context.rotationSpeed));
        lerpTime += deltalerp;
        if (lerpTime < 1.0f)
        {
            context.transform.eulerAngles = new Vector3(0.0f, Mathf.LerpAngle(from, to, lerpTime), 0.0f);
        }
        else
        {
            context.transform.eulerAngles = toVector;
            Debug.Log(toVector);
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        Debug.Log("Target!");
        if (!done)
        {
            startRotation = targetRotation;
            targetRotation = new Vector3(startRotation.x, startRotation.y - 90.0f, startRotation.z);
            lerpTime = 0.0f;
            done = true;
        } else
        {
            context.GoToState(new PatrollingState(context));
        }
    }

}
