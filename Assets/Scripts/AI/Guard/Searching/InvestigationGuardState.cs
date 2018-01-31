
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
        startRotation = context.transform.forward;
    }

    public override void OnStateEnter()
    {
       //base.OnStateEnter();
        lerpSpeed = context.RotationSpeed / 180.0f;

        startRotation = context.transform.rotation.eulerAngles;
        targetRotation = Quaternion.AngleAxis(80, startRotation) * Vector3.left;


        done = false;
    }

    public override void Update()
    {
        RotateTo(targetRotation);
    }

    private Vector3 GetTargetPosition()
    {
        return base.targetPosition;
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

    private void RotateTo(Vector3 rotateTarget)
    {
        Vector3 from = context.transform.forward;
        Vector3 newRotation = Vector3.RotateTowards(from, rotateTarget, Time.deltaTime * context.RotationSpeed * Mathf.Deg2Rad, 0.0f);
        context.transform.rotation = Quaternion.LookRotation(newRotation);
        if (Vector3.Angle(context.transform.forward, rotateTarget) < 0.5f)
        {
            OnTargetReached();
        }
    }

    private void RotateGuard(float from, float to)
    {
        Vector3 toVector = new Vector3(0.0f, to, 0.0f);
        float deltalerp = Time.deltaTime * (1 / (System.Math.Abs(from - to) / context.RotationSpeed));
        lerpTime += deltalerp;
        if (lerpTime < 1.0f)
        {
            context.transform.eulerAngles = new Vector3(0.0f, Mathf.LerpAngle(from, to, lerpTime), 0.0f);
        }
        else
        {
            context.transform.eulerAngles = toVector;
            //Debug.Log(toVector);
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        if (!done)
        {
            targetRotation = Quaternion.AngleAxis(80, startRotation) * Vector3.right;
            done = true;
        } else
        {
            context.GoToState(new PatrollingState(context));
        }
    }

}
