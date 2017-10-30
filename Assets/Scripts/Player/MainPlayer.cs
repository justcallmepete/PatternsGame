using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
* The main player class to control the state of the player and other behaviours. Alpha Fading, 
* Pulling and Teleporting are the other behaviours.
*/

[RequireComponent(typeof(InputComponent))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(WhistleComponent))]
[RequireComponent(typeof(DetectionComponent))]
[RequireComponent(typeof(Rigidbody))]

public class MainPlayer : MonoBehaviour
{
    [HideInInspector]
    public PlayerComponent[] components;
    [HideInInspector]
    public Rigidbody rigidBody;
    [Header("General settings")]
    // Animation curve for pulling
    public AnimationCurve pullEasing;

    public enum PlayerIndex
    {
        P1, P2
    }

    [Header("Player settings")]
    public PlayerIndex playerIndex;

    [SerializeField]
    private float outerRadius = 1.3f;
    [SerializeField]
    private float pullSpeed = 1;
    [SerializeField]
    private float teleportDelay = 1;

    private Controlable controlable;
    private Material material;
    public float alphaSpeed = 1f;    // How fast alpha value decreases.
    private Color color;            // Used to store color reference.

    private InputComponent inputComponent;
    public enum State
    {
        Idle,
        Busy,
        Pulled,
        Teleported,
        Channelling
    }

    /*
     * 0 - X
     * 1 - A
     * 2 - B 
     * 3 - Y
     * 4 - L1
     * 5 - R1
     * 6 - L2
     * 7 - R2
     * 8 - Select
     * 9 - Start
     */
     
    [HideInInspector]
    public bool[] buttonDownList = new bool[10];
    [HideInInspector]
    public bool[] buttonList = new bool[10];
    [HideInInspector]
    public Vector3 axisDirection = Vector3.zero;


    private State currentState = State.Idle;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    private void Awake()
    {
        components = gameObject.GetComponents<PlayerComponent>();
        foreach(PlayerComponent component in components)
        {
            component.MainPlayer = this;
            component.AwakeComponent();
        }
        rigidBody = gameObject.GetComponent<Rigidbody>();
      
    }

    private void Start()
    {
        // Cache components for later use
        controlable = gameObject.GetComponent<Controlable>();
        material = gameObject.GetComponent<MeshRenderer>().material;
        color = material.color;
    }

    private void Update()
    {
        foreach (PlayerComponent component in components)
        {
            component.UpdateComponent();
        }
    }

    private void FixedUpdate()
    {
        foreach (PlayerComponent component in components)
        {
            component.FixedUpdateComponent();
        }
    }


    #region Pull Mechanic
    public void BePulled(GameObject obj, float maxDistance)
    {
        currentState = State.Busy;

        StartCoroutine(PullPlayer(obj, maxDistance));
    }

    private IEnumerator PullPlayer(GameObject obj, float maxDistance)
    {
        // Wait for pulling
        yield return new WaitForSeconds(teleportDelay);

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
        currentState = State.Idle;
    }

    public void PullPlayer(GameObject obj)
    {
        currentState = State.Busy;

        StartCoroutine(WaitForPlayer(obj));
    }

    #endregion

    public IEnumerator StartTeleport(GameObject pPlayer)
    {
        currentState = State.Busy;
        StartCoroutine(AlphaFade());
        // Wait for teleport
        yield return new WaitForSeconds(teleportDelay);

        Vector3 path = pPlayer.transform.position + (gameObject.transform.position - pPlayer.transform.position).normalized * outerRadius;
        gameObject.transform.position = path;

        StartCoroutine(AlphaFade(1));
        currentState = State.Idle;
    }
    
    private IEnumerator WaitForPlayer(GameObject obj)
    {

        while (obj.GetComponent<MainPlayer>().IsBusy())
        {
            yield return new WaitForEndOfFrame();
        }

        currentState = State.Idle;
    }

    public bool IsBusy()
    {
        return CurrentState == State.Busy;
    }

    public bool IsFree()
    {
        return CurrentState == State.Idle;
    }

    public bool IsChannelling()
    {
        return CurrentState == State.Channelling;
    }

    private IEnumerator AlphaFade(float pAlpha = 0)
    {
        // Alpha start value.
        float currentAlpha = material.color.a;

        if (pAlpha < currentAlpha) {
            // Loop until aplha is below zero (completely invisalbe)
            while (currentAlpha > pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha -= alphaSpeed * Time.deltaTime;
                Debug.Log(currentAlpha);
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
        else
        {
            while (currentAlpha < pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha += alphaSpeed * Time.deltaTime;
                Debug.Log(currentAlpha);
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
    }
}
