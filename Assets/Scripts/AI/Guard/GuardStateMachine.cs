using UnityEngine;
using UnityEngine.AI;

/** This class is used as the Context class for the Guard State Machine 
 * See https://sourcemaking.com/design_patterns/state and 
 * http://gameprogrammingpatterns.com/state.html for more info on the State Pattern */

public class GuardStateMachine : MonoBehaviour {

    //public enum PatrolStyle
    //{
    //   BackAndForth,
    //   Loop,
    //   Stationary
    //}

   // public PatrolStyle patrolStyle = PatrolStyle.Stationary;
    public float rotationSpeed;

    private GuardState state;
    public int LastWaypointIndex { get { return lastWaypointIndex; } set { lastWaypointIndex = value; } }
    private int lastWaypointIndex = 0;
    public GameObject TargetPlayer { get { return targetPlayer; } set { targetPlayer = value; } }
    private GameObject targetPlayer;
    public NavMeshAgent NavigationAgent { get { return agent; }  set { } }
    private NavMeshAgent agent;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        GoToState(new PatrollingState(this));
    }

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

    public void Distract(Vector3 position)
    {
        state.OnDistraction(position);
    }

    public void Alert()
    {
        state.OnSeePlayer();
    }

    public void PlayerLost(Vector3 lastKnownPosition)
    {
        GoToState(new SearchingGuardState(this, lastKnownPosition));
    }
    
    public void Shoot()
    {
        Debug.Log("Bang!");
    }
}
