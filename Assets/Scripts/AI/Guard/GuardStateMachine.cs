using System.Collections.Generic;
using UnityEngine;


/** This class is used as the Context class for the Guard State Machine 
 * See https://sourcemaking.com/design_patterns/state and 
 * http://gameprogrammingpatterns.com/state.html for more info on the State Pattern */

public class GuardStateMachine : MonoBehaviour {

    public enum PatrolStyle
    {
       BackAndForth,
       Loop,
       Stationary
    }

    public PatrolStyle patrolStyle = PatrolStyle.Stationary;
    public float rotationSpeed;

    private GuardState state;
    public int LastWaypointIndex { get { return lastWaypointIndex; } set { lastWaypointIndex = value; } }
    private int lastWaypointIndex = 0;
    
    // Use this for initialization
    void Start () {

        GoToState(new PatrollingState(this));

    }

    // Update is called once per frame
    void Update () {
        state.Update();
    }

    public void GoToState(GuardState state)
    {
        if (state != null)
        {
            state.OnStateExit();
        }
        this.state = state;
        state.OnStateEnter();
    }

    private void OnTriggerEnter(Collider other)
    {




    }
}
