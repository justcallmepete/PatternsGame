using System.Collections;
using UnityEngine;

/*
 * The main player class to control the state of the player and other behaviours.
 */
 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Controlable))]
[RequireComponent(typeof(Teleportation))]   
public class MainPlayer : MonoBehaviour
{
    [Header("General settings")]
    // Animation curve for pulling
    public AnimationCurve pullEasing;

    [SerializeField]
    private float outerRadius = 1.3f;
    [SerializeField]
    private float pullSpeed = 1;
    [SerializeField]
    private float teleportDelay = 1;

    private Controlable controlable;

    public enum State
    {
        Free,
        Busy,
        Channelling
    }

    private State currentState = State.Free;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    private void Start()
    {
        controlable = gameObject.GetComponent<Controlable>();
    }

    #region Pull Mechanic
    public void BePulled(GameObject obj, float maxDistance)
    {
        currentState = State.Busy;

        StartCoroutine(MovePlayer(obj, maxDistance));
    }

    private IEnumerator MovePlayer(GameObject obj, float maxDistance)
    {
        float curveTime = 0f;
        float curveAmount = pullEasing.Evaluate(curveTime);

        // Calculate the path
        Vector3 path = gameObject.transform.position - obj.transform.position - 
            (gameObject.transform.position - obj.transform.position).normalized * outerRadius;

        // Set begin position
        Vector3 beginPosition = gameObject.transform.position;

        while (curveTime < pullEasing[pullEasing.length - 1].time)
        {
            // Update easing
            curveTime += Time.deltaTime * pullSpeed * maxDistance / path.magnitude;
            curveAmount = pullEasing.Evaluate(curveTime);

            // Update transform position
            gameObject.transform.position = beginPosition - curveAmount * path;
            yield return new WaitForEndOfFrame();
        }
        
        // Set state to free when target is reached
        currentState = State.Free;
    }

    public void PullPlayer(GameObject obj)
    {
        currentState = State.Busy;

        StartCoroutine(WaitForPlayer(obj));
    }

    #endregion

    public IEnumerator StartTeleport(GameObject pPlayer)
    {
        Debug.Log("Teleport");
        yield return new WaitForSeconds(teleportDelay);

        Vector3 path = pPlayer.transform.position + (gameObject.transform.position - pPlayer.transform.position).normalized * outerRadius;
        gameObject.transform.position = path;
    }
    

    private IEnumerator WaitForPlayer(GameObject obj)
    {

        while (obj.GetComponent<MainPlayer>().IsBusy())
        {
            yield return new WaitForEndOfFrame();
        }

        currentState = State.Free;
    }

    public bool IsBusy()
    {
        return CurrentState == State.Busy;
    }

    public bool IsFree()
    {
        return CurrentState == State.Free;
    }

    public bool IsChannelling()
    {
        return CurrentState == State.Channelling;
    }
}
