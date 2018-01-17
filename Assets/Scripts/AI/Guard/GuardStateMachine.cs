using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/** This class is used as the Context class for the Guard State Machine 
 * See https://sourcemaking.com/design_patterns/state and 
 * http://gameprogrammingpatterns.com/state.html for more info on the State Pattern */

public class GuardStateMachine : MonoBehaviour {

    public enum PatrolStyle
    {
        Stationary,
        BackAndForth,
        Loop,
        Roaming
    }

    public enum IndicatorImage
    {
        None,
        Blinded,
        Searching,
        Alert
    }



    /****************************
     *     INSPECTOR VALUES     *
     ****************************/
    [Header("Pathing Options")]
    public PatrolStyle patrolStyle;
    [SerializeField]
    private GuardPatrol patrol;

    [Header("Speed Options")]
    public float cluelessMovementSpeed;
    public float searchingMovementSpeed;
    public float alertMovementSpeed;

    public float cluelessRotationSpeed;
    public float searchingRotationSpeed;
    public float alertRotationSpeed;

    //public float rotationSpeed;

    [Header("Chasing Options")]
    [Range(1.0f, 10.0f)]
    public float stoppingDistance;
    [Tooltip("How long does the guard still know where the player is after he has lost sight")]
    public float chaseTime;

    [Header("General")]
    public Image indicator;
    public Sprite[] indicators;
    public GameObject vision;

    [Header("Debug Options")]
    [SerializeField]
    private bool logStateTransitions;

    //Private values
    [SerializeField]
    private GuardState state;
    bool inLight = true;

    //Properties
    public bool InLight { get { return inLight; } set { inLight = value; } }
    public Vector3 StartLocation { get { return startLocation; } set { startLocation = value; } }
    private Vector3 startLocation; // Save the spawn location so guards can return to it.
    public Vector3 StartRotation { get { return startRotation; } set { startRotation = value; } }
    private Vector3 startRotation; // Save the start rotation so the guard faces the right way spawn.
    public int LastWaypointIndex { get { return lastWaypointIndex; } set { lastWaypointIndex = value; } }
    private int lastWaypointIndex = 0;
    public bool PlayerVisible { get { return playerVisible; } set { } }
    private bool playerVisible = false;
    public GameObject TargetPlayer { get { return targetPlayer; } set { targetPlayer = value; } }
    private GameObject targetPlayer;
    public NavMeshAgent NavigationAgent { get { return agent; } set { } }
    private NavMeshAgent agent;
    public Color VisionColor { get {return vision.GetComponentInChildren<MeshRenderer>().material.color; } set { vision.GetComponentInChildren<MeshRenderer>().material.color = value; } }
    private IndicatorImage indicatorImage = IndicatorImage.None;
    public IndicatorImage Indicator { get { return indicatorImage; } set {
            switch (value)
            {
                case IndicatorImage.None:
                    indicator.gameObject.SetActive(false);
                    break;
                case IndicatorImage.Blinded:
                    indicator.gameObject.SetActive(true);
                    indicator.sprite = indicators[0];
                    indicator.color = Color.white;
                     break;
                case IndicatorImage.Searching:
                    indicator.gameObject.SetActive(true);
                    indicator.sprite = indicators[1];
                    indicator.color = Color.yellow;
                    break;
                case IndicatorImage.Alert:
                    indicator.gameObject.SetActive(true);
                    indicator.sprite = indicators[2];
                    indicator.color = Color.red;
                    break;
                default:
                    indicator.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public GuardPatrol PatrolRoute { get { return patrol; } set { patrol = value; } }
    public float MovementSpeed { get { return NavigationAgent.speed; } set { NavigationAgent.speed = value; } }
    private float rotationSpeed;
    public float RotationSpeed
    {
        get
        {
            if (state is AlertGuardState) { return alertRotationSpeed; }
            else if (state is SearchingGuardState) { return searchingRotationSpeed; }
            else return cluelessRotationSpeed;
        }
        set { }
    }


  

    void Awake () {
        StartLocation = transform.position;
        StartRotation = transform.forward;
        agent = GetComponent<NavMeshAgent>();
        GoToState(new PatrollingState(this));
    }

    void Update () {
        if (!inLight)
        {
            GoToState(new BlindedGuardState(this));
        }
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
        TargetPlayer = player;
        playerVisible = true;
        state.OnSeePlayer(player);
    }

    //Call if player is no longer visible (Go to searching state)
    public void PlayerLost(Vector3 lastKnownPosition)
    {
        playerVisible = false;
    }

    public void ForgetPlayer()
    {
        Vector3 lastPlayerPosition = TargetPlayer.transform.position;
        TargetPlayer = null;
        GoToState(new SearchingGuardState(this, lastPlayerPosition));
    }
    
    // Call if one of the players is shot.
    public void Shoot()
    {
       // GameManager.Instance.ReloadCheckpoint();
    }

    public void Reset()
    {
        this.NavigationAgent.isStopped = true;
       
        this.transform.position = startLocation;
        this.transform.eulerAngles = startRotation;
        TargetPlayer = null;
        GoToState(new PatrollingState(this));

    }

    public void GoToState(GuardState newState)
    {
        if (state != null)
        {
            state.OnStateExit();
        }
        this.state = newState;
        state.OnStateEnter();
        
        //Debug Option
        if (logStateTransitions)
        {
            Debug.Log("Transitioned to: " + state.ToString());
        }
    }
}
