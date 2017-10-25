using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/** This class is used as the Context class for the Guard State Machine 
 * See https://sourcemaking.com/design_patterns/state and 
 * http://gameprogrammingpatterns.com/state.html for more info on the State Pattern */

public class GuardStateMachine : MonoBehaviour {

    public enum PatrolStyle
    {
       BackAndForth,
       Loop,
       Roaming
    }

    public PatrolStyle patrolStyle = PatrolStyle.Loop;

    //Inspector
    public float rotationSpeed;
    public GameObject indicator;

    private GuardState state;

    //Properties
    public int LastWaypointIndex { get { return lastWaypointIndex; } set { lastWaypointIndex = value; } }
    private int lastWaypointIndex = 0;
    public GameObject TargetPlayer { get { return targetPlayer; } set { targetPlayer = value; } }
    private GameObject targetPlayer;
    public NavMeshAgent NavigationAgent { get { return agent; }  set { } }
    private NavMeshAgent agent;
    public Color IndicatorColor { get { return indicator.GetComponent<MeshRenderer>().material.color; } set { indicator.GetComponent<MeshRenderer>().material.color = value;  } }

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        GoToState(new PatrollingState(this));
    }

    void Update () {
        state.Update();
    }

    //Call when the Guard sees or hears a distraction. (Go to Searching state)
    public void Distract(Vector3 position)
    {
        state.OnDistraction(position);
    }

    //Call when guard can currently see a player. (Go to Alert state)
    public void Alert(GameObject player)
    {
        state.OnSeePlayer(player);
    }

    //Call if player is no longer visible (Go to searching state)
    public void PlayerLost(Vector3 lastKnownPosition)
    {
        GoToState(new SearchingGuardState(this, lastKnownPosition));
    }
    
    // Shoot a player.
    public void Shoot()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}
