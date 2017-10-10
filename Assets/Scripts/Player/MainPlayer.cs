using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody), typeof(Controlable))]
public class MainPlayer : MonoBehaviour
{

    private float minDistance = 1;
    private float pullSpeed = 5;
    public enum State
    {
        Free,
        Busy
    }

    private State currentState = State.Free;

    public void BePulled(GameObject obj)
    {
        currentState = State.Busy;

        StartCoroutine(MoveObject(obj));
    }

    private IEnumerator MoveObject(GameObject obj)
    {
      
        while (Vector3.Distance(gameObject.transform.position, obj.transform.position) > minDistance)
        {
           
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, obj.transform.position, Time.deltaTime * pullSpeed);
            yield return new WaitForEndOfFrame();
        }

        currentState = State.Free;
    }

}
