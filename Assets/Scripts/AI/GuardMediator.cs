using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMediator : MonoBehaviour
{
    public List<GuardStateMachine> guards;

    public void PlayerStandinInLight(Vector3 position, int guardsToPoint, int guardsOnAlert, float toPointDist, float onAlertDist)
    {
        AssignTaskToGuards(position, guardsToPoint, guardsOnAlert, toPointDist, onAlertDist);
    }

    private void AssignTaskToGuards(Vector3 fromPoint, int guardsToPoint, int guardsOnAlert, float toPointDist,
        float onAlertDist)
    {
        guards.Sort((a, b) => Vector2.Distance(fromPoint, a.transform.position)
            .CompareTo(
                Vector2.Distance(fromPoint, b.transform.position)));

        for (int i = 0; i < guardsToPoint; i++)
        {
            guards[i].Distract(fromPoint);
        }

        for (int j = 0; j < guardsOnAlert; j++)
        {
            guards[j + guardsToPoint].Distract(guards[j + guardsToPoint].transform.position);

        }
    }
}