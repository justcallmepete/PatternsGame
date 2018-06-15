using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMediator : MonoBehaviour
{
    public List<GuardStateMachine> guards;

    /*
     * This method will be called by the camera or other sensor when a player is spotted.
     */
    public void PlayerStandinInLight(Vector3 position, int guardsToPoint, int guardsOnAlert, float toPointDist, float onAlertDist)
    {
        AssignTaskToGuards(position, guardsToPoint, guardsOnAlert, toPointDist, onAlertDist);
    }

    /*
     * This method is used to assign specific tasks to the guards depending on the distance to a certain point and the amount of guards needed for a specific task
     * First it will assign the task that sends the guards to the location and then it will put the remaining guards on alert on their own spot.
     *
     */
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