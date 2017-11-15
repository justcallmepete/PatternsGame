﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

   

    /****************************
     *     INSPECTOR VALUES     *
     ****************************/
    [Header("Pathing Options")]
    public PatrolStyle patrolStyle;
    [SerializeField]
    private GuardPatrol patrol;
    public float rotationSpeed;

    [Header("Chasing Options")]
    [Range(1.0f, 10.0f)]
    public float stoppingDistance;

    [Header("General")]
    public GameObject indicator;

    [Header("Debug Options")]
    [SerializeField]
    private bool logStateTransitions;

    //Private values
    [SerializeField]
    private GuardState state;

    //Properties
    public Vector3 StartLocation { get { return startLocation; } set { startLocation = value; } }
    private Vector3 startLocation; // Save the spawn location so guards can return to it.
    public Vector3 StartRotation { get { return startRotation; } set { startRotation = value; } }
    private Vector3 startRotation; // Save the start rotation so the guard faces the right way spawn.
    public int LastWaypointIndex { get { return lastWaypointIndex; } set { lastWaypointIndex = value; } }
    private int lastWaypointIndex = 0;
    public GameObject TargetPlayer { get { return targetPlayer; } set { targetPlayer = value; } }
    private GameObject targetPlayer;
    public NavMeshAgent NavigationAgent { get { return agent; }  set { } }
    private NavMeshAgent agent;
    public Color IndicatorColor { get { return indicator.GetComponent<MeshRenderer>().material.color; } set { indicator.GetComponent<MeshRenderer>().material.color = value;  } }
    public GuardPatrol PatrolRoute { get { return patrol; } set { patrol = value; } }

    void Awake () {
        StartLocation = transform.position;
        StartRotation = transform.forward;
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
        TargetPlayer = player;
        state.OnSeePlayer(player);
    }

    //Call if player is no longer visible (Go to searching state)
    public void PlayerLost(Vector3 lastKnownPosition)
    {
        TargetPlayer = null;
        GoToState(new SearchingGuardState(this, lastKnownPosition));
    }
    
    // Call if one of the players is shot.
    public void Shoot()
    {
        GameManager.Instance.ReloadScene();
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
