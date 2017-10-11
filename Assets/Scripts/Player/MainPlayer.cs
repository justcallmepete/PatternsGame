using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Controlable))]
[RequireComponent(typeof(Pull))]
public class MainPlayer : MonoBehaviour
{
    // Animation curve for pulling
    public AnimationCurve pullEasing;

    [SerializeField]
    private float minDistance = 1.3f;
    [SerializeField]
    private float pullSpeed = 1;
    private Controlable controlable;
    public enum State
    {
        Free,
        Busy
    }

    private State currentState = State.Free;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    private void Start()
    {
        controlable = gameObject.GetComponent<Controlable>();
    }

    public void BePulled(GameObject obj, float maxDistance)
    {
        currentState = State.Busy;

        StartCoroutine(MoveObject(obj, maxDistance))    ;
    }

    private IEnumerator MoveObject(GameObject obj, float maxDistance)
    {
        float curveTime = 0f;
        float curveAmount = pullEasing.Evaluate(curveTime);

        Vector3 path = gameObject.transform.position - obj.transform.position - (gameObject.transform.position - obj.transform.position).normalized * minDistance;
        Vector3 beginPosition = gameObject.transform.position;
        float distance = path.magnitude;
        while (curveTime < pullEasing[pullEasing.length - 1].time)
        {
            curveTime += Time.deltaTime * pullSpeed * maxDistance / path.magnitude;
            curveAmount = pullEasing.Evaluate(curveTime);
            gameObject.transform.position = beginPosition - curveAmount * path;
            yield return new WaitForEndOfFrame();
        }
        currentState = State.Free;
    }

    public void PullPlayer(GameObject obj)
    {
        currentState = State.Busy;

        StartCoroutine(WaitForObject(obj));
    }

    private IEnumerator WaitForObject(GameObject obj)
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
}
